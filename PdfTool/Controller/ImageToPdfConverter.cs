using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfTool.Models;
using PdfSharpCore;
using System.Text;
using PdfTool.Extensions;

namespace PdfTool.Controller;

/// <summary>
/// Converts images to pdf
/// </summary>
internal sealed class ImageToPdfConverter {
    private readonly AppSettings _settings;
    private static Func<PdfPage, XImage, Task>? _selectedConverter;

    public ImageToPdfConverter(AppSettings settings) {
        _settings = settings;
        var provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);
    }

    /// <summary>
    /// Batch converts images to pdfs and saves them to the same directory as the first file
    /// </summary>
    /// <param name="filePaths"></param>
    public async Task<Result> ConvertImagesAsync(string[] filePaths) {
        _selectedConverter = _settings.ConversionMode switch {
            ImageConversionMode.Full => DrawImageFull,
            _ => DrawImageFit
        };

        var tasks = filePaths.Select(x => new Func<Task>(() => ConvertImageAsync(x)));
        await tasks.ParallelForEachAsync(-1, async func => await func());

        return new Result {
            Success = true,
            Message = "Conversion successful."
        };
    }

    private async Task ConvertImageAsync(string imagePath) {
        using var document = new PdfDocument();
        document.Info.Title = Path.GetFileNameWithoutExtension(imagePath);

        var page = document.AddPage();
        using var image = XImage.FromFile(imagePath);

        await _selectedConverter!.Invoke(page, image);

        string resultPath = Path.ChangeExtension(imagePath, ".pdf");

        if (File.Exists(resultPath)) {
            File.Delete(resultPath);
        }

        if (document.PageCount > 0) document.Save(resultPath);
    }

    /// <summary>
    /// Draws image to pdf page
    /// </summary>
    /// <param name="image"></param>
    private Task DrawImageFit(PdfPage page, XImage image) {
        if (image.PixelWidth > image.PixelHeight) {
            page.Orientation = PageOrientation.Landscape;
        }
        using var gfx = XGraphics.FromPdfPage(page);

        int x = 0, y = 0;

        var area = page.Width * page.Height;

        var inner = image.PixelWidth / (double)image.PixelHeight * area;
        var Width = Math.Sqrt(inner);
        //var Height = Width * height / width;
        var Height = Width * image.PixelHeight / image.PixelWidth;

        if (Width > page.Width) {
            var ratio = Width / page.Width;
            Width /= ratio;
            Height /= ratio;
        }

        if (Height > page.Height) {
            var ratio = Height / page.Height;
            Height /= ratio;
        }

        if (Height < page.Height) {
            y += (int)((page.Height - Height) / 2);
        }

        if (Width < page.Width) {
            x += (int)((page.Width - Width) / 2);
        }

        gfx.DrawImage(image, x, y, (int)Width, (int)Height);//scale to A4
        return Task.CompletedTask;
    }

    /// <summary>
    /// Draws image to pdf page
    /// </summary>
    /// <param name="image"></param>
    private Task DrawImageFull(PdfPage page, XImage image) {
        page.Width = image.PixelWidth;
        page.Height = image.PixelHeight;
        using var gfx = XGraphics.FromPdfPage(page);
        gfx.DrawImage(image, 0, 0, image.PixelWidth, image.PixelHeight);
        return Task.CompletedTask;
    }
}
