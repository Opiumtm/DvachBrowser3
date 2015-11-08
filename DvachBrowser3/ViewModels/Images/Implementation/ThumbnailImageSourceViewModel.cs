using System;
using Windows.Storage;
using DvachBrowser3.Engines;
using DvachBrowser3.Logic;
using DvachBrowser3.Posts;

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
    }
}