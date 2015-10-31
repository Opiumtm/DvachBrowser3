using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления борды.
    /// </summary>
    public interface IBoardListBoardViewModel
    {
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
        /// Фильтровать.
        /// </summary>
        /// <param name="filterString">Строка фильтра.</param>
        /// <returns>true, если нужно отобразить.</returns>
        bool Filter(string filterString);
    }
}