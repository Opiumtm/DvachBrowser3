using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Links;
using DvachBrowser3.Styles;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления борды.
    /// </summary>
    public interface IBoardListBoardViewModel : ICommonTileViewModel, INotifyPropertyChanged
    {
        /// <summary>
        /// Этот элемент.
        /// </summary>
        IBoardListBoardViewModel This { get; }

        /// <summary>
        /// Категория.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Цвет плитки.
        /// </summary>
        Color TileBackgroundColor { get; }

        /// <summary>
        /// Фильтровать.
        /// </summary>
        /// <param name="filterString">Строка фильтра.</param>
        /// <returns>true, если нужно отобразить.</returns>
        bool Filter(string filterString);

        /// <summary>
        /// Избранное.
        /// </summary>
        bool IsFavorite { get; }

        /// <summary>
        /// Для взрослых.
        /// </summary>
        bool IsAdult { get; }
    }
}