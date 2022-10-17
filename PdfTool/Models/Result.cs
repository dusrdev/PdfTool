namespace PdfTool.Models;

/// <summary>
/// A result container with message and status
/// </summary>
/// <typeparam name="T">Type of inner value</typeparam>
internal readonly record struct Result {
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public readonly bool Success { get; init; }

    /// <summary>
    /// Operation message/output
    /// </summary>
    public readonly string Message { get; init; }
}
