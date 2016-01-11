using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Исполнение команды текстового рендеринга в Direct 2D.
    /// </summary>
    public class Direct2DCommandExecutor : ITextRenderCommandExecutor
    {
        private static readonly CycleCache<string, double> WidthCache = new CycleCache<string, double>(500, StringComparer.Ordinal);

        protected readonly IDirect2DElementFactory Factory;

        protected readonly CanvasDrawingSession Session;

        protected readonly IWordSplitter Splitter;

        protected readonly double RenderWidth;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="session">Сессия отрисовки.</param>
        /// <param name="factory">Фабрика.</param>
        /// <param name="splitter">Средство разбики слов.</param>
        /// <param name="renderWidth">Ширина отрисовки.</param>
        public Direct2DCommandExecutor(CanvasDrawingSession session, IDirect2DElementFactory factory, IWordSplitter splitter, double renderWidth)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (splitter == null) throw new ArgumentNullException(nameof(splitter));
            Factory = factory;
            Session = session;
            Splitter = splitter;
            RenderWidth = renderWidth;
        }

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <returns>Остаток для последующего вызова.</returns>
        public ITextRenderCommand ExecuteCommand(ITextRenderCommand command)
        {
            if (command == null || command.Content == null || command.Attributes == null || command.Content.Id == null)
            {
                return null;
            }
            if (!CommonTextRenderContent.Text.Equals(command.Content.Id, StringComparison.OrdinalIgnoreCase))
            {
                ExecuteNonText(command);
                return null;
            }
            var txt = command.Content as ITextRenderTextContent;
            if (txt != null && txt.Text != null)
            {
                var remText = ExectuteText(command.Attributes, txt.Text);
                if (string.IsNullOrEmpty(remText))
                {
                    return null;
                }
                return new TextRenderCommand(command.Attributes, new TextRenderTextContent(remText));
            }
            return null;
        }

        /// <summary>
        /// Выполнить текстовую команду.
        /// </summary>
        /// <param name="attributes">Атрибуты.</param>
        /// <param name="text">Текст.</param>
        /// <returns>Остаток текста.</returns>
        protected virtual string ExectuteText(ITextRenderAttributeState attributes, string text)
        {
            var words = Splitter.Split(text);
            var run = CheckLengths(attributes, words);
            var isAdded = false;
            Action old = null;
            var sb = new StringBuilder();
            foreach (var r in run)
            {
                if (!r.Item2)
                {
                    bool isThis = false;
                    if (!isAdded)
                    {
                        isAdded = true;
                        var ta = old;
                        if (ta != null)
                        {
                            ta();
                        }
                        else
                        {
                            if (FirstInLine)
                            {
                                r.Item3?.Invoke();
                                isThis = true;
                            }
                            else
                            {
                                NewLine(attributes);
                            }
                        }
                    }
                    if (!isThis)
                    {
                        sb.Append(r.Item1);
                    }
                }
                old = r.Item3;
            }
            if (!isAdded)
            {
                var ta = old;
                ta?.Invoke();
            }

            return sb.ToString();
        }

        protected IEnumerable<Tuple<string, bool, Action, bool>> CheckLengths(ITextRenderAttributeState attributes, IEnumerable<string> words)
        {
            var wordsArr = words.ToArray();
            var testCom = new TextRenderCommand(attributes, new TextRenderTextContent("a"));
            var charWidth = GetElementWidth(testCom);

            int idx0 = 0;
            string current = "";
            for (int i = 0; i < wordsArr.Length; i++)
            {
                var word = wordsArr[i];
                current += word;
                var len = current.Length * charWidth;
                if ((len + LineWidth) > RenderWidth)
                {
                    idx0 = i;
                    break;
                }
            }

            int idx1 = -1;

            for (int j = idx0; j >= 0; j--)
            {
                current = "";
                for (int i = 0; i <= j; i++)
                {
                    var word = wordsArr[i];
                    current += word;
                }
                var com = new TextRenderCommand(attributes, new TextRenderTextContent(current));
                var elWidth = GetElementWidth(com);
                if (!((elWidth + LineWidth) > RenderWidth))
                {
                    idx1 = j;
                    break;
                }
            }

            bool exceeded = false;
            current = "";
            for (int i = 0; i < wordsArr.Length; i++)
            {
                var word = wordsArr[i];
                current += word;
                if (exceeded)
                {
                    yield return new Tuple<string, bool, Action, bool>(word, false, null, true);
                }
                else
                {
                    var com = new TextRenderCommand(attributes, new TextRenderTextContent(current));
                    if (i > idx1)
                    {
                        var elWidth = GetElementWidth(com);
                        if ((elWidth + LineWidth) > RenderWidth)
                        {
                            exceeded = true;
                        }
                        yield return new Tuple<string, bool, Action, bool>(word, !exceeded, () => AddElementToCanvas(com), false);
                    }
                    else
                    {
                        yield return new Tuple<string, bool, Action, bool>(word, true, () => AddElementToCanvas(com), false);
                    }
                }
            }
        }


        private double GetElementWidth(ITextRenderCommand command)
        {
            return WidthCache.GetValue(Factory.GetCacheKey(command), () => Factory.MeasureCommand(command).Width);
        }

        private void AddElementToCanvas(ITextRenderCommand command)
        {
            if (command == null)
            {
                return;
            }
            Size size;
            if (IsMeasureMode)
            {
                size = Factory.MeasureCommand(command);
            }
            else
            {
                size = Factory.DrawCommand(new Point() { X = LineWidth, Y = Top }, Session, command);
            }
            LineWidth += size.Width;
            if (LineHeight < size.Height)
            {
                LineHeight = size.Height;
                SynCanvasHeight();
            }
            FirstInLine = false;
        }

        /// <summary>
        /// Выполнить не текстовую команду.
        /// </summary>
        /// <param name="command">Команда.</param>
        protected virtual void ExecuteNonText(ITextRenderCommand command)
        {
            if (CommonTextRenderContent.LineBreak.Equals(command.Content.Id))
            {
                NewLine(command.Attributes);
            }
        }

        /// <summary>
        /// Начать новую строку.
        /// </summary>
        /// <param name="attributes">Атрибуты.</param>
        protected virtual void NewLine(ITextRenderAttributeState attributes)
        {
            Lines++;
            if (LineHeight < 0.01)
            {
                SetDefaultLineHeight(attributes);
            }
            Top = Top + LineHeight;
            LineHeight = 0;
            LineWidth = 0;
            FirstInLine = true;
            SynCanvasHeight();
        }

        /// <summary>
        /// Установить высоту линии по умолчанию.
        /// </summary>
        /// <param name="attributes">Атрибуты.</param>
        protected virtual void SetDefaultLineHeight(ITextRenderAttributeState attributes)
        {
            var size = Factory.MeasureCommand(new TextRenderCommand(attributes, new TextRenderTextContent("A")));
            if (LineHeight < size.Height)
            {
                LineHeight = size.Height;
            }
        }

        /// <summary>
        /// Всего линий отрисовано.
        /// </summary>
        public int Lines { get; protected set; }

        /// <summary>
        /// Высота линии.
        /// </summary>
        protected double LineHeight { get; set; }

        /// <summary>
        /// Ширина линии.
        /// </summary>
        protected double LineWidth { get; set; }

        /// <summary>
        /// Первый в линии.
        /// </summary>
        protected bool FirstInLine { get; set; }

        /// <summary>
        /// Верхняя позиция.
        /// </summary>
        protected double Top { get; set; }

        /// <summary>
        /// Очистить.
        /// </summary>
        public void Clear()
        {
            Top = 0;
            Lines = 0;
            LineWidth = 0;
            LineHeight = 0;
            RenderHeight = 0;
            FirstInLine = true;
        }

        /// <summary>
        /// Высота.
        /// </summary>
        public double RenderHeight { get; protected set; }

        /// <summary>
        /// Синхронизировать высоту канвы.
        /// </summary>
        protected void SynCanvasHeight()
        {
            var newHeight = Top + LineHeight;
            RenderHeight = newHeight;
        }

        /// <summary>
        /// Режим расчёта размеров (без рисования).
        /// </summary>
        public bool IsMeasureMode { get; set; }
    }
}