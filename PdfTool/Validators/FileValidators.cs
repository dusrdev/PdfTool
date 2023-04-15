using System.IO;

namespace PdfTool.Validators;

/// <summary>
/// Validates files
/// </summary>
internal static class FileValidators {
    /// <summary>
    /// Checks that the <paramref name="filePaths"/> is not empty and all files are of acceptable <paramref name="extensions"/>
    /// </summary>
    /// <param name="filePaths"></param>
    /// <param name="extensions"></param>
    public static Result AreFilesValid(ReadOnlyMemory<string> filePaths, HashSet<string> extensions) {
        if (filePaths.Length is 0) {
            return Result.Fail("No files selected.");
        }

        int i = 0;

        while (++i < filePaths.Length) {
            var extension = Path.GetExtension(filePaths.Span[i]).ToLower();

            if (extensions.Contains(extension)) {
                continue;
            }

            return Result.Fail("At least one file has an unsupported extension.");
        }

        return Result.Ok("All files have valid extensions.");
    }
}
