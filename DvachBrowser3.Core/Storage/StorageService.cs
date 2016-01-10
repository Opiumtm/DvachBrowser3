using System;
using DvachBrowser3.Storage.Files;

namespace DvachBrowser3.Storage
{
    public sealed class StorageService : ServiceBase, IStorageService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public StorageService(IServiceProvider services) : base(services)
        {
            SmallImages = new MediaStorage(services, "images", new CacheRecycleConfig()
            {
                MaxSize = 6 * 1024 * 1024,
                NormalSize = 5 * 1024 * 1024,
                MaxFiles = 1000,
                NormalFiles = 800
            }, "Маленькие изображения");
            FullSizeMediaFiles = new MediaStorage(services, "media", new CacheRecycleConfig()
            {
                MaxSize = 24 * 1024 * 1024,
                NormalSize = 20 * 1024 * 1024,
                MaxFiles = 500,
                NormalFiles = 400
            }, "Полноразмерные медиафайлы");
            ThreadData = new ThreadDataStorage(services, "threads", new CacheRecycleConfig()
            {
                MaxSize = 12 * 1024 * 1024,
                NormalSize = 10 * 1024 * 1024,
                MaxFiles = 300,
                NormalFiles = 250
            }, "Данные тредов");
            PostData = new PostDataStorage(services, "posting", new CacheRecycleConfig()
            {
                MaxSize = 6 * 1024 * 1024,
                NormalSize = 5 * 1024 * 1024,
                MaxFiles = 240,
                NormalFiles = 200
            }, "Данные постинга", new PostingMediaStore(services, "posting-img", new CacheRecycleConfig()
            {
                MaxSize = 12 * 1024 * 1024,
                NormalSize = 10 * 1024 * 1024,
                MaxFiles = 120,
                NormalFiles = 100
            }, "Изображения постинга"));
            Drafts = new DraftDataStorage(services, "drafts", "Черновики", new DraftMediaStore(services, "drafts-img", "Изображения черновиков"));
            Archives = new ArchiveStore(services, "archive", "Архив");
            CurrentPostStore = new CurrentPostStore(services, "other", "currentposts.cache");
            CustomData = new CustomDataStore(services, "custom", new CacheRecycleConfig()
            {
                MaxSize = 5 * 1024 * 1024,
                NormalSize = 3 * 1024 * 1024,
                MaxFiles = 120,
                NormalFiles = 100
            }, "Прочие данные");
            CacheFolders = new ICacheFolderInfo[]
            {
                ThreadData,
                SmallImages,
                FullSizeMediaFiles,
                PostData,
                PostData.MediaStorage,
                Drafts,
                Drafts.MediaStorage,
                Archives,
                CustomData
            };
        }

        /// <summary>
        /// Директории данных.
        /// </summary>
        public ICacheFolderInfo[] CacheFolders { get; private set; }

        /// <summary>
        /// Маленькие изображения.
        /// </summary>
        public IMediaStorage SmallImages { get; private set; }

        /// <summary>
        /// Полноразмерные медиафайлы.
        /// </summary>
        public IMediaStorage FullSizeMediaFiles { get; private set; }

        /// <summary>
        /// Данные тредов.
        /// </summary>
        public IThreadDataStorage ThreadData { get; private set; }

        /// <summary>
        /// Данные постов.
        /// </summary>
        public IPostDataStorage PostData { get; private set; }

        /// <summary>
        /// Черновики.
        /// </summary>
        public IDraftDataStorage Drafts { get; private set; }

        /// <summary>
        /// Архивы.
        /// </summary>
        public IArchiveStore Archives { get; private set; }

        /// <summary>
        /// Хранилище текущих постов.
        /// </summary>
        public ICurrentPostStore CurrentPostStore { get; private set; }

        /// <summary>
        /// Любые данные.
        /// </summary>
        public ICustomDataStore CustomData { get; private set; }
    }
}