using PdfTool.Constants;

namespace PdfTool.Models;

internal sealed record AppSettings {
    public PageSize PageSize { get; set; } = DefaultPageSizes.A4;

    public bool MaintainAspectRatio { get; set; } = true;

    public string MergedFilename { get; set; } = "Merged";
}
