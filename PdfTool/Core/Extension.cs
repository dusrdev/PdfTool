using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PdfTool.Core;

public static class Extension {
    public static void Toggle(this ProgressBar progressBar, bool isVisible, SolidColorBrush foreground) {
        if (isVisible) {
            progressBar.Foreground = foreground;
            progressBar.IsIndeterminate = true;
            progressBar.Visibility = Visibility.Visible;
        } else {
            progressBar.IsIndeterminate = false;
            progressBar.Visibility = Visibility.Collapsed;
        }
    }
}