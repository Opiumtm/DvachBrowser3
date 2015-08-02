using System.Collections.Generic;
using System.ComponentModel;
using DvachBrowser3.Navigation;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления поста.
    /// </summary>
    public interface IPostViewModel : IHyperlinkViewModel, IEntryInvalidateViewModel, ICancellationTokenSource, INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская коллекция.
        /// </summary>
        IPostCollectionViewModel Parent { get; }

        /// <summary>
        /// Данные.
        /// </summary>
        PostTree Data { get; }

        /// <summary>
        /// Номер поста.
        /// </summary>
        string PostNumber { get; }

        /// <summary>
        /// Номер треда.
        /// </summary>
        string ThreadNumber { get; }

        /// <summary>
        /// Ключ навигации.
        /// </summary>
        INavigationKey NavigationKey { get; }

        /// <summary>
        /// Родительский ключ навигации.
        /// </summary>
        INavigationKey ParentNavigationKey { get; }

        /// <summary>
        /// Ответы на этот пост.
        /// </summary>
        IList<IQuoteViewModel> Quotes { get; }

        /// <summary>
        /// Медиа файлы.
        /// </summary>
        IList<IThumbnailViewModel> Media { get; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        string Subject { get; }

        /// <summary>
        /// Дата.
        /// </summary>
        string Date { get; }

        /// <summary>
        /// Постер.
        /// </summary>
        IPosterViewModel Poster { get; }

        /// <summary>
        /// Страна.
        /// </summary>
        IIconViewModel Country { get; }

        /// <summary>
        /// Иконка.
        /// </summary>
        IIconWithNameViewModel Icon { get; }

        /// <summary>
        /// Почта.
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Есть почта.
        /// </summary>
        bool HasEmail { get; }

        /// <summary>
        /// Флаги.
        /// </summary>
        IPostFlagsViewModel Flags { get; }

        /// <summary>
        /// Счётчик постов.
        /// </summary>
        int Counter { get; }

        /// <summary>
        /// Хэш поста.
        /// </summary>
        string Hash { get; }

        /// <summary>
        /// Текст поста.
        /// </summary>
        PostNodes Comment { get; }

        /// <summary>
        /// Пост попал в отображение.
        /// </summary>
        void GotIntoView();

        /// <summary>
        /// Режим отображения поста.
        /// </summary>
        PostViewMode ViewMode { get; set; }

        /// <summary>
        /// Обновить квоты.
        /// </summary>
        /// <param name="src">Новое значение.</param>
        void UpdateQuotesAndFlags(PostTree src);
    }
}