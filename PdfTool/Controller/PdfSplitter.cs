using System.Collections.ObjectModel;
using System.IO;
using System.Text;

using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

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
    public static Result SplitPdf(ReadOnlyCollection<string> filePaths) {
        if (filePaths.Count > 1) {
            return Result.Fail("Only one file can be split at a time.");
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

        return Result.Ok("Pdf splitted successfully.");
    }
}
