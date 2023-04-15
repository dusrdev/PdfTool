using System.Windows.Media;

namespace PdfTool.Core;

public readonly record struct BorderConfig {
    public readonly SolidColorBrush StaticColor { get; init; }
	public readonly SolidColorBrush ActiveColor { get; init; }
}