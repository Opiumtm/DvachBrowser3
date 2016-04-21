using System;
using Windows.Storage;
using DvachBrowser3.Engines;
using DvachBrowser3.Logic;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления предпросмотра изображения постов.
    /// </summary>
    public sealed class ThumbnailImageSourceViewModel : ImageSourceViewModelBase, IImageSourceViewModelWithSize
    {
        private readonly PostImage thumbnail;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="thumbnail">Данные изображения.</param>
        /// <param name="bindToPageLifetime">Привязать к времени жизни страницы.</param>
        public ThumbnailImageSourceViewModel(PostImage thumbnail, bool bindToPageLifetime = true) : base(bindToPageLifetime)
        {
            if (thumbnail == null) throw new ArgumentNullException(nameof(thumbnail));
            this.thumbnail = thumbnail;
        }

        /// <summary>
        /// Получить ключ кэширования.
        /// </summary>
        /// <returns>Ключ кэширования.</returns>
        protected override string GetCacheKey()
        {
            return ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetLinkHash(thumbnail.Link);
        }

        /// <summary>
        /// Фабрика операций.
        /// </summary>
        /// <param name="arg">Параметр.</param>
        /// <returns>Операция.</returns>
        protected override IEngineOperationsWithProgress<StorageFile, EngineProgress> OperationFactory(object arg)
        {
            return ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>().LoadMediaFile(thumbnail.Link, LoadMediaFileMode.DefaultSmallSize);
        }

        /// <summary>
        /// Высота.
        /// </summary>
        public int Height => thumbnail.Height;

        /// <summary>
        /// Ширина.
        /// </summary>
        public int Width => thumbnail.Width;

        /// <summary>
        /// Получить URL кэша изображения.
        /// </summary>
        /// <returns>URL кэша.</returns>
        protected override Uri GetImageCacheUri()
        {
            return ServiceLocator.Current.GetServiceOrThrow<IStorageService>().SmallImages.GetStoredImageUri(thumbnail.Link);
        }
    }
}