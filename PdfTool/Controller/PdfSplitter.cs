#pragma warning disable RCS1229

using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf;
using System.Text;
using System.IO;

namespace PdfTool.Controller;

internal static class PdfSplitter {
    static PdfSplitter() {
        var provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);
    }

    /// <summary>
    /// Merges pdfs and saves them to the same
    /// </summary>
    /// <param name="filePaths"></param>
    public static ValueTask<Result> SplitPdfAsync(string[] filePaths) {
        if (filePaths.Length > 1) {
            return Result.Fail("Only one file can be split at a time.").AsValueTask();
        }

        var directory = Path.GetDirectoryName(filePaths[0]);
        var filename = Path.GetFileNameWithoutExtension(filePaths[0]);

        using var inputDocument = PdfReader.Open(filePaths[0], PdfDocumentOpenMode.Import);
        var count = 1;
        foreach (var page in inputDocument.Pages) {
            using var outputDocument = new PdfDocument();
            outputDocument.AddPage(page);
            var outputFilename = Path.Combine(directory!, $"{filename}-{count}.pdf");
            count++;
            outputDocument.Save(outputFilename);
        }

        return Result.Ok("Pdf splitted successfully.").AsValueTask();
    }
}
