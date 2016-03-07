using System.ComponentModel;
using DvachBrowser3.Links;
using DvachBrowser3.Styles;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Медиа файлпоста.
    /// </summary>
    public interface IPostMediaFileViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; }

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

        /// <summary>
        /// Строка с информацией.
        /// </summary>
        string InfoString { get; }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        IStyleManager StyleManager { get; }

    }
}