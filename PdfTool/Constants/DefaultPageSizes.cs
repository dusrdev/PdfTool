using PdfTool.Models;

namespace PdfTool.Constants;

internal static class DefaultPageSizes {
    public static readonly PageSize A4 = new() { Height = 842, Width = 595 };
    public static readonly PageSize Letter = new() { Height = 792, Width = 612 };
}
