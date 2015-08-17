namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Элемент программы с атрибутом.
    /// </summary>
    public interface IAttributeRenderProgramElement : IRenderProgramElement
    {
        /// <summary>
        /// Атрибут.
        /// </summary>
        ITextRenderAttribute Attribute { get; } 
    }
}