using System;
using System.ComponentModel;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Загрузчик страницы борды.
    /// </summary>
    public interface IBoardPageLoaderViewModel : INotifyPropertyChanged, IStartableViewModelWithResume
    {
        /// <summary>
        /// Ссылка на страницу.
        /// </summary>
        BoardLinkBase PageLink { get; }

        /// <summary>
        /// Страница.
        /// </summary>
        IBoardPageViewModel Page { get; }

        /// <summary>
        /// Операция обновления.
        /// </summary>
        IOperationViewModel Update { get; }

        /// <summary>
        /// Загрузить.
        /// </summary>
        void Reload();

        /// <summary>
        /// Проверить на обновления.
        /// </summary>
        void CheckForUpdates();

        /// <summary>
        /// Страница загружена.
        /// </summary>
        event EventHandler PageLoaded;

        /// <summary>
        /// Загрузка начата.
        /// </summary>
        event EventHandler PageLoadStarted;

        /// <summary>
        /// Заголовок.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Номер страницы.
        /// </summary>
        int PageNum { get; }

        /// <summary>
        /// Заголовок со страницей.
        /// </summary>
        string TitleWithPage { get; }

        /// <summary>
        /// Была навигация назад.
        /// </summary>
        bool IsBackNavigatedToViewModel { get; set; }

        /// <summary>
        /// Можно вызывать каталог.
        /// </summary>
        bool CanInvokeCatalog { get; }
    }
}