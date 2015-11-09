using System;
using System.ComponentModel;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    // Можно использовать вот это https://github.com/XamlAnimatedGif/XamlAnimatedGif

    // Размер баннера двача - 300x100

    /// <summary>
    /// Баннер.
    /// </summary>
    public interface IBoardPageBannerViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Поведение.
        /// </summary>
        BoardPageBannerBehavior Behavior { get; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase BannerLink { get; }

        /// <summary>
        /// Ссылка на изображение.
        /// </summary>
        BoardLinkBase BannerImageLink { get; }

        /// <summary>
        /// URI загруженного баннера.
        /// </summary>
        Uri LoadedBannerUri { get; }

        /// <summary>
        /// Высота.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Ширина.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Попробовать загрузить баннер.
        /// </summary>
        void TryLoadBanner();

        /// <summary>
        /// Баннер загружается.
        /// </summary>
        bool IsLoading { get; }

        /// <summary>
        /// Ошибка.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Баннер загружен.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Баннер загружен.
        /// </summary>
        event EventHandler BannerLoaded;

        /// <summary>
        /// Ошибка загрузки баннера.
        /// </summary>
        event EventHandler BannerLoadError;
    }

    /// <summary>
    /// Что делать с баннером.
    /// </summary>
    public enum BoardPageBannerBehavior
    {
        /// <summary>
        /// Запрещено к показу.
        /// </summary>
        Disabled,

        /// <summary>
        /// Показывать.
        /// </summary>
        Show,

        /// <summary>
        /// Скрывать.
        /// </summary>
        Hide,
    }

    /// <summary>
    /// Тип медиа файла баннера.
    /// </summary>
    public enum BoardPageBannerMediaType
    {
        /// <summary>
        /// JPEG.
        /// </summary>
        Jpeg,

        /// <summary>
        /// PNG.
        /// </summary>
        Png,

        /// <summary>
        /// GIF.
        /// </summary>
        Gif,

        /// <summary>
        /// Другой тип баннера.
        /// </summary>
        Other
    }
}