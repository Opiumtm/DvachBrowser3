using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления каталога.
    /// </summary>
    public interface IBoardCatalogViewModel : IPostCollectionViewModel, IStartableViewModelWithResume
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// Заголовок борды.
        /// </summary>
        BoardLinkBase BoardLink { get; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Была навигация назад.
        /// </summary>
        bool IsBackNavigatedToViewModel { get; set; }
    }
}