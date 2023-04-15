namespace PdfTool.Core;

internal static class SupportedExtensions {
    public static readonly HashSet<string> Images = new() {
        ".jpeg", ".jpg", ".png", ".tif", ".tiff", ".bmp"
    };

    public static readonly HashSet<string> Pdf = new() {
        ".pdf"
    };
}