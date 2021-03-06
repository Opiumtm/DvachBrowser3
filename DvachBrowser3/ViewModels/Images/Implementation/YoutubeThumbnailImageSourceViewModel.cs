﻿using System;
using Windows.Storage;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;

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
        /// Получить ключ кэширования.
        /// </summary>
        /// <returns>Ключ кэширования.</returns>
        protected override string GetCacheKey()
        {
            var link = new YoutubeLink() {Engine = engine, YoutubeId = youtubeId};
            return ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link);
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
        /// Получить URL кэша изображения.
        /// </summary>
        /// <returns>URL кэша.</returns>
        protected override Uri GetImageCacheUri()
        {
            var link = new YoutubeLink() {Engine = engine, YoutubeId = youtubeId};
            return ServiceLocator.Current.GetServiceOrThrow<IStorageService>().SmallImages.GetStoredImageUri(link);
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