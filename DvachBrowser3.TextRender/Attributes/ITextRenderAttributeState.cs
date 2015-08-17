using System.Collections.Generic;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Состояние атрибутов.
    /// </summary>
    public interface ITextRenderAttributeState
    {
        /// <summary>
        /// Атрибуты.
        /// </summary>
        IDictionary<string, ITextRenderAttribute> Attributes { get; }

        /// <summary>
        /// Добавить атрибут.
        /// </summary>
        /// <param name="attribute">Атрибут.</param>
        void AddAttribute(ITextRenderAttribute attribute);

        /// <summary>
        /// Удалить атрибут.
        /// </summary>
        /// <param name="attribute">Атрибут.</param>
        void RemoveAttribute(ITextRenderAttribute attribute);

        /// <summary>
        /// Очистить.
        /// </summary>
        void Clear();
    }
}