using System.Windows.Media;

namespace PdfTool.Core;

internal static class Constants {
    public static readonly Dictionary<string, BorderConfig> BorderConfigs = new() {
        {
            "ConvertBorder", new BorderConfig {
                StaticColor = Brushes.Lime,
                ActiveColor = Brushes.GreenYellow
            }
        },
        {
            "MergeBorder", new BorderConfig {
                StaticColor = Brushes.DodgerBlue,
                ActiveColor = Brushes.LightBlue
            }
        },
        {
            "SplitBorder", new BorderConfig {
                StaticColor = Brushes.Crimson,
                ActiveColor = Brushes.OrangeRed
            }
        }
    };
}