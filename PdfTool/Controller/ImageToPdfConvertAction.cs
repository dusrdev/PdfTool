using System.IO;

using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace PdfTool.Controller;

public readonly struct ImageToPdfConvertAction : IAction<string> {
    private readonly ThreadSafe<double> _counter;
    private static readonly IModifier<double> Adder = new Adder();
    private readonly int _total;
    private readonly IProgress<double> _progress;

    public ImageToPdfConvertAction(ThreadSafe<double> counter, int total, IProgress<double> progress) {
        _counter = counter;
        _total = total;
        _progress = progress;
    }

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

        var progress = Adder.Modify(_counter.Value, 1d) * 100d / _total;
        _progress.Report(progress);
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
