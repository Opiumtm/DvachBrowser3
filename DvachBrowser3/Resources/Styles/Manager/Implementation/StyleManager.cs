using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Менеджер стилей.
    /// </summary>
    public sealed class StyleManager : StyleManagerObjectBase, IStyleManager
    {
        /// <summary>
        /// Назначить значения.
        /// </summary>
        protected override void SetValues()
        {
        }

        /// <summary>
        /// Текст.
        /// </summary>
        public IStyleManagerText Text { get; } = new StyleManagerText();

        /// <summary>
        /// Иконки.
        /// </summary>
        public IStyleManagerIcons Icons { get; } = new StyleManagerIcons();

        /// <summary>
        /// Тайлы.
        /// </summary>
        public IStyleManagerTiles Tiles { get; } = new StyleManagerTiles();
    }
}