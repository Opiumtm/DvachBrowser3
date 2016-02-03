using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Text;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Средство размещения элементов c использованием Direct 2D.
    /// </summary>
    public sealed class Direct2DTextRender2MeasureMapper : ITextRender2MeasureMapper
    {
        /// <summary>
        /// Коэффициент для зачёркнутого текста, шрифт Segoe UI.
        /// </summary>
        public const double StrikethrougKoef = 0.6;

        /// <summary>
        /// Создать карту.
        /// </summary>
        /// <param name="program">Программа.</param>
        /// <param name="width">Ширина.</param>
        /// <param name="fontSize">Размер шрифта.</param>
        /// <param name="maxLines">Максимальное число линий.</param>
        /// <returns>Карта.</returns>
        public ITextRender2MeasureMap CreateMap(ITextRender2RenderProgram program, double width, double fontSize, int? maxLines)
        {
            if (program == null) throw new ArgumentNullException(nameof(program));
            var interProgram = CreateIntermediateProgram(program).ToArray();
            var allText = interProgram.Aggregate(new StringBuilder(), (sb, s) => sb.Append(s.RenderString)).ToString();
            using (var tf = new CanvasTextFormat())
            {
                tf.FontFamily = "Segoe UI";
                tf.FontSize = (float)fontSize;
                tf.WordWrapping = CanvasWordWrapping.Wrap;
                tf.Direction = CanvasTextDirection.LeftToRightThenTopToBottom;
                tf.Options = CanvasDrawTextOptions.Default;

                using (var tl = new CanvasTextLayout(CanvasDevice.GetSharedDevice(), allText, tf, (float)width, 10f))
                {
                    foreach (var item in interProgram)
                    {
                        ApplyAttributes(tl, item, fontSize);
                    }
                    var map = AnalyzeMap(tl, allText).ToArray();
                    var result = new MeasureMap(map, width, maxLines, StrikethrougKoef);
                    return result;
                }
            }
        }

        private IEnumerable<ITextRender2MeasureMapLine> AnalyzeMap(CanvasTextLayout tl, string allText)
        {
            var lines = tl.LineMetrics;
            int idx = 0;
            var formatChanges = ToRanges(tl.GetFormatChangeIndices(), allText.Length).ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                //var regions = tl.GetCharacterRegions(idx, line.CharacterCount);
                var regions = GetRegions(tl, idx, line, formatChanges).ToArray();

                var resMap = new List<TextRender2MeasureMapElement>();

                foreach (var region in regions)
                {
                    var attr = tl.GetCustomBrush(region.CharacterIndex) as ITextRenderAttributeState;
                    var subStr = allText.Substring(region.CharacterIndex, region.CharacterCount).Replace("\n", "");
                    if (subStr.Length > 0)
                    {
                        resMap.Add(new TextRender2MeasureMapElement()
                        {
                            Command = new TextRenderCommand(attr ?? new TextRenderAttributeState(), new TextRenderTextContent(subStr)),
                            Placement = new Point(region.LayoutBounds.X, region.LayoutBounds.Y),
                            Size = new Size(region.LayoutBounds.Width, region.LayoutBounds.Height)
                        });
                    }
                }

                yield return new MeasureMapLine(resMap.ToArray(), i, line.Height);
                idx += line.CharacterCount;
            }
        }

        private IEnumerable<CanvasTextLayoutRegion> GetRegions(CanvasTextLayout tl, int idx, CanvasLineMetrics line, Range[] formatChanges)
        {
            var lineRange = new Range(idx, line.CharacterCount);
            var inLineChanges = formatChanges.Where(i => lineRange.InterlappedWith(i)).ToArray();
            if (inLineChanges.Length == 0)
            {
                var regs = tl.GetCharacterRegions(idx, line.CharacterCount);
                foreach (var r in regs)
                {
                    yield return r;
                }
            }
            else
            {
                foreach (var p in inLineChanges)
                {
                    var b = Math.Max(p.Begin, lineRange.Begin);
                    var e = Math.Min(p.End, lineRange.End);
                    var c = e - b + 1;
                    if (c > 0)
                    {
                        var regs = tl.GetCharacterRegions(b, c);
                        foreach (var r in regs)
                        {
                            yield return r;
                        }
                    }
                }
            }
        }

        private IEnumerable<Range> ToRanges(int[] c, int totalLength)
        {
            if (c.Length == 0)
            {
                yield return new Range(0, totalLength);
                yield break;
            }
            for (int i = 0; i < c.Length-1; i++)
            {
                yield return new Range(c[i], c[i+1] - c[i]);
            }
        }

        private struct Range
        {
            public Range(int begin, int count)
            {
                Begin = begin;
                Count = count;
            }

            public int Begin;

            public int Count;

            public int End => Begin + Count - 1;

            public bool InterlappedWith(Range other)
            {
                return !(End < other.Begin || Begin > other.End);
            }
        }

        private void ApplyAttributes(CanvasTextLayout tl, IntermediateElement item, double fontSize)
        {
            var command = item.Command;
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Bold))
            {
                tl.SetFontWeight(item.Index, item.RenderString.Length, FontWeights.Bold);
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Italic))
            {
                tl.SetFontStyle(item.Index, item.RenderString.Length, FontStyle.Italic);
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Fixed))
            {
                tl.SetFontFamily(item.Index, item.RenderString.Length, "Courier New");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Subscript) || command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Superscript))
            {
                tl.SetFontSize(item.Index, item.RenderString.Length, (float)(fontSize * 2.0 / 3.0));
            }
            tl.SetCustomBrush(item.Index, item.RenderString.Length, item.Command.Attributes);
        }

        private IEnumerable<IntermediateElement> CreateIntermediateProgram(ITextRender2RenderProgram program)
        {
            var idx = 0;
            foreach (var p in program.GetCommands())
            {
                if (p.Content.Id == CommonTextRenderContent.LineBreak)
                {
                    yield return new IntermediateElement() {RenderString = "\n", Command = p, Index = idx};
                    idx++;
                }
                if (p.Content.Id == CommonTextRenderContent.Text)
                {
                    var t = p.Content as ITextRenderTextContent;
                    var s = t?.Text ?? "";
                    if (s.Length > 0)
                    {
                        yield return new IntermediateElement() { RenderString = t?.Text ?? "", Command = p, Index = idx };
                        idx += s.Length;
                    }
                }
            }
        }

        private struct IntermediateElement
        {
            public int Index;

            public string RenderString;

            public ITextRenderCommand Command;
        }

        private sealed class MeasureMapLine : ITextRender2MeasureMapLine
        {
            private readonly TextRender2MeasureMapElement[] elements;

            public MeasureMapLine(TextRender2MeasureMapElement[] elements, int lineNumber, double height)
            {
                if (elements == null) throw new ArgumentNullException(nameof(elements));
                this.elements = elements;
                LineNumber = lineNumber;
                Height = height;
            }

            /// <summary>
            /// Получить карту размещения.
            /// </summary>
            /// <returns>Карта размещения.</returns>
            public IReadOnlyList<TextRender2MeasureMapElement> GetMeasureMap()
            {
                return elements;
            }

            /// <summary>
            /// Номер строки (с нулевой).
            /// </summary>
            public int LineNumber { get; }

            /// <summary>
            /// Высота.
            /// </summary>
            public double Height { get; }
        }

        private sealed class MeasureMap : ITextRender2MeasureMap
        {
            public MeasureMap(ITextRender2MeasureMapLine[] lines, double width, int? maxLines, double strikethrougKoef)
            {
                if (lines == null) throw new ArgumentNullException(nameof(lines));
                this.lines = lines;
                MaxLines = maxLines;
                var h = 0.0;
                foreach (var l in lines)
                {
                    if (maxLines.HasValue)
                    {
                        if (l.LineNumber >= maxLines.Value)
                        {
                            ExceedLines = true;
                            break;
                        }
                    }
                    h += l.Height;
                }
                Bounds = new Size(width, h);
                StrikethrougKoef = strikethrougKoef;
            }

            private readonly ITextRender2MeasureMapLine[] lines;

            /// <summary>
            /// Получить карту размещения.
            /// </summary>
            /// <returns>Карта размещения.</returns>
            public IReadOnlyList<ITextRender2MeasureMapLine> GetMeasureMapLines()
            {
                return lines;
            }

            /// <summary>
            /// Максимальное число линий.
            /// </summary>
            public int? MaxLines { get; }

            /// <summary>
            /// Превышено число линий.
            /// </summary>
            public bool ExceedLines { get; }

            /// <summary>
            /// Размеры.
            /// </summary>
            public Size Bounds { get; }

            /// <summary>
            /// Коэффициент для рендеринга зачёркнутого текста.
            /// </summary>
            public double StrikethrougKoef { get; }
        }
    }
}