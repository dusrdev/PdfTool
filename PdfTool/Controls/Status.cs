using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace PdfTool.Controls;

public class Status : Border {
    private readonly TextBlock _textBlock;
    public static readonly TimeSpan CollapseDelay = TimeSpan.FromSeconds(5);

    public Status() {
        Collapse();
        _textBlock = new TextBlock {
            FontWeight = FontWeights.SemiBold
        };
        Child = _textBlock;
    }

    private void Collapse() => Visibility = Visibility.Collapsed;

    public async Task DelayCollapse() {
        await Task.Delay(CollapseDelay);
        Collapse();
    }

    public void Update(Result result) {
        if (Visibility is Visibility.Collapsed) {
            Visibility = Visibility.Visible;
        }

        _textBlock.Text = result.Message;

        Background = result.IsOk switch {
            true => Brushes.Lime,
            _ => Brushes.Crimson
        };

        _ = DelayCollapse();
    }
}
