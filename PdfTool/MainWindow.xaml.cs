using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PdfTool.Constants;
using PdfTool.Controller;
using PdfTool.Models;
using PdfTool.Validators;

namespace PdfTool;

public partial class MainWindow : Window {
    private readonly ImageToPdfConverter _imageConverter;
    private readonly PdfMerger _merger;
    private readonly AppSettings _settings;

    public MainWindow() {
        InitializeComponent();
        _settings = new AppSettings();
        _imageConverter = new ImageToPdfConverter(_settings);
        _merger = new PdfMerger(_settings);
    }

    /// <summary>
    /// Merges multiple files to one file
    /// </summary>
    /// <param name="dataObject"></param>
    private async Task MergeFiles(IDataObject dataObject) {
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        var validation = FileValidators.AreFilesValid(ref filePaths, SupportedExtensions.Pdf);
        
        if (!validation.Success) {
            Status.Update(validation.Message, false);
            return;
        }

        var result = await _merger.MergeDocumentsAsync(filePaths, TxtMergedFileName.Text);

        Status.Update(result.Message, result.Success);
    }

    /// <summary>
    /// Converts images to pdfs
    /// </summary>
    /// <param name="dataObject"></param>
    private async Task ConvertImages(IDataObject dataObject) {
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        var validation = FileValidators.AreFilesValid(ref filePaths, SupportedExtensions.Images);

        if (!validation.Success) {
            Status.Update(validation.Message, false);
            return;
        }

        var result = await _imageConverter.ConvertImagesAsync(filePaths);

        Status.Update(result.Message, result.Success);
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

    private void BtnMaintainAspectRatio_Click(object sender, RoutedEventArgs e) => _settings.MaintainAspectRatio = BtnMaintainAspectRatio.IsChecked;
}
