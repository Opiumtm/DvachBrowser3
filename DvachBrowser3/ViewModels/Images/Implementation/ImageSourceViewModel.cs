using System;
using Windows.Storage;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;

namespace DvachBrowser3.ViewModels
{
    public sealed class ImageSourceViewModel : ImageSourceViewModelBase
    {
        private BoardLinkBase link;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="bindToPageLifetime">Привязать ко времени жизни страницы.</param>
        public ImageSourceViewModel(BoardLinkBase link, bool bindToPageLifetime = true) : base(bindToPageLifetime)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            this.link = link;
        }

        /// <summary>
        /// Фабрика операций.
        /// </summary>
        /// <param name="arg">Параметр.</param>
        /// <returns>Операция.</returns>
        protected override IEngineOperationsWithProgress<StorageFile, EngineProgress> OperationFactory(object arg)
        {
            return ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>().LoadMediaFile(link, LoadMediaFileMode.DefaultSmallSize);
        }

        /// <summary>
        /// Получить URL кэша изображения.
        /// </summary>
        /// <returns>URL кэша.</returns>
        protected override Uri GetImageCacheUri()
        {
            return ServiceLocator.Current.GetServiceOrThrow<IStorageService>().SmallImages.GetStoredImageUri(link);
        }
    }
}