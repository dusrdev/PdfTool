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
    /// <param name="files"></param>
    private async ValueTask MergePdfAction(string[] files) {
        var validation = FileValidators.AreFilesValid(files, SupportedExtensions.Pdf);

        if (validation.IsFail) {
            Status.Update(validation);
            return;
        }

        var requestedFileName = TxtMergedFileName.Text;
        var result = await Task.Run(() => PdfMerger.MergeDocuments(files, requestedFileName));

        Status.Update(result);
    }

    /// <summary>
    /// Splits pdf file into multiple files
    /// </summary>
    /// <param name="files"></param>
    private async ValueTask SplitPdfAction(string[] files) {
        var validation = FileValidators.AreFilesValid(files, SupportedExtensions.Pdf);

        if (validation.IsFail) {
            Status.Update(validation);
            return;
        }

        var result = await Task.Run(() => PdfSplitter.SplitPdf(files));

        Status.Update(result);
    }

    /// <summary>
    /// Converts images to pdfs
    /// </summary>
    /// <param name="files"></param>
    private async ValueTask ConvertImages(string[] files) {
        var validation = FileValidators.AreFilesValid(files, SupportedExtensions.Images);

        if (validation.IsFail) {
            Status.Update(validation);
            return;
        }

        var result = await Task.Run(() => ImageToPdfConverter.ConvertImages(files));

        Status.Update(result);
    }

    private async void Border_Drop(object sender, DragEventArgs e) {
        if (sender is not Border border) {
            return;
        }
        var staticColor = Constants.BorderConfigs[border.Name].StaticColor;
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        ProgressBar.Toggle(true, staticColor);
        if (border == MergeBorder) {
            await MergePdfAction(files);
        } else if (border == SplitBorder) {
            await SplitPdfAction(files);
        } else if (border == ConvertBorder) {
            await ConvertImages(files);
        }
        ProgressBar.Toggle(false, staticColor);
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
