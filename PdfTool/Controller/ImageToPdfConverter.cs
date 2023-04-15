using System.Text;

namespace PdfTool.Controller;

/// <summary>
/// Converts images to pdf
/// </summary>
internal static class ImageToPdfConverter {
    static ImageToPdfConverter() {
        var provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);
    }

    /// <summary>
    /// Batch converts images to pdfs and saves them to the same directory as the first file
    /// </summary>
    /// <param name="filePaths"></param>
    public static async ValueTask<Result> ConvertImagesAsync(string[] filePaths) {
        // _selectedConverter = _settings.ConversionMode switch {
        //     ImageConversionMode.Full => DrawImageFull,
        //     _ => DrawImageFit
        // };

        // var tasks = filePaths.Select(x => new Func<Task>(() => ConvertImageAsync(x)));
        // await tasks.ParallelForEachAsync(-1, async func => await func());

        var converter = new ImageToPdfConvertAction();
        await filePaths.Concurrent().ForEachAsync(converter);

        return Result.Ok("Conversion successful.");
    }
    /*
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
        var imageWidth = Math.Sqrt(inner);
        var imageHeight = imageWidth * image.PixelHeight / image.PixelWidth;

        if (imageWidth > page.Width) {
            var ratio = imageWidth / page.Width;
            imageWidth /= ratio;
            imageHeight /= ratio;
        }

        if (imageHeight > page.Height) {
            var ratio = imageHeight / page.Height;
            imageHeight /= ratio;
        }

        if (imageHeight < page.Height) {
            y += (int)((page.Height - imageHeight) * 0.5);
        }

        if (imageWidth < page.Width) {
            x += (int)((page.Width - imageWidth) * 0.5);
        }

        gfx.DrawImage(image, x, y, (int)imageWidth, (int)imageHeight); //scale to A4
        return Task.CompletedTask;
    }
    */
}
