using Windows.Storage;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class YoutubeThumbnailImageSourceViewModel : ImageSourceViewModelBase, IImageSourceViewModelWithSize
    {
        private readonly string youtubeId;

        private readonly string engine;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <param name="youtubeId">Идентификатор YouTube.</param>
        /// <param name="bindToPageLifetime">Привязать к времени жизни страницы.</param>
        public YoutubeThumbnailImageSourceViewModel(string engine, string youtubeId, bool bindToPageLifetime = true) : base(bindToPageLifetime)
        {
            this.engine = engine ?? "";
            this.youtubeId = youtubeId ?? "";
        }

        /// <summary>
        /// Фабрика операций.
        /// </summary>
        /// <param name="arg">Параметр.</param>
        /// <returns>Операция.</returns>
        protected override IEngineOperationsWithProgress<StorageFile, EngineProgress> OperationFactory(object arg)
        {
            return ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>().LoadMediaFile(new YoutubeLink() { Engine = engine, YoutubeId = youtubeId }, LoadMediaFileMode.DefaultSmallSize);
        }

        /// <summary>
        /// Высота.
        /// </summary>
        public int Height => 360;

        /// <summary>
        /// Ширина.
        /// </summary>
        public int Width => 480;
    }
}