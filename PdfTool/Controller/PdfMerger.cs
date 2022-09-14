using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf;

using PdfTool.Models;
using System.IO;
using System.Text;

namespace PdfTool.Controller;

internal sealed class PdfMerger {
    private readonly AppSettings _settings;

    public PdfMerger(AppSettings settings) {
        _settings = settings;
        var provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);
    }

    /// <summary>
    /// Gets a valid filename for the output
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private string GetFileName(string fileName) => string.IsNullOrWhiteSpace(fileName) ? _settings.MergedFilename : fileName;

    /// <summary>
    /// Merges pdfs and saves them to the same
    /// </summary>
    /// <param name="filePaths"></param>
    /// <param name="requestedFileName"></param>
    /// <returns></returns>
    public async Task<Result> MergeDocumentsAsync(string[] filePaths, string requestedFileName) {
        var directory = Path.GetDirectoryName(filePaths[0]);
        var fileName = GetFileName(requestedFileName);
        var newFileName = await NewFileName(fileName, directory!);
        var filePath = Path.Combine(directory!, newFileName);
        var outputPath = Path.ChangeExtension(filePath, ".pdf");

        PdfDocument document = new PdfDocument();
        document.Info.Title = fileName;

        foreach (string path in filePaths) {
            PdfDocument inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);

            foreach (var page in inputDocument.Pages) {
                document.AddPage(page);
            }
        }

        if (document.PageCount == 0) {
            return new Result {
                Success = false,
                Message = "Document processing failed!"
            };
        }

        document.Save(outputPath);

        return new Result {
            Success = true,
            Message = "Merge successful."
        };
    }

    /// <summary>
    /// In case there are multiple copies of the same file, it will add a TimeStamp to new file
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="directory"></param>
    /// <returns></returns>
    private static Task<string> NewFileName(string fileName, string directory) {
        var files = Directory.GetFiles(directory);

        bool exists = false;

        foreach (var file in files) {
            if (file.Contains(fileName)) {
                exists = true;
                break;
            }
        }

        if (!exists) {
            return Task.FromResult(fileName);
        }

        var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        return Task.FromResult($"{fileName}-{timeStamp}");
    }
}
