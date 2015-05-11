using System.Threading;
using Windows.UI.Xaml.Media;
using DvachBrowser3.Navigation;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Предварительный просмотр.
    /// </summary>
    public interface IThumbnailViewModel : IHyperlinkViewModel
    {
        /// <summary>
        /// Родительская модель.
        /// </summary>
        IPostViewModel Parent { get; }

        /// <summary>
        /// Медиафайл.
        /// </summary>
        PostMediaBase Media { get; }

        /// <summary>
        /// Изображение.
        /// </summary>
        ImageSource Image { get; }

        /// <summary>
        /// Ключ навигации для медиафайла.
        /// </summary>
        INavigationKey MediaKey { get; }

        /// <summary>
        /// Навигация к отображению медиафайла.
        /// </summary>
        void NavigateToMedia();

        /// <summary>
        /// Сетевая операция.
        /// </summary>
        INetworkViewModel NetworkOperation { get; }

        /// <summary>
        /// Высота.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Ширина.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Высота предпросмотра.
        /// </summary>
        int ThumbnailHeight { get; }

        /// <summary>
        /// Ширина предпросмотра.
        /// </summary>
        int ThumbnailWidth { get; }

        /// <summary>
        /// Есть информация о размере.
        /// </summary>
        bool HasSize { get; }

        /// <summary>
        /// Размер в килобайтах.
        /// </summary>
        int SizeKb { get; }

        /// <summary>
        /// Строка с размерами.
        /// </summary>
        string SizeString { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        string Name { get; }
    }
}