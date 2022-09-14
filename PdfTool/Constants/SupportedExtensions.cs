namespace PdfTool.Constants;

internal static class SupportedExtensions {
    public static readonly HashSet<string> Images = new() {
        ".jpeg", ".jpg", ".png", ".tif", ".tiff"
    };

    public static readonly HashSet<string> Pdf = new() {
        ".pdf"
    };
}
