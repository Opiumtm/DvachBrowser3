using System.Collections.Generic;
using DvachBrowser3.Navigation;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления поста.
    /// </summary>
    public interface IPostViewModel : IHyperlinkViewModel, IEntryInvalidateViewModel
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
        /// Заголовок.
        /// </summary>
        string Subject { get; }

        /// <summary>
        /// Дата.
        /// </summary>
        string Date { get; }
    }
}