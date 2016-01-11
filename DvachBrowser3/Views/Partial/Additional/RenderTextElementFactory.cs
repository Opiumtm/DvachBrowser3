using System;
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

        private readonly bool isNarrow;

        private long count;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="linkClickCallback">Обратный вызов клика на ссылку.</param>
        /// <param name="isNarrow">Узкое представление.</param>
        public RenderTextElementFactory(ILinkClickCallback linkClickCallback, bool isNarrow)
        {
            this.linkClickCallback = linkClickCallback;
            this.isNarrow = isNarrow;
        }

        /// <summary>
        /// Получить количество выполнений.
        /// </summary>
        /// <returns>Результат.</returns>
        public long GetExecutionCount()
        {
            return count;
        }

        /// <summary>
        /// Создать элемент.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <returns>Элемент.</returns>
        public FrameworkElement Create(ITextRenderCommand command)
        {
            count++;
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
                TextWrapping = TextWrapping.NoWrap,
                TextTrimming = TextTrimming.None,
                FontSize = 14.5,
                TextLineBounds = TextLineBounds.Full,
                IsTextSelectionEnabled = false,
                TextAlignment = TextAlignment.Left
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

            Border strikeBorder = null;

            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Strikethrough))
            {
                needGrid = true;
                strikeBorder = new Border()
                {
                    Background = r.Foreground,
                    Height = 1.2,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Top,
                };
                g.Children.Add(strikeBorder);
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

            r.Text = "o";
            r.Measure(new Size(0, 0));
            var spaceWidth = r.ActualWidth;

            int endSpaces = 0;
            var s2 = text;
            while (s2.EndsWith(" "))
            {
                endSpaces++;
                s2 = s2.Substring(0, s2.Length - 1);
            }

            r.Text = s2;

            r.Measure(new Size(0, 0));
            r.Height = r.ActualHeight;
            r.Width = r.ActualWidth + endSpaces * spaceWidth;

            if (strikeBorder != null)
            {
                var oh = r.ActualHeight;
                const double koef = 0.6;
                strikeBorder.Margin = new Thickness(0, koef * oh, 0, 0);
            }

            if (needGrid)
            {
                g.Height = result.Height;
                g.Width = result.Width;
                g.Children.Add(result);
                result = g;
            }
            if (needBorder)
            {
                b.Height = result.Height;
                b.Width = result.Width;
                b.Child = result;
                result = b;
            }
            if (needOuterGrid)
            {
                g2.Height = result.Height + g2.Padding.Bottom + g2.Padding.Top;
                g2.Width = result.Width;
                g2.Children.Add(result);
                result = g2;
            }

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

        /// <summary>
        /// Получить ключ кэша.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <returns>Ключ кэша.</returns>
        public string GetCacheKey(ITextRenderCommand command)
        {
            return command.GetCacheKey(isNarrow);
        }

        /// <summary>
        /// Создать элемент.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <returns>Элемент.</returns>
        FrameworkElement ICanvasElementFactory.Create(ITextRenderCommand command)
        {
            return Create(command);
        }
    }
}