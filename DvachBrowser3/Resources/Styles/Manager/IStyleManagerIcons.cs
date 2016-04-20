using System.ComponentModel;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Иконки.
    /// </summary>
    public interface IStyleManagerIcons : INotifyPropertyChanged
    {
        /// <summary>
        /// Размер флага/иконки.
        /// </summary>
        double PoIconSize { get; }

        /// <summary>
        /// Размер иконки хидера.
        /// </summary>
        double HeaderIconSize { get; }

        /// <summary>
        /// Ширина флага.
        /// </summary>
        double FlagIconWidth { get; }

        /// <summary>
        /// Ширина флага.
        /// </summary>
        double FlagIconHeight { get; }
    }
}