using DvachBrowser3.Links;
using DvachBrowser3.Posts;
using DvachBrowser3.TextRender;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Логика рендеринга для модели рендеринга V1
    /// </summary>
    public sealed class RenderProgramV1Logic : RenderProgramLogicBase<ITextRenderProgramConsumer, IRenderProgramElement, ITextRenderAttribute, ITextRender2RenderProgram>
    {
        /// <summary>
        /// Объект перевода строки.
        /// </summary>
        /// <returns>Элемент.</returns>
        protected override IRenderProgramElement LineBreak()
        {
            return new LineBreakRenderProgramElement();
        }

        /// <summary>
        /// Печатать текст.
        /// </summary>
        /// <param name="text">Текст.</param>
        /// <returns>Элемент.</returns>
        protected override IRenderProgramElement PrintText(string text)
        {
            return new TextRenderProgramElement(text);
        }

        /// <summary>
        /// Добавить атрибут.
        /// </summary>
        /// <param name="attr">Атрибут.</param>
        /// <returns>Элемент.</returns>
        protected override IRenderProgramElement AddAttribute(ITextRenderAttribute attr)
        {
            return new AttributeRenderProgramElement(attr, true);
        }

        /// <summary>
        /// Удалить атрибут.
        /// </summary>
        /// <param name="attr">Атрибут.</param>
        /// <returns>Элемент.</returns>
        protected override IRenderProgramElement RemoveAttribute(ITextRenderAttribute attr)
        {
            return new AttributeRenderProgramElement(attr, false);
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        /// <param name="link">Объкт ссылки.</param>
        /// <returns>Атрибут.</returns>
        protected override ITextRenderAttribute Link(BoardLinkBase link)
        {
            return new LinkTextRenderAttribute("[data]", link);
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        /// <param name="uri">URI ссылки.</param>
        /// <returns>Атрибут.</returns>
        protected override ITextRenderAttribute Link(string uri)
        {
            return new LinkTextRenderAttribute(uri);
        }

        /// <summary>
        /// Создать атрибут.
        /// </summary>
        /// <param name="attr">Атрибут в тексте.</param>
        /// <returns>Атрибут.</returns>
        protected override ITextRenderAttribute CreateAttribute(PostNodeBasicAttribute attr)
        {
            switch (attr)
            {
                case PostNodeBasicAttribute.Bold:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Bold);
                case PostNodeBasicAttribute.Italic:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Italic);
                case PostNodeBasicAttribute.Monospace:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Fixed);
                case PostNodeBasicAttribute.Overscore:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Overline);
                case PostNodeBasicAttribute.Quote:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Quote);
                case PostNodeBasicAttribute.Spoiler:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Spoiler);
                case PostNodeBasicAttribute.Strikeout:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Strikethrough);
                case PostNodeBasicAttribute.Sub:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Subscript);
                case PostNodeBasicAttribute.Sup:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Superscript);
                case PostNodeBasicAttribute.Underscore:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Undeline);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Загрузить элемент.
        /// </summary>
        /// <param name="consumer">Объект для загрузки.</param>
        /// <param name="element">Элемент.</param>
        protected override void PushElement(ITextRenderProgramConsumer consumer, IRenderProgramElement element)
        {
            consumer.PushProgramElement(element);
        }

        /// <summary>
        /// Создать объект загрузки программы.
        /// </summary>
        /// <returns>Объект загрузки.</returns>
        protected override ITextRenderProgramConsumer CreateConsumer()
        {
            return new TextRender2ProgramFormer(new TextRenderCommandFormer());
        }

        /// <summary>
        /// Завершить формирование программы.
        /// </summary>
        /// <param name="consumer">Объект загрузки.</param>
        protected override void Flush(ITextRenderProgramConsumer consumer)
        {
            consumer.Flush();
        }

        /// <summary>
        /// Получить результат.
        /// </summary>
        /// <param name="consumer">Объект загрузки.</param>
        /// <returns>Результат.</returns>
        protected override ITextRender2RenderProgram GetResult(ITextRenderProgramConsumer consumer)
        {
            return (consumer as ITextRender2ProgramFormer)?.GetProgram();
        }
    }
}