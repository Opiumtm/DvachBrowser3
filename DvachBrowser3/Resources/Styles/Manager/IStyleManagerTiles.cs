using System;
using System.ComponentModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Тайлы.
    /// </summary>
    public interface IStyleManagerTiles : INotifyPropertyChanged
    {
        /// <summary>
        /// Высота превью изображений.
        /// </summary>
        double ImagePreviewHeight { get; }

        /// <summary>
        /// Ширина превью изображений.
        /// </summary>
        double ImagePreviewWidth { get; }

        /// <summary>
        /// Высота превью изображения.
        /// </summary>
        double ThreadPrevewImageHeight { get; }

        /// <summary>
        /// Ширина превью изображения.
        /// </summary>
        double ThreadPreviewImageWidth { get; }

        /// <summary>
        /// Размер названия борды.
        /// </summary>
        double BoardTileBoardFontSize { get; }

        /// <summary>
        /// Видимость имени движка.
        /// </summary>
        Visibility BoardTileEngineNameVisibility { get; }

        /// <summary>
        /// Размер логотипа движка.
        /// </summary>
        double BoardTileEngineLogoSize { get; }

        /// <summary>
        /// Размер шрифта имени движка.
        /// </summary>
        double BoardTileEngineNameFontSize { get; }

        /// <summary>
        /// Размер описания доски.
        /// </summary>
        double BoardTileDescFontSize { get; }

        /// <summary>
        /// Высота тайла борды.
        /// </summary>
        double BoardTileHeight { get; }

        /// <summary>
        /// Ширина тайла борды.
        /// </summary>
        double BoardTileWidth { get; }

        /// <summary>
        /// Высота тайла каталога.
        /// </summary>
        double CatalogTileHeight { get; }

        /// <summary>
        /// Ширина тайла каталога.
        /// </summary>
        double CatalogTileWidth { get; }

        /// <summary>
        /// Высота тайла изображения постинга.
        /// </summary>
        double PostingMediaTileHeight { get; }

        /// <summary>
        /// Ширина тайла изображения постинга.
        /// </summary>
        double PostingMediaTileWidth { get; }

        /// <summary>
        /// Обрезка тайла.
        /// </summary>
        RectangleGeometry BoardTileClip { get; }
    }
}