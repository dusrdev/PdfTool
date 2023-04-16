using System.Text;

namespace PdfTool.Controller;

/// <summary>
/// Converts images to pdf
/// </summary>
internal static class ImageToPdfConverter {
    static ImageToPdfConverter() {
        var provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);
    }

    /// <summary>
    /// Batch converts images to pdfs and saves them to the same directory as the first file
    /// </summary>
    /// <param name="filePaths"></param>
    public static Result ConvertImages(string[] filePaths) {
        var converter = new ImageToPdfConvertAction();
        filePaths.Concurrent().ForEach(converter);

        return Result.Ok("Conversion successful.");
    }
}
