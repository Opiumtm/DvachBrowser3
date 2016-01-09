using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Исполнение команды текстового рендеринга.
    /// </summary>
    public class CanvasTextRenderCommandExecutor : ITextRenderCommandExecutor
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="textCanvas">Канва.</param>
        /// <param name="factory">Фабрика элементов.</param>
        /// <param name="splitter">Средство разбивки на слова.</param>
        public CanvasTextRenderCommandExecutor(Canvas textCanvas, ICanvasElementFactory factory, IWordSplitter splitter)
        {
            if (textCanvas == null) throw new ArgumentNullException("textCanvas");
            if (factory == null) throw new ArgumentNullException("factory");
            if (splitter == null) throw new ArgumentNullException("splitter");
            TextCanvas = textCanvas;
            Factory = factory;
            Splitter = splitter;
            FirstInLine = true;
        }

        /// <summary>
        /// Канва.
        /// </summary>
        public Canvas TextCanvas { get; private set; }

        /// <summary>
        /// Фабрика элементов.
        /// </summary>
        protected ICanvasElementFactory Factory { get; private set; }

        /// <summary>
        /// Средство разбивки на слова.
        /// </summary>
        protected IWordSplitter Splitter { get; private set; }

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
            Func<FrameworkElement> old = null;
            var sb = new StringBuilder();
            foreach (var r in run)
            {
                if (!r.Item2)
                {
                    bool isThis = false;
                    if (!isAdded)
                    {
                        isAdded = true;
                        var ta = old?.Invoke();
                        if (ta != null)
                        {
                            AddElementToCanvas(ta);
                        }
                        else
                        {
                            if (FirstInLine)
                            {
                                AddElementToCanvas(r.Item3());
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
                var ta = old?.Invoke();
                if (ta != null)
                {
                    AddElementToCanvas(ta);
                }
            }

            return sb.ToString();
        }

        protected IEnumerable<Tuple<string, bool, Func<FrameworkElement>, bool>> CheckLengths(ITextRenderAttributeState attributes, IEnumerable<string> words)
        {
            var wordsArr = words.ToArray();
            var testEl = Factory.Create(new TextRenderCommand(attributes, new TextRenderTextContent("a")));
            var charWidth = testEl.Width;

            int idx0 = 0;
            string current = "";
            for (int i = 0; i < wordsArr.Length; i++)
            {
                var word = wordsArr[i];
                current += word;
                var len = current.Length*charWidth;
                if ((len + LineWidth) > TextCanvas.ActualWidth)
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
                var el = Factory.Create(new TextRenderCommand(attributes, new TextRenderTextContent(current)));
                if (!((el.Width + LineWidth) > TextCanvas.ActualWidth))
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
                    yield return new Tuple<string, bool, Func<FrameworkElement>, bool>(word, false, () => null, true);
                }
                else
                {
                    if (i > idx1)
                    {
                        var el = Factory.Create(new TextRenderCommand(attributes, new TextRenderTextContent(current)));
                        if ((el.Width + LineWidth) > TextCanvas.ActualWidth)
                        {
                            exceeded = true;
                        }
                        yield return new Tuple<string, bool, Func<FrameworkElement>, bool>(word, !exceeded, () => el, false);
                    }
                    else
                    {
                        var com = new TextRenderCommand(attributes, new TextRenderTextContent(current));
                        yield return new Tuple<string, bool, Func<FrameworkElement>, bool>(word, true, () => Factory.Create(com), false);
                    }
                }
            }
        }

        private void AddElementToCanvas(FrameworkElement el)
        {
            if (el == null)
            {
                return;
            }
            el.SetValue(Canvas.LeftProperty, LineWidth);
            el.SetValue(Canvas.TopProperty, Top);
            TextCanvas.Children.Add(el);
            LineWidth += el.Width;
            if (LineHeight < el.Height)
            {
                LineHeight = el.Height;
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
            var el = Factory.Create(new TextRenderCommand(attributes, new TextRenderTextContent("A")));
            if (LineHeight < el.Height)
            {
                LineHeight = el.Height;
            }
        }

        /// <summary>
        /// Количество линий.
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
        public virtual void Clear()
        {
            TextCanvas.Children.Clear();
            Top = 0;
            TextCanvas.Height = 0;
            Lines = 0;
            LineWidth = 0;
            LineHeight = 0;
            FirstInLine = true;
        }

        /// <summary>
        /// Синхронизировать высоту канвы.
        /// </summary>
        protected void SynCanvasHeight()
        {
            var newHeight = Top + LineHeight;
            if (TextCanvas.Height != newHeight)
            {
                TextCanvas.Height = newHeight;
            }
        }
    }
}