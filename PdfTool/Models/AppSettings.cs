namespace PdfTool.Models;

internal sealed class AppSettings {
    private ImageConversionMode _conversionMode;
    private PdfAction _action;

    public ImageConversionMode ConversionMode {
        get => _conversionMode;
        set {
            _conversionMode = value;
            Properties.Settings.Default[nameof(ImageConversionMode)] = (int)value;
            Properties.Settings.Default.Save();
        }
    }

    public PdfAction Action {
        get => _action;
        set {
            _action = value;
            Properties.Settings.Default[nameof(PdfAction)] = (int)value;
            Properties.Settings.Default.Save();
        }
    }

    public string MergedFilename { get; set; } = "Merged";

    public AppSettings() {
        _conversionMode = (ImageConversionMode)Properties.Settings.Default[nameof(ImageConversionMode)];
        _action = (PdfAction)Properties.Settings.Default[nameof(PdfAction)];
    }
}
