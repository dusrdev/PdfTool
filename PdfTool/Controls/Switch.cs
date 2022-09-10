using System.Windows.Controls;
using System.Windows.Media;

namespace PdfTool.Controls;
public class Switch : Button {
    public bool IsChecked { get; set; } = true;

    public Switch() {
        Click += Switch_Click;
        MouseEnter += Switch_MouseEnter;
        MouseLeave += Switch_MouseLeave;
        SetColors();
    }

    private void Switch_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) => BorderBrush = IsChecked switch {
        true => Brushes.Navy,
        _ => Brushes.Gray
    };

    private void Switch_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) => BorderBrush = IsChecked switch {
        true => Brushes.White,
        _ => Brushes.Navy
    };

    private void Switch_Click(object sender, System.Windows.RoutedEventArgs e) {
        IsChecked = !IsChecked;
        SetColors();
    }

    private void SetColors() {
        Background = IsChecked switch {
            true => Brushes.Navy,
            _ => Brushes.White
        };
        BorderBrush = IsChecked switch {
            true => Brushes.Navy,
            _ => Brushes.Gray
        };
    }
}
