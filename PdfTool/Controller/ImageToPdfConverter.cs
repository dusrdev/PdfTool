using PdfSharpCore.Drawing;

using PdfTool.Models;

namespace PdfTool.Controller;

/// <summary>
/// Converts images to pdf
/// </summary>
internal sealed class ImageToPdfConverter {
    private readonly AppSettings _settings;

    public ImageToPdfConverter(AppSettings settings) {
        _settings = settings;
    }

    public async Task<Result> ConvertImagesAsync(IEnumerable<string> filePaths) {

    }

    /// <summary>
    /// Draws image to pdf page
    /// </summary>
    /// <param name="gfx"></param>
    /// <param name="image"></param>
    /// <param name="isVertical"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private Task DrawImage(XGraphics gfx, XImage image, bool isVertical, int x, int y, int width, int height) {
        if (!_settings.MaintainAspectRatio) {
            gfx.DrawImage(image, x, y, width, height);
            return Task.CompletedTask;
        }
        
        if (image.PixelHeight <= _settings.PageSize.Height && image.PixelWidth <= _settings.PageSize.Width) { //A4 height and width
            gfx.DrawImage(image, x, y); // don't scale
            return Task.CompletedTask;
        }

        var inner = image.PixelWidth / (double)image.PixelHeight * _settings.PageSize.Area;
        var Width = Math.Sqrt(inner);
        var Height = Width * image.PixelHeight / image.PixelWidth;
        var ScaledMargin = isVertical ? _settings.PageSize.Width : _settings.PageSize.Height;
        if (Width > ScaledMargin) {
            var ratio = Width / ScaledMargin;
            Width /= ratio;
            Height /= ratio;
        }
        if (isVertical) {
            x += (int)((_settings.PageSize.Width - Width) / 2);
            y += (int)((_settings.PageSize.Height - Height) / 2);
        } else {
            x += (int)((_settings.PageSize.Height - Width) / 2);
            y += (int)((_settings.PageSize.Width - Height) / 2);
        }
        gfx.DrawImage(image, x, y, (int)Width, (int)Height);//scale to A4
        return Task.CompletedTask;
    }
}
