using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ImageToPdf;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();

        var provider = CodePagesEncodingProvider.Instance;

        Encoding.RegisterProvider(provider);
    }

    private static void ConvertImages(IDataObject dataObject) {
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

        AlertComplete("Converted files successfully.");
    }

    private static void DrawImage(XGraphics gfx, string jpegsamplepath, int x, int y, int width, int height) {
        XImage image = XImage.FromFile(jpegsamplepath);
        gfx.DrawImage(image, x, y, width, height);
    }

    private static void AlertComplete(string Message) => MessageBox.Show(Message, "Completed", MessageBoxButton.OK);

    private void MergeBorder_Drop(object sender, DragEventArgs e) {

    }

    private void ConvertBorder_Drop(object sender, DragEventArgs e) => ConvertImages(e.Data);

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
}
