namespace PdfTool.Models;

internal sealed record AppSettings {
    public bool MaintainAspectRatio { get; set; } = true;

    public string MergedFilename { get; set; } = "Merged";
}
