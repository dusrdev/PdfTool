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
    private readonly PdfSplitter _splitter;
    private readonly AppSettings _settings;

    public MainWindow() {
        InitializeComponent();
        _settings = new AppSettings();
        _imageConverter = new ImageToPdfConverter(_settings);
        _merger = new PdfMerger(_settings);
        _splitter = new PdfSplitter();
    }

    /// <summary>
    /// Merges multiple files to one file
    /// </summary>
    /// <param name="dataObject"></param>
    private async Task InvokePdfAction(IDataObject dataObject) {
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        var validation = FileValidators.AreFilesValid(ref filePaths, SupportedExtensions.Pdf);

        if (!validation.Success) {
            Status.Update(validation.Message, false);
            return;
        }

        var result = _settings.Action switch {
            PdfAction.Merge => await _merger.MergeDocumentsAsync(filePaths, TxtMergedFileName.Text),
            _ => await _splitter.SplitPdfAsync(filePaths)
        };

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

    private async void PdfActionBorder_Drop(object sender, DragEventArgs e) {
        await InvokePdfAction(e.Data);
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

    private void SliderMode_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        _settings.Action = (PdfAction)(int)SliderMode.Value;
        TxtPdfAction.Text = _settings.Action switch {
            PdfAction.Merge => "Merge Pdfs",
            _ => "Split Pdf"
        };
    }

    private void SliderImageConvertMode_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        _settings.ConversionMode = (ImageConversionMode)(int)SliderMode.Value;
    }
}
