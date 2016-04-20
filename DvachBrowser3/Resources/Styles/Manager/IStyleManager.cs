using System.ComponentModel;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Менеджер стилей.
    /// </summary>
    public interface IStyleManager : INotifyPropertyChanged
    {
        /// <summary>
        /// Текст.
        /// </summary>
        IStyleManagerText Text { get; }

        /// <summary>
        /// Иконки.
        /// </summary>
        IStyleManagerIcons Icons { get; }

        /// <summary>
        /// Тайлы.
        /// </summary>
        IStyleManagerTiles Tiles { get; }
    }
}