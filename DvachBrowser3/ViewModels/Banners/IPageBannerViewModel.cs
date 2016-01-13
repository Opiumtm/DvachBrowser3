using System;
using System.ComponentModel;
using System.Threading.Tasks;
using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    // Можно использовать вот это https://github.com/XamlAnimatedGif/XamlAnimatedGif

    // Размер баннера двача - 300x100

    /// <summary>
    /// Баннер.
    /// </summary>
    public interface IPageBannerViewModel : INotifyPropertyChanged, ICancellableViewModel
    {
        /// <summary>
        /// Поведение.
        /// </summary>
        PageBannerBehavior Behavior { get; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase BannerLink { get; }

        /// <summary>
        /// Заголовок ссылки.
        /// </summary>
        string BannerLinkTitle { get; }

        /// <summary>
        /// Ссылка на изображение.
        /// </summary>
        BoardLinkBase BannerImageLink { get; }

        /// <summary>
        /// Тип медиа.
        /// </summary>
        PageBannerMediaType MediaType { get; }

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
        Task TryLoadBanner();

        /// <summary>
        /// Баннер загружается.
        /// </summary>
        bool IsLoading { get; }

        /// <summary>
        /// Ошибка.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Ошибка.
        /// </summary>
        Exception Error { get; }

        /// <summary>
        /// Баннер загружен.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Баннер загружен.
        /// </summary>
        event BannerLoadedEventHandler BannerLoaded;

        /// <summary>
        /// Ошибка загрузки баннера.
        /// </summary>
        event EventHandler BannerLoadError;

        /// <summary>
        /// Начата загрузка.
        /// </summary>
        event EventHandler BannerLoadStarted;

        /// <summary>
        /// Модель возобновлена.
        /// </summary>
        event EventHandler ModelResumed;
    }
}