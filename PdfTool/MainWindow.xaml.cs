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
    private const int A4Size = 842;

    public MainWindow() {
        InitializeComponent();

        var provider = CodePagesEncodingProvider.Instance;

        Encoding.RegisterProvider(provider);
    }

    private void MergeFiles(IDataObject dataObject) {
        if (!dataObject.GetDataPresent(DataFormats.FileDrop)) {
            return;
        }

        // Note that you can have more than one file.
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        if (filePaths.Length == 0) {
            return;
        }

        var directory = Path.GetDirectoryName(filePaths[0]);
        var fileName = ForceFileName();
        var filePath = Path.Combine(directory!, ForceFileName());
        var outputPath = Path.ChangeExtension(filePath, ".pdf");
        PdfDocument document = new PdfDocument();
        document.Info.Title = fileName;

        foreach (string path in filePaths) {
            if (!path.EndsWith(".pdf")) {
                continue;
            }

            PdfDocument inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);

            foreach (var page in inputDocument.Pages) {
                document.AddPage(page);
            }
        }

        if (document.PageCount == 0) {
            Alert("Error", "Document processing failed!");
            return;
        }

        document.Save(outputPath);
        Alert("Success", "Merge successful.");
    }

    private void ConvertImages(IDataObject dataObject) {
        if (!dataObject.GetDataPresent(DataFormats.FileDrop)) {
            return;
        }

        // Note that you can have more than one file.
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        if (filePaths.Length == 0) {
            return;
        }

        foreach (string path in filePaths) {
            if (!(path.EndsWith(".jpg") || path.EndsWith(".png"))) {
                continue;
            }

            PdfDocument documents = new PdfDocument();
            documents.Info.Title = Path.GetFileNameWithoutExtension(path);

            PdfPage page = documents.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            DrawImage(gfx, path, 0, 0, (int)page.Width, (int)page.Height);

            string resultPath = Path.ChangeExtension(path, ".pdf");

            if (File.Exists(resultPath)) {
                File.Delete(resultPath);
            }

            if (documents.PageCount > 0) documents.Save(resultPath);
        }

        Alert("Success", "Conversion successful.");
    }

    private void DrawImage(XGraphics gfx, string imagePath, int x, int y, int width, int height) {
        XImage image = XImage.FromFile(imagePath);
        if (BtnMaintainAspectRatio.IsChecked) {
            if (image.PixelHeight <= A4Size)//A4 height
                gfx.DrawImage(image, 0, 0); // don't scale
            else {
                double Scale = image.PixelHeight / A4Size;
                gfx.DrawImage(image, 0, 0, (int)(image.PixelWidth / Scale), (int)(image.PixelHeight / Scale));//scale to A4
            }
        } else {
            gfx.DrawImage(image, x, y, width, height);
        }
    }

    private static void Alert(string Title, string Message) => MessageBox.Show(Message, Title, MessageBoxButton.OK);

    private void MergeBorder_Drop(object sender, DragEventArgs e) {
        MergeFiles(e.Data);
        OnDragLeave(sender, e);
    }

    private void ConvertBorder_Drop(object sender, DragEventArgs e) {
        ConvertImages(e.Data);
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

    private string ForceFileName() => string.IsNullOrWhiteSpace(TxtMergedFileName.Text) ? "Merged" : TxtMergedFileName.Text;
}
