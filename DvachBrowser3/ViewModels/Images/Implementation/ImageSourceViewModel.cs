using System;
using Windows.Storage;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;

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
    }
}