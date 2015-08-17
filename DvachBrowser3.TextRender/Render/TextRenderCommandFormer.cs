using System;
using System.Text;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Формирователь команд.
    /// </summary>
    public class TextRenderCommandFormer : ITextRenderCommandFormer
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextRenderCommandFormer()
        {
            Text = new StringBuilder();
            Attributes = new TextRenderAttributeState();
        }

        /// <summary>
        /// Аттрибуты.
        /// </summary>
        protected ITextRenderAttributeState Attributes { get; private set; }

        /// <summary>
        /// Текст.
        /// </summary>
        protected StringBuilder Text { get; private set; }

        /// <summary>
        /// Следующая строка.
        /// </summary>
        protected bool LineBreak { get; set; }

        /// <summary>
        /// Добавить элемент.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <returns>true, если добавлено успешно. false, если нужно вызвать GetCommand</returns>
        public virtual bool AddElement(IRenderProgramElement element)
        {
            if (element == null || element.Id == null)
            {
                return true;
            }
            if (HasNonTextContent)
            {
                return false;
            }
            if (CommonRenderProgramElements.PrintText.Equals(element.Id, StringComparison.OrdinalIgnoreCase) &&
                element is ITextRenderProgramElement)
            {
                var txt = (ITextRenderProgramElement) element;
                Text.Append(txt.Text ?? "");
                return true;
            }
            if (Text.Length > 0)
            {
                return false;
            }
            ExecuteElement(element);
            return true;
        }

        /// <summary>
        /// Есть не текстовый контент.
        /// </summary>
        protected virtual bool HasNonTextContent
        {
            get { return LineBreak; }
        }

        /// <summary>
        /// Выполнить элемент программы.
        /// </summary>
        /// <param name="element">Элемент.</param>
        protected virtual void ExecuteElement(IRenderProgramElement element)
        {
            if (CommonRenderProgramElements.LineBreak.Equals(element.Id, StringComparison.CurrentCultureIgnoreCase))
            {
                LineBreak = true;
                return;
            }
            if (CommonRenderProgramElements.AddAttribute.Equals(element.Id, StringComparison.CurrentCultureIgnoreCase))
            {
                var el = element as IAttributeRenderProgramElement;
                if (el != null && el.Attribute != null)
                {
                    Attributes.AddAttribute(el.Attribute);
                }
                return;
            }
            if (CommonRenderProgramElements.RemoveAttribute.Equals(element.Id, StringComparison.CurrentCultureIgnoreCase))
            {
                var el = element as IAttributeRenderProgramElement;
                if (el != null && el.Attribute != null)
                {
                    Attributes.RemoveAttribute(el.Attribute);
                }
                return;
            }
        }

        /// <summary>
        /// Получить команду.
        /// </summary>
        /// <returns>Команда (null - нет команды).</returns>
        public virtual ITextRenderCommand GetCommand()
        {
            if (LineBreak)
            {
                return new TextRenderCommand(Attributes, new TextRenderLineBreakContent());
            }
            if (Text.Length > 0)
            {
                return new TextRenderCommand(Attributes, new TextRenderTextContent(Text.ToString()));
            }
            return null;
        }

        /// <summary>
        /// Очистить.
        /// </summary>
        public virtual void Clear()
        {
            Attributes.Clear();
            Flush();
        }

        /// <summary>
        /// Очистка текстового буфера.
        /// </summary>
        public virtual void Flush()
        {
            Text.Clear();
            LineBreak = false;
        }
    }
}