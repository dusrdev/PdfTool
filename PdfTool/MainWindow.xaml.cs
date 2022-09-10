using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PdfTool;

public partial class MainWindow : Window {
    private const int A4Height = 842;
    private const int A4Width = 595;
    private const int A4Area = 595 * 842;
    private HashSet<string> _imageExtensions;
    private HashSet<string> _pdf;
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(3);

    public MainWindow() {
        InitializeComponent();

        var provider = CodePagesEncodingProvider.Instance;

        Encoding.RegisterProvider(provider);

        _imageExtensions = new HashSet<string>() {
            ".jpeg", ".jpg", ".png", ".tif", ".tiff"
        };

        _pdf = new HashSet<string>() {
            ".pdf"
        };
    }

    /// <summary>
    /// Merges multiple files to one file
    /// </summary>
    /// <param name="dataObject"></param>
    private async Task MergeFiles(IDataObject dataObject) {
        if (!dataObject.GetDataPresent(DataFormats.FileDrop)) {
            Status.Update("File drop has no data!", false);
            await CollapseStatus();
            return;
        }

        // Note that you can have more than one file.
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        if (filePaths.Length == 0) {
            Status.Update("No files were found!", false);
            await CollapseStatus();
            return;
        }

        if (!ExtensionsValid(ref filePaths, ref _pdf)) {
            Status.Update("Selected files are invalid!", false);
            await CollapseStatus();
            return;
        }

        var directory = Path.GetDirectoryName(filePaths[0]);
        var fileName = ForceFileName();
        var newFileName = await NewFileName(fileName, directory!);
        var filePath = Path.Combine(directory!, newFileName);
        var outputPath = Path.ChangeExtension(filePath, ".pdf");

        if (File.Exists(outputPath)) {
            fileName += "(1)";
            filePath = Path.Combine(directory!, fileName);
            outputPath = Path.ChangeExtension(filePath, ".pdf");
        }

        PdfDocument document = new PdfDocument();
        document.Info.Title = fileName;

        foreach (string path in filePaths) {
            PdfDocument inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);

            foreach (var page in inputDocument.Pages) {
                document.AddPage(page);
            }
        }

        if (document.PageCount == 0) {
            Status.Update("Document processing failed!", false);
            await CollapseStatus();
            return;
        }

        document.Save(outputPath);

        Status.Update("Merge successful.", true);
        await CollapseStatus();
    }

    /// <summary>
    /// Converts images to pdfs
    /// </summary>
    /// <param name="dataObject"></param>
    private async Task ConvertImages(IDataObject dataObject) {
        if (!dataObject.GetDataPresent(DataFormats.FileDrop)) {
            Status.Update("File drop has no data!", false);
            await CollapseStatus();
            return;
        }

        // Note that you can have more than one file.
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        if (filePaths.Length == 0) {
            Status.Update("No files were found!", false);
            await CollapseStatus();
            return;
        }

        if (!ExtensionsValid(ref filePaths, ref _imageExtensions)) {
            Status.Update("Selected files are invalid!", false);
            await CollapseStatus();
            return;
        }

        foreach (string path in filePaths) {
            PdfDocument documents = new PdfDocument();
            documents.Info.Title = Path.GetFileNameWithoutExtension(path);

            var isVertical = true;
            PdfPage page = documents.AddPage();
            XImage image = XImage.FromFile(path);
            if (image.PixelWidth > image.PixelHeight) {
                page.Orientation = PdfSharp.PageOrientation.Landscape;
                isVertical = false;
            }
            XGraphics gfx = XGraphics.FromPdfPage(page);

            await DrawImage(gfx, image, isVertical, 0, 0, (int)page.Width, (int)page.Height);

            string resultPath = Path.ChangeExtension(path, ".pdf");

            if (File.Exists(resultPath)) {
                File.Delete(resultPath);
            }

            if (documents.PageCount > 0) documents.Save(resultPath);
        }

        Status.Update("Conversion successful.", true);
        await CollapseStatus();
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
        if (BtnMaintainAspectRatio.IsChecked) {
            if (image.PixelHeight <= A4Height && image.PixelWidth <= A4Width) //A4 height and width
                gfx.DrawImage(image, x, y); // don't scale
            else {
                var inner = image.PixelWidth / (double)image.PixelHeight * A4Area;
                var Width = Math.Sqrt(inner);
                var Height = Width * image.PixelHeight / image.PixelWidth;
                var ScaledMargin = isVertical ? A4Width : A4Height;
                if (Width > ScaledMargin) {
                    var ratio = Width / ScaledMargin;
                    Width /= ratio;
                    Height /= ratio;
                }
                gfx.DrawImage(image, x, y, (int)Width, (int)Height);//scale to A4
            }
        } else {
            gfx.DrawImage(image, x, y, width, height);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// In case there are multiple copies of the same file or new instances with a number attached, it will add a number to the last
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="directory"></param>
    /// <returns></returns>
    private static Task<string> NewFileName(string fileName, string directory) {
        var files = Directory.GetFiles(directory);

        var matchingFiles = new List<string>();

        foreach (var file in files) {
            if (file.Contains(fileName)) {
                matchingFiles.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        if (matchingFiles.Count == 0) {
            return Task.FromResult(fileName);
        }

        matchingFiles.Sort();

        return Task.FromResult(matchingFiles.Last() + "(1)");
    }

    private async Task CollapseStatus() {
        await Task.Delay(_delay);
        Status.Collapse();
    }

    private async void MergeBorder_Drop(object sender, DragEventArgs e) {
        await MergeFiles(e.Data);
        OnDragLeave(sender, e);
    }

    private async void ConvertBorder_Drop(object sender, DragEventArgs e) {
        await ConvertImages(e.Data);
        OnDragLeave(sender, e);
    }

    private void OnDragEnter(object sender, DragEventArgs e) {
        if (sender is not Border border) {
            return;
        }
        border.Background = Brushes.Tomato;
    }

    private void OnDragLeave(object sender, DragEventArgs e) {
        if (sender is not Border border) {
            return;
        }
        border.Background = Brushes.Lavender;
    }

    /// <summary>
    /// Validates that all files have an acceptable extensions
    /// </summary>
    /// <param name="files"></param>
    /// <param name="extensions"></param>
    /// <returns></returns>
    private static bool ExtensionsValid(ref string[] files, ref HashSet<string> extensions) {
        foreach (var file in files) {
            var ext = Path.GetExtension(file);
            if (!extensions.Contains(ext)) {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Forces filename if the custom textbox is empty
    /// </summary>
    /// <returns></returns>
    private string ForceFileName() => string.IsNullOrWhiteSpace(TxtMergedFileName.Text) ? "Merged" : TxtMergedFileName.Text;
}
