using System.IO;
using System.Text;

using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

using PdfTool.Models;

namespace PdfTool.Controller;

internal static class PdfMerger {
    static PdfMerger() {
        var provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);
    }

    /// <summary>
    /// Merges pdfs and saves them to the same
    /// </summary>
    /// <param name="filePaths"></param>
    /// <param name="requestedFileName"></param>
    /// <param name="appSettings"></param>
    public static async ValueTask<Result> MergeDocumentsAsync(string[] filePaths, string requestedFileName, AppSettings appSettings) {
        var directory = Path.GetDirectoryName(filePaths[0]);
        var fileName = string.IsNullOrWhiteSpace(requestedFileName)
            ? appSettings.MergedFilename
            : requestedFileName;
        var newFileName = await NewFileName(fileName, directory!);
        var filePath = Path.Combine(directory!, newFileName);
        var outputPath = Path.ChangeExtension(filePath, ".pdf");

        using var document = new PdfDocument();
        document.Info.Title = fileName;

        foreach (string path in filePaths) {
            using var inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);

            foreach (var page in inputDocument.Pages) {
                document.AddPage(page);
            }
        }

        if (document.PageCount is 0) {
            Result.Fail("No pages found in the document(s).");
        }

        document.Save(outputPath);

        return Result.Ok("Merge successful.");
    }

    /// <summary>
    /// In case there are multiple copies of the same file, it will add a TimeStamp to new file
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="directory"></param>
    private static Task<string> NewFileName(string fileName, string directory) {
        var fileCount = Directory.GetFiles(directory, $"*{fileName}*").Length;

        if (fileCount is 0) {
            return Task.FromResult(fileName);
        }

        var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        return Task.FromResult($"{fileName}-{timeStamp}");
    }
}
