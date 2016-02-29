using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DvachBrowser3_TextRender_Native;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Средство рендеринга разметки.
    /// </summary>
    public sealed class XamlCanvasTextRender2Renderer : ITextRender2Renderer
    {
        private static readonly MassChildUpdateHelper UpdateHelper = new MassChildUpdateHelper();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="callback">Обратный вызов.</param>
        public XamlCanvasTextRender2Renderer(ITextRender2RenderCallback callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            Callback = new ColorsCacheCallback(callback);
        }

        /// <summary>
        /// Обратный вызов.
        /// </summary>
        public ITextRender2RenderCallback Callback { get; }

        /// <summary>
        /// Отрисовать.
        /// </summary>
        /// <param name="map">Карта расположений.</param>
        /// <returns>Результат.</returns>
        public FrameworkElement Render(ITextRender2MeasureMap map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            var canvas = new Canvas()
            {
                Height = map.Bounds.Height,
                Width = map.Bounds.Width
            };
            DoRender(canvas, map);
            return canvas;
        }

        private void DoRender(Canvas canvas, ITextRender2MeasureMap map)
        {
            var elements = DoRenderLines(map).ToArray();
            UpdateHelper.UpdateChildren(canvas.Children, elements);
        }

        private IEnumerable<UIElement> DoRenderLines(ITextRender2MeasureMap map)
        {
            foreach (var line in map.GetMeasureMapLines())
            {
                if (map.MaxLines.HasValue)
                {
                    if (line.LineNumber >= map.MaxLines.Value)
                    {
                        break;
                    }
                }
                foreach (var el in DoRenderLine(map, line))
                {
                    yield return el;
                }
            }
        }

        private IEnumerable<UIElement> DoRenderLine(ITextRender2MeasureMap map, ITextRender2MeasureMapLine line)
        {
            foreach (var el in line.GetMeasureMap())
            {
                yield return DoRenderElement(map, line, el);
            }
        }

        private static readonly FontFamily SegoeUi = new FontFamily("Segoe UI");

        private UIElement DoRenderElement(ITextRender2MeasureMap map, ITextRender2MeasureMapLine line, TextRender2MeasureMapElement el)
        {
            var command = el.Command;

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
                FontFamily = SegoeUi,
                Foreground = Callback.PostNormalTextBrush,
                TextWrapping = TextWrapping.NoWrap,
                TextTrimming = TextTrimming.None,
                FontSize = Callback.PostFontSize,
                IsTextSelectionEnabled = false,
                TextAlignment = TextAlignment.Left,
                Text = text
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
                b.Background = Callback.PostSpoilerBackgroundBrush;
                r.Foreground = Callback.PostSpoilerTextBrush;
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Quote))
            {
                r.Foreground = Callback.PostQuoteTextBrush;
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Link))
            {
                r.Foreground = Callback.PostLinkTextBrush;
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
                var fh = line.Height;
                r.FontSize = r.FontSize*2.0/3.0;
                var fh2 = line.Height*2.0/3.0;
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
                    g2.Padding = new Thickness(0, delta/2, 0, delta/2);
                }
            }

            if (strikeBorder != null)
            {
                var oh = el.Size.Height;
                strikeBorder.Margin = new Thickness(0, map.StrikethrougKoef*oh, 0, 0);
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

            Canvas.SetLeft(result, el.Placement.X);
            Canvas.SetTop(result, el.Placement.Y);

            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Link))
            {
                var linkAttribute = command.Attributes.Attributes[CommonTextRenderAttributes.Link] as ITextRenderLinkAttribute;
                if (linkAttribute != null)
                {
                    Callback.RenderLinkCallback(result, linkAttribute);
                }
            }

            return result;
        }
    }
}