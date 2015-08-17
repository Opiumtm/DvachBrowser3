using System;
using System.Collections.Generic;
using System.Reflection;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Состояние атрибутов.
    /// </summary>
    public sealed class TextRenderAttributeState : ITextRenderAttributeState
    {
        private readonly Dictionary<string, ITextRenderAttribute> current = new Dictionary<string, ITextRenderAttribute>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, Stack<ITextRenderAttribute>> all = new Dictionary<string, Stack<ITextRenderAttribute>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Атрибуты.
        /// </summary>
        public IDictionary<string, ITextRenderAttribute> Attributes
        {
            get { return current; }
        }

        /// <summary>
        /// Добавить атрибут.
        /// </summary>
        /// <param name="attribute">Атрибут.</param>
        public void AddAttribute(ITextRenderAttribute attribute)
        {
            if (attribute == null || attribute.Id == null)
            {
                return;
            }
            if (!all.ContainsKey(attribute.Id))
            {
                all[attribute.Id] = new Stack<ITextRenderAttribute>();
            }
            all[attribute.Id].Push(attribute);
            current[attribute.Id] = attribute;
        }

        /// <summary>
        /// Удалить атрибут.
        /// </summary>
        /// <param name="attribute">Атрибут.</param>
        public void RemoveAttribute(ITextRenderAttribute attribute)
        {
            if (attribute == null || attribute.Id == null)
            {
                return;
            }
            ITextRenderAttribute top = null;
            if (all.ContainsKey(attribute.Id))
            {
                var stack = all[attribute.Id];
                if (stack.Count > 0)
                {
                    stack.Pop();
                }
                if (stack.Count > 0)
                {
                    top = stack.Peek();
                }
                else
                {
                    all.Remove(attribute.Id);
                }
            }
            if (top != null)
            {
                current[attribute.Id] = top;
            }
            else
            {
                current.Remove(attribute.Id);
            }
        }

        /// <summary>
        /// Очистить.
        /// </summary>
        public void Clear()
        {
            current.Clear();
            all.Clear();
        }
    }
}