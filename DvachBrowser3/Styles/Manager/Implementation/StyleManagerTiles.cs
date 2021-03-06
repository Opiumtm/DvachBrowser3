﻿using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Тайлы.
    /// </summary>
    public sealed class StyleManagerTiles : StyleManagerObjectBase, IStyleManagerTiles
    {
        /// <summary>
        /// Назначить значения.
        /// </summary>
        protected override void SetValues()
        {
            if (!IsNarrowView)
            {
                ImagePreviewHeight = 100;
                ImagePreviewWidth = 160;
                ThreadPrevewImageHeight = 100;
                ThreadPreviewImageWidth = 160;
                BoardTileEngineNameVisibility = Visibility.Visible;
                BoardTileBoardFontSize = 16;
                BoardTileEngineLogoSize = 16;
                BoardTileEngineNameFontSize = 14;
                BoardTileDescFontSize = 12;
                BoardTileHeight = 80;
                BoardTileWidth = 160;
                CatalogTileWidth = 160;
                CatalogTileHeight = 90;
                PostingMediaTileHeight = 240;
                PostingMediaTileWidth = 320;
            }
            else
            {
                ImagePreviewHeight = 90;
                ImagePreviewWidth = 130;
                ThreadPreviewImageWidth = 100;
                ThreadPrevewImageHeight = 100;
                BoardTileEngineNameVisibility = Visibility.Collapsed;
                BoardTileBoardFontSize = 14;
                BoardTileEngineLogoSize = 14;
                BoardTileEngineNameFontSize = 12;
                BoardTileDescFontSize = 11;
                BoardTileHeight = 80;
                BoardTileWidth = 90;
                CatalogTileWidth = 100;
                CatalogTileHeight = 90;
                PostingMediaTileHeight = 240;
                PostingMediaTileWidth = 320;
            }
            BoardTileClip = new RectangleGeometry()
            {
                Rect = new Rect(new Point(0, 0), new Size(BoardTileWidth, BoardTileHeight))
            };
        }

        private double imagePreviewHeight;

        /// <summary>
        /// Высота превью изображений.
        /// </summary>
        public double ImagePreviewHeight
        {
            get { return imagePreviewHeight; }
            private set
            {
                imagePreviewHeight = value;
                OnPropertyChanged();
            }
        }

        private double imagePreviewWidth;

        /// <summary>
        /// Ширина превью изображений.
        /// </summary>
        public double ImagePreviewWidth
        {
            get { return imagePreviewWidth; }
            private set
            {
                imagePreviewWidth = value;
                OnPropertyChanged();
            }
        }

        private double threadPrevewImageHeight;

        /// <summary>
        /// Высота превью изображения.
        /// </summary>
        public double ThreadPrevewImageHeight
        {
            get { return threadPrevewImageHeight; }
            private set
            {
                threadPrevewImageHeight = value;
                OnPropertyChanged();
            }
        }

        private double threadPreviewImageWidth;

        /// <summary>
        /// Ширина превью изображения.
        /// </summary>
        public double ThreadPreviewImageWidth
        {
            get { return threadPreviewImageWidth; }
            private set
            {
                threadPreviewImageWidth = value;
                OnPropertyChanged();
            }
        }

        private double boardTileBoardFontSize;

        /// <summary>
        /// Размер названия борды.
        /// </summary>
        public double BoardTileBoardFontSize
        {
            get { return boardTileBoardFontSize; }
            private set
            {
                boardTileBoardFontSize = value;
                OnPropertyChanged();
            }
        }

        private Visibility boardTileEngineNameVisibility;

        /// <summary>
        /// Видимость имени движка.
        /// </summary>
        public Visibility BoardTileEngineNameVisibility
        {
            get { return boardTileEngineNameVisibility; }
            private set
            {
                boardTileEngineNameVisibility = value;
                OnPropertyChanged();
            }
        }

        private double boardTileEngineLogoSize;

        /// <summary>
        /// Размер логотипа движка.
        /// </summary>
        public double BoardTileEngineLogoSize
        {
            get { return boardTileEngineLogoSize; }
            private set
            {
                boardTileEngineLogoSize = value;
                OnPropertyChanged();
            }
        }

        private double boardTileEngineNameFontSize;

        /// <summary>
        /// Размер шрифта имени движка.
        /// </summary>
        public double BoardTileEngineNameFontSize
        {
            get { return boardTileEngineNameFontSize; }
            private set
            {
                boardTileEngineNameFontSize = value;
                OnPropertyChanged();
            }
        }

        private double boardTileDescFontSize;

        /// <summary>
        /// Размер описания доски.
        /// </summary>
        public double BoardTileDescFontSize
        {
            get { return boardTileDescFontSize; }
            private set
            {
                boardTileDescFontSize = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Высота тайла борды.
        /// </summary>
        private double boardTileHeight;

        /// <summary>
        /// Высота тайла борды.
        /// </summary>
        public double BoardTileHeight
        {
            get { return boardTileHeight; }
            private set
            {
                boardTileHeight = value;
                OnPropertyChanged();
            }
        }

        private double boardTileWidth;

        /// <summary>
        /// Ширина тайла борды.
        /// </summary>
        public double BoardTileWidth
        {
            get { return boardTileWidth; }
            private set
            {
                boardTileWidth = value;
                OnPropertyChanged();
            }
        }

        private double catalogTileHeight;

        /// <summary>
        /// Высота тайла каталога.
        /// </summary>
        public double CatalogTileHeight
        {
            get { return catalogTileHeight; }
            private set
            {
                catalogTileHeight = value;
                OnPropertyChanged();
            }
        }

        private double catalogTileWidth;

        /// <summary>
        /// Ширина тайла каталога.
        /// </summary>
        public double CatalogTileWidth
        {
            get { return catalogTileWidth; }
            private set
            {
                catalogTileWidth = value;
                OnPropertyChanged();
            }
        }

        private double postingMediaTileHeight;

        /// <summary>
        /// Высота тайла изображения постинга.
        /// </summary>
        public double PostingMediaTileHeight
        {
            get { return postingMediaTileHeight; }
            private set
            {
                postingMediaTileHeight = value;
                OnPropertyChanged();
            }
        }

        private double postingMediaTileWidth;

        /// <summary>
        /// Ширина тайла изображения постинга.
        /// </summary>
        public double PostingMediaTileWidth
        {
            get { return postingMediaTileWidth; }
            private set
            {
                postingMediaTileWidth = value;
                OnPropertyChanged();
            }
        }

        private RectangleGeometry boardTileClip;

        /// <summary>
        /// Обрезка тайла.
        /// </summary>
        public RectangleGeometry BoardTileClip
        {
            get { return boardTileClip; }
            private set
            {
                boardTileClip = value;
                OnPropertyChanged();
            }
        }
    }
}