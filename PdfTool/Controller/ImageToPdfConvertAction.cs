using System.IO;

using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace PdfTool.Controller;

public readonly struct ImageToPdfConvertAction : IAction<string> {
    public void Invoke(string input) {
        using var document = new PdfDocument();
        document.Info.Title = Path.GetFileNameWithoutExtension(input);

        var page = document.AddPage();
        using var image = XImage.FromFile(input, PdfSharpCore.Pdf.IO.enums.PdfReadAccuracy.Strict);

        // Draw direct

        DrawImageFull(page, image);

        string resultPath = Path.ChangeExtension(input, ".pdf");

        if (File.Exists(resultPath)) {
            File.Delete(resultPath);
        }

        if (document.PageCount > 0) {
            document.Save(resultPath);
        }
    }

    /// <summary>
    /// Draws image to pdf page
    /// </summary>
    /// <param name="image"></param>
    private static void DrawImageFull(PdfPage page, XImage image) {
        page.Width = image.PixelWidth;
        page.Height = image.PixelHeight;
        using var gfx = XGraphics.FromPdfPage(page);
        gfx.DrawImage(image, 0, 0, image.PixelWidth, image.PixelHeight);
    }
}
