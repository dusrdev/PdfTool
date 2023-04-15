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
    /// <returns></returns>
    public static Result AreFilesValid(ref string[] filePaths, HashSet<string> extensions) {
        if (filePaths.Length is 0) {
            return Result.Fail("No files selected.");
        }

        foreach (var filePath in filePaths) {
            var extension = System.IO.Path.GetExtension(filePath).ToLower();

            if (extensions.Contains(extension)) {
                continue;
            }

            return Result.Fail("At least one file has an unsupported extension.");
        }

        return Result.Ok("All files have valid extensions.");
    }
}
