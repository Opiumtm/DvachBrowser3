using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления борды.
    /// </summary>
    public interface IBoardListBoardViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Этот элемент.
        /// </summary>
        IBoardListBoardViewModel This { get; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// Короткое имя.
        /// </summary>
        string ShortName { get; }

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Движок.
        /// </summary>
        string Engine { get; }

        /// <summary>
        /// Ресурс.
        /// </summary>
        string Resource { get; }

        /// <summary>
        /// Категория.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Цвет плитки.
        /// </summary>
        Color TileBackgroundColor { get; }

        /// <summary>
        /// Логотип.
        /// </summary>
        ImageSource ResourceLogo { get; }

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