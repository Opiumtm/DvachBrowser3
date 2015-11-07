using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DvachBrowser3.TextRender;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Фабрика элементов для рендеринга текста.
    /// </summary>
    public sealed class RenderTextElementFactory : ICanvasElementFactory
    {
        private readonly ILinkClickCallback linkClickCallback;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="linkClickCallback">Обратный вызов клика на ссылку.</param>
        public RenderTextElementFactory(ILinkClickCallback linkClickCallback)
        {
            this.linkClickCallback = linkClickCallback;
        }

        /// <summary>
        /// Создать элемент.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <returns>Элемент.</returns>
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
                Foreground = Application.Current.Resources["PostNormalTextBrush"] as Brush,
                Text = text,
                TextWrapping = TextWrapping.NoWrap,
                TextTrimming = TextTrimming.None,
                FontSize = 14.5,
                TextLineBounds = TextLineBounds.Full,
                IsTextSelectionEnabled = false
            };

            FrameworkElement result = r;

            Border b = new Border();
            bool needBorder = false;

            Grid g = new Grid();
            bool needGrid = false;

            Grid g2 = new Grid();
            bool needOuterGrid = false;


            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Bold))
            {
                r.FontWeight = FontWeights.Bold;
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Italic))
            {
                r.FontStyle = FontStyle.Italic;
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Fixed))
            {
                r.FontFamily = new FontFamily("Courier New");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Spoiler))
            {
                needBorder = true;
                b.Background = Application.Current.Resources["PostSpoilerBackgroundBrush"] as Brush;
                r.Foreground = Application.Current.Resources["PostSpoilerTextBrush"] as Brush;
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Quote))
            {
                r.Foreground = Application.Current.Resources["PostQuoteTextBrush"] as Brush;
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Link))
            {
                r.Foreground = Application.Current.Resources["PostLinkTextBrush"] as Brush;
            }

            b.BorderBrush = r.Foreground;
            b.BorderThickness = new Thickness(0);

            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Undeline) || command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Link))
            {
                needBorder = true;
                b.BorderThickness = new Thickness(b.BorderThickness.Left, b.BorderThickness.Top, b.BorderThickness.Right, 1.2);
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Overline))
            {
                needBorder = true;
                b.BorderThickness = new Thickness(b.BorderThickness.Left, 1.2, b.BorderThickness.Right, b.BorderThickness.Bottom);
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Strikethrough))
            {
                needGrid = true;
                g.Children.Add(new Border()
                {
                    Background = r.Foreground,
                    Height = 1.2,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                });
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Subscript) || command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Superscript))
            {
                needOuterGrid = true;
                r.Measure(new Size(0, 0));
                var fh = r.ActualHeight;
                r.FontSize = r.FontSize * 2.0 / 3.0;
                r.Measure(new Size(0, 0));
                var fh2 = r.ActualHeight;
                var delta = fh - fh2;
                if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Subscript) &&
                    !command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Superscript))
                {
                    g2.Padding = new Thickness(0, delta, 0, 0);
                }
                else if (!command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Subscript) &&
                         command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Superscript))
                {
                    g2.Padding = new Thickness(0, 0, 0, delta);
                }
                else
                {
                    g2.Padding = new Thickness(0, delta / 2, 0, delta / 2);
                }
            }

            if (needGrid)
            {
                g.Children.Add(result);
                result = g;
            }
            if (needBorder)
            {
                b.Child = result;
                result = b;
            }
            if (needOuterGrid)
            {
                g2.Children.Add(result);
                result = g2;
            }

            result.Measure(new Size(0, 0));

            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Link))
            {
                var linkAttribute = command.Attributes.Attributes[CommonTextRenderAttributes.Link] as ITextRenderLinkAttribute;
                if (linkAttribute != null)
                {
                    result.Tapped += (sender, e) =>
                    {
                        e.Handled = true;
                        linkClickCallback?.OnLinkClick(linkAttribute);
                    };
                }
            }

            return result;
        }
    }
}