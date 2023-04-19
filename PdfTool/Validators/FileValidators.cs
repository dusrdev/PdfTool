using System.Collections.ObjectModel;
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
    public static Result AreFilesValid(ReadOnlyCollection<string> filePaths, HashSet<string> extensions) {
        if (filePaths.Count is 0) {
            return Result.Fail("No files selected.");
        }

        int i = 0;

        while (i < filePaths.Count) {
            var path = filePaths[i];
            i++;

            if (!File.Exists(path)) {
                return Result.Fail($"File \"{path}\" does not exist.");
            }

            var extension = Path.GetExtension(path).ToLower();

            if (!extensions.Contains(extension)) {
                return Result.Fail($"File \"{path}\" has an unsupported extension.");
            }
        }

        return Result.Ok("All files are valid.");
    }
}
