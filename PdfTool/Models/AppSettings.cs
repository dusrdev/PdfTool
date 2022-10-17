namespace PdfTool.Models;

internal sealed record AppSettings {
    public ImageConversionMode ConversionMode { get; set; }

    public PdfAction Action { get; set; }

    public string MergedFilename { get; set; } = "Merged";
}
