using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace PdfTool.Controls;

public class Status : Border {
    private readonly TextBlock _textBlock;
    public TimeSpan CollapseDelay { get; set; }

    public Status() {
        Collapse();
        CollapseDelay = TimeSpan.FromSeconds(3);
        _textBlock = new TextBlock {
            FontWeight = FontWeights.SemiBold
        };
        Child = _textBlock;
    }

    public void Collapse() => Visibility = Visibility.Collapsed;

    public async Task DelayCollapse() {
        await Task.Delay(CollapseDelay);
        Collapse();
    }

    public void Update(string status, bool isSuccess) {
        if (Visibility is Visibility.Collapsed) {
            Visibility = Visibility.Visible;
        }

        _textBlock.Text = status;

        Background = isSuccess switch {
            true => Brushes.SpringGreen,
            _ => Brushes.Crimson
        };

        _ = DelayCollapse();
    }
}
