namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Элемент программы с атрибутом.
    /// </summary>
    public sealed class AttributeRenderProgramElement : IAttributeRenderProgramElement
    {
        private readonly bool add;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="attribute">Атрибут.</param>
        /// <param name="add">Добавить атрибут.</param>
        public AttributeRenderProgramElement(ITextRenderAttribute attribute, bool add)
        {
            this.add = add;
            Attribute = attribute;
        }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id
        {
            get { return add ? CommonRenderProgramElements.AddAttribute : CommonRenderProgramElements.RemoveAttribute; }
        }

        public ITextRenderAttribute Attribute { get; private set; }
    }
}