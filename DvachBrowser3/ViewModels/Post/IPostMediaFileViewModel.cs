using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Медиа файлпоста.
    /// </summary>
    public interface IPostMediaFileViewModel : IPostPartViewModel
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// Изображение предварительного просмотра.
        /// </summary>
        IImageSourceViewModelWithSize ThumbnailImage { get; }

        /// <summary>
        /// Есть изображение предварительного просмотра.
        /// </summary>
        bool HasThumbnail { get; }

        /// <summary>
        /// Размер в байтах.
        /// </summary>
        int? Size { get; }

        /// <summary>
        /// Высота.
        /// </summary>
        int? Height { get; }

        /// <summary>
        /// Ширина.
        /// </summary>
        int? Width { get; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Веб-ссылка.
        /// </summary>
        string WebLink { get; }
    }
}