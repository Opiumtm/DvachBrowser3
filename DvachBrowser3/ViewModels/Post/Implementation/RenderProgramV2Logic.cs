using DvachBrowser3.Links;
using DvachBrowser3.Posts;
using Ipatov.MarkupRender;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Логика рендеринга для модели рендеринга V2
    /// </summary>
    public sealed class RenderProgramV2Logic : RenderProgramLogicBase<RenderProgramCommandsFormer, IRenderProgramElement, ITextAttribute, IRenderCommandsSource>
    {
        /// <summary>
        /// Объект перевода строки.
        /// </summary>
        /// <returns>Элемент.</returns>
        protected override IRenderProgramElement LineBreak()
        {
            return CommonProgramElements.LinkeBreakElement;
        }

        /// <summary>
        /// Печатать текст.
        /// </summary>
        /// <param name="text">Текст.</param>
        /// <returns>Элемент.</returns>
        protected override IRenderProgramElement PrintText(string text)
        {
            return CommonProgramElements.PrintTextElement(text);
        }

        /// <summary>
        /// Добавить атрибут.
        /// </summary>
        /// <param name="attr">Атрибут.</param>
        /// <returns>Элемент.</returns>
        protected override IRenderProgramElement AddAttribute(ITextAttribute attr)
        {
            return CommonProgramElements.AddAttributeElement(attr);
        }

        /// <summary>
        /// Удалить атрибут.
        /// </summary>
        /// <param name="attr">Атрибут.</param>
        /// <returns>Элемент.</returns>
        protected override IRenderProgramElement RemoveAttribute(ITextAttribute attr)
        {
            return CommonProgramElements.RemoveAttributeElement(attr);
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        /// <param name="link">Объкт ссылки.</param>
        /// <returns>Атрибут.</returns>
        protected override ITextAttribute Link(BoardLinkBase link)
        {
            return CommonTextAttributes.CreateLink(link);
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        /// <param name="uri">URI ссылки.</param>
        /// <returns>Атрибут.</returns>
        protected override ITextAttribute Link(string uri)
        {
            return CommonTextAttributes.CreateLink(uri);
        }

        /// <summary>
        /// Создать атрибут.
        /// </summary>
        /// <param name="attr">Атрибут в тексте.</param>
        /// <returns>Атрибут.</returns>
        protected override ITextAttribute CreateAttribute(PostNodeBasicAttribute attr)
        {
            switch (attr)
            {
                case PostNodeBasicAttribute.Bold:
                    return CommonTextAttributes.BoldAttribute;
                case PostNodeBasicAttribute.Italic:
                    return CommonTextAttributes.ItalicAttribute;
                case PostNodeBasicAttribute.Monospace:
                    return CommonTextAttributes.FixedAttribute;
                case PostNodeBasicAttribute.Overscore:
                    return CommonTextAttributes.OverlineAttribute;
                case PostNodeBasicAttribute.Quote:
                    return CommonTextAttributes.QuoteAttribute;
                case PostNodeBasicAttribute.Spoiler:
                    return CommonTextAttributes.SpoilerAttribute;
                case PostNodeBasicAttribute.Strikeout:
                    return CommonTextAttributes.StrikethroughAttribute;
                case PostNodeBasicAttribute.Sub:
                    return CommonTextAttributes.SubscriptAttribute;
                case PostNodeBasicAttribute.Sup:
                    return CommonTextAttributes.SubscriptAttribute;
                case PostNodeBasicAttribute.Underscore:
                    return CommonTextAttributes.UnderlineAttribute;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Загрузить элемент.
        /// </summary>
        /// <param name="consumer">Объект для загрузки.</param>
        /// <param name="element">Элемент.</param>
        protected override void PushElement(RenderProgramCommandsFormer consumer, IRenderProgramElement element)
        {
            consumer.Push(element);
        }

        /// <summary>
        /// Создать объект загрузки программы.
        /// </summary>
        /// <returns>Объект загрузки.</returns>
        protected override RenderProgramCommandsFormer CreateConsumer()
        {
            return new RenderProgramCommandsFormer();
        }

        /// <summary>
        /// Завершить формирование программы.
        /// </summary>
        /// <param name="consumer">Объект загрузки.</param>
        protected override void Flush(RenderProgramCommandsFormer consumer)
        {
            consumer.Flush();
        }

        /// <summary>
        /// Получить результат.
        /// </summary>
        /// <param name="consumer">Объект загрузки.</param>
        /// <returns>Результат.</returns>
        protected override IRenderCommandsSource GetResult(RenderProgramCommandsFormer consumer)
        {
            return consumer;
        }
    }
}