namespace PdfTool.Models;

internal sealed class AppSettings {
    private ImageConversionMode _conversionMode;

    public ImageConversionMode ConversionMode {
        get => _conversionMode;
        set {
            _conversionMode = value;
            Properties.Settings.Default[nameof(ImageConversionMode)] = (int)value;
            Properties.Settings.Default.Save();
        }
    }

    public AppSettings() {
        _conversionMode = (ImageConversionMode)Properties.Settings.Default[nameof(ImageConversionMode)];
    }
}
