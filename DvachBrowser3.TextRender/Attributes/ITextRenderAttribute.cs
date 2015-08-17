using System;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Атрибут рендеринга текста.
    /// </summary>
    public interface ITextRenderAttribute
    {
        /// <summary>
        /// Идентификатор атрибута.
        /// </summary>
        string Id { get; } 
    }
}