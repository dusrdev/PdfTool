namespace PdfTool.Models;

internal sealed record PageSize {
    public int Height { get; init; }
    public int Width { get; init; }
    public int Area => Height * Width;
}
