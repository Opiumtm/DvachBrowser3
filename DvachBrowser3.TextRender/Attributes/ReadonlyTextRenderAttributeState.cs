using System;
using System.Collections.Generic;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Состояние атрибутов только для чтения.
    /// </summary>
    public sealed class ReadonlyTextRenderAttributeState : ITextRenderAttributeState
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="original">Оригинальное состояние.</param>
        public ReadonlyTextRenderAttributeState(ITextRenderAttributeState original)
        {
            var attr = original?.Attributes;
            if (attr != null)
            {
                foreach (var kv in attr)
                {
                    Attributes[kv.Key] = kv.Value;
                }
            }
        }

        /// <summary>
        /// Атрибуты.
        /// </summary>
        public IDictionary<string, ITextRenderAttribute> Attributes { get; } = new Dictionary<string, ITextRenderAttribute>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Добавить атрибут.
        /// </summary>
        /// <param name="attribute">Атрибут.</param>
        public void AddAttribute(ITextRenderAttribute attribute)
        {
            throw new System.NotImplementedException("Нельзя изменять копию только для чтения состояния атрибутов");
        }

        /// <summary>
        /// Удалить атрибут.
        /// </summary>
        /// <param name="attribute">Атрибут.</param>
        public void RemoveAttribute(ITextRenderAttribute attribute)
        {
            throw new System.NotImplementedException("Нельзя изменять копию только для чтения состояния атрибутов");
        }

        /// <summary>
        /// Очистить.
        /// </summary>
        public void Clear()
        {
            throw new System.NotImplementedException("Нельзя изменять копию только для чтения состояния атрибутов");
        }

        /// <summary>
        /// Получить read-only копию.
        /// </summary>
        /// <returns>Клон состояния.</returns>
        public ITextRenderAttributeState GetReadonlyCopy()
        {
            return new ReadonlyTextRenderAttributeState(this);
        }
    }
}