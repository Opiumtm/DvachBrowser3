using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления поста.
    /// </summary>
    public interface IPostViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель представления.
        /// </summary>
        IPostCollectionViewModel Parent { get; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// Родительская ссылка.
        /// </summary>
        BoardLinkBase ParentLink { get; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        string Subject { get; }

        /// <summary>
        /// Текст поста.
        /// </summary>
        IPostTextViewModel Text { get; }

        /// <summary>
        /// Медиафайлы.
        /// </summary>
        IPostMediaViewModel Media { get; }

        /// <summary>
        /// Флаги.
        /// </summary>
        IPostFlagsViewModel Flags { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        IPostNameViewModel Name { get; }

        /// <summary>
        /// Дата.
        /// </summary>
        string Date { get; }

        /// <summary>
        /// Номер поста.
        /// </summary>
        int? PostNum { get; }

        /// <summary>
        /// Номер поста.
        /// </summary>
        string PostNumStr { get; }

        /// <summary>
        /// Цвет подложки изображения.
        /// </summary>
        Color ImageBackgroundColor { get; }

        /// <summary>
        /// Дополнительные данные.
        /// </summary>
        IDictionary<string, object> CustomAttachedData { get; }
    }
}