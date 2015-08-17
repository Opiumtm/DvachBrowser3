using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DvachBrowser3.TextRender;

namespace TestingApp
{
    public class TestElementFactory : ICanvasElementFactory
    {
        public FrameworkElement Create(ITextRenderCommand command)
        {
            string text;
            var textCnt = command.Content as ITextRenderTextContent;
            if (textCnt != null)
            {
                text = textCnt.Text ?? "";
            }
            else
            {
                text = "";
            }
            var r = new TextBlock()
            {
                Foreground = new SolidColorBrush(Colors.Black), 
                Text = text, 
                TextWrapping = TextWrapping.NoWrap, 
                TextTrimming = TextTrimming.None, 
                FontSize = 24,
                TextLineBounds = TextLineBounds.Full
            };
            r.Measure(new Size(0, 0));
            return r;
        }
    }
}