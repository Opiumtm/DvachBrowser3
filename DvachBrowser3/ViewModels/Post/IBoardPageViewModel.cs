using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DvachBrowser3.Links;
using DvachBrowser3.Styles;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления страницы борды.
    /// </summary>
    public interface IBoardPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Ссылка на борду.
        /// </summary>
        BoardLinkBase BoardLink { get; }

        /// <summary>
        /// Ссылка на страницу.
        /// </summary>
        BoardLinkBase PageLink { get; }

        /// <summary>
        /// Ссылка на следующую страницу.
        /// </summary>
        BoardLinkBase NextPageLink { get; }

        /// <summary>
        /// Ссылка на предыдущую страницу.
        /// </summary>
        BoardLinkBase PrevPageLink { get; }

        /// <summary>
        /// Получить ссылку на страницу.
        /// </summary>
        /// <param name="page">Страница.</param>
        /// <returns>Ссылка на страницу.</returns>
        BoardLinkBase GetPageLink(int page);

        /// <summary>
        /// Получить список страниц.
        /// </summary>
        /// <returns>Список страниц.</returns>
        int[] GetPages();

        /// <summary>
        /// Треды.
        /// </summary>
        IList<IThreadPreviewViewModel> Threads { get; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Номер страницы.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Можно перейти к следующей странице.
        /// </summary>
        bool CanGoNextPage { get; }

        /// <summary>
        /// Можно перейти к предыдущей странице.
        /// </summary>
        bool CanGoPrevPage { get; }

        /// <summary>
        /// Скорость постинга на борде.
        /// </summary>
        string BoardSpeed { get; }

        /// <summary>
        /// Баннер.
        /// </summary>
        IPageBannerViewModel Banner { get; }

        /// <summary>
        /// Клик на ссылку.
        /// </summary>
        event LinkClickEventHandler LinkClick;

        /// <summary>
        /// Провести асинхронную инициализацию.
        /// </summary>
        /// <returns>Задача.</returns>
        Task Initialize();

        /// <summary>
        /// Страница устарела.
        /// </summary>
        bool IsObsolete { get; }

        /// <summary>
        /// Пометить как устаревшую.
        /// </summary>
        void TriggerObsolete();

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        IStyleManager StyleManager { get; }
    }
}