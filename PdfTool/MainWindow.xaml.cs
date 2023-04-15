using System.Windows;
using System.Windows.Controls;

using PdfTool.Controller;
using PdfTool.Core;
using PdfTool.Models;
using PdfTool.Validators;

namespace PdfTool;

public partial class MainWindow : Window {
    private readonly AppSettings _settings;

    public MainWindow() {
        InitializeComponent();
        _settings = new AppSettings();
        Initialize();
    }

    /// <summary>
    /// Synchronizes UI with settings
    /// </summary>
    private void Initialize() {
        SliderImageConvertMode.Value = (int)_settings.ConversionMode;
        MergeBorder.Background = Constants.BorderConfigs[nameof(MergeBorder)].StaticColor;
        SplitBorder.Background = Constants.BorderConfigs[nameof(SplitBorder)].StaticColor;
        ConvertBorder.Background = Constants.BorderConfigs[nameof(ConvertBorder)].StaticColor;
    }

    /// <summary>
    /// Merges multiple files to one file
    /// </summary>
    /// <param name="dataObject"></param>
    private async Task MergePdfAction(IDataObject dataObject) {
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        var validation = FileValidators.AreFilesValid(ref filePaths, SupportedExtensions.Pdf);

        if (validation.IsFail) {
            Status.Update(validation);
            return;
        }

        var result = await PdfMerger.MergeDocumentsAsync(filePaths, TxtMergedFileName.Text, _settings);

        Status.Update(result);
    }

    /// <summary>
    /// Splits pdf file into multiple files
    /// </summary>
    /// <param name="dataObject"></param>
    private async Task SplitPdfAction(IDataObject dataObject) {
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        var validation = FileValidators.AreFilesValid(ref filePaths, SupportedExtensions.Pdf);

        if (validation.IsFail) {
            Status.Update(validation);
            return;
        }

        var result = await PdfSplitter.SplitPdfAsync(filePaths);

        Status.Update(result);
    }

    /// <summary>
    /// Converts images to pdfs
    /// </summary>
    /// <param name="dataObject"></param>
    private async Task ConvertImages(IDataObject dataObject) {
        string[] filePaths = (string[])dataObject.GetData(DataFormats.FileDrop);

        var validation = FileValidators.AreFilesValid(ref filePaths, SupportedExtensions.Images);

        if (validation.IsFail) {
            Status.Update(validation);
            return;
        }

        var result = await ImageToPdfConverter.ConvertImagesAsync(filePaths);

        Status.Update(result);
    }

    private async void MergeBorder_Drop(object sender, DragEventArgs e) {
        await MergePdfAction(e.Data);
        OnDragLeave(sender, e);
    }

    private async void ConvertBorder_Drop(object sender, DragEventArgs e) {
        await ConvertImages(e.Data);
        OnDragLeave(sender, e);
    }

    private async void SplitBorder_Drop(object sender, DragEventArgs e) {
        await SplitPdfAction(e.Data);
        OnDragLeave(sender, e);
    }

    private void OnDragEnter(object sender, DragEventArgs e) {
        if (sender is not Border border) {
            return;
        }
        border.Background = Constants.BorderConfigs[border.Name].ActiveColor;
    }

    private void OnDragLeave(object sender, DragEventArgs e) {
        if (sender is not Border border) {
            return;
        }
        border.Background = Constants.BorderConfigs[border.Name].StaticColor;
    }

    private void SliderImageConvertMode_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        _settings.ConversionMode = (ImageConversionMode)(int)SliderImageConvertMode.Value;
    }
}
