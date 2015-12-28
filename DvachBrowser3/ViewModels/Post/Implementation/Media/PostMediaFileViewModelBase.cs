using System;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления медиа-файла.
    /// </summary>
    public abstract class PostMediaFileViewModelBase<T> : PostPartViewModelBase, IPostMediaFileViewModel where T : PostMediaBase
    {
        /// <summary>
        /// Данные медиа.
        /// </summary>
        protected T MediaData { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="mediaData">Данные медиа.</param>
        protected PostMediaFileViewModelBase(IPostViewModel parent, T mediaData) : base(parent)
        {
            this.MediaData = mediaData;
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link => MediaData?.Link;

        /// <summary>
        /// Изображение предварительного просмотра.
        /// </summary>
        public abstract IImageSourceViewModelWithSize ThumbnailImage { get; }

        /// <summary>
        /// Есть изображение предварительного просмотра.
        /// </summary>
        public bool HasThumbnail => ThumbnailImage != null;

        /// <summary>
        /// Размер в байтах.
        /// </summary>
        public int? Size => MediaData?.Size;

        /// <summary>
        /// Высота.
        /// </summary>
        public abstract int? Height { get; }

        /// <summary>
        /// Ширина.
        /// </summary>
        public abstract int? Width { get; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Веб-ссылка.
        /// </summary>
        public string WebLink => Link?.GetWebLink()?.ToString();

        /// <summary>
        /// Строка с информацией.
        /// </summary>
        public abstract string InfoString { get; }
    }
}