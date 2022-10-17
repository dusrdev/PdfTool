using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf;
using PdfTool.Models;
using System.Text;
using System.IO;

namespace PdfTool.Controller;

internal sealed class PdfSplitter {
    public PdfSplitter() {
        var provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);
    }

    /// <summary>
    /// Merges pdfs and saves them to the same
    /// </summary>
    /// <param name="filePaths"></param>
    public Task<Result> SplitPdfAsync(string[] filePaths) {
        if (filePaths.Length > 1) {
            return Task.FromResult(new Result {
                Message = "Only one file can be split at a time."
            });
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

        return Task.FromResult(new Result {
            Success = true,
            Message = "Pdf splitted successfuly."
        });
    }
}
