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
            SmallImages = new MediaStorage(services, "images", 11 * 1024 * 1024, 10 * 1024 * 1024, "Маленькие изображения");
            FullSizeMediaFiles = new MediaStorage(services, "media", 11 * 1024 * 1024, 10 * 1024 * 1024, "Полноразмерные медиафайлы");
            ThreadData = new ThreadDataStorage(services, "threads", 11 * 1024 * 1024, 10 * 1024 * 1024, "Данные тредов");
            PostData = new PostDataStorage(services, "posting", 6 * 1024 * 1024, 5 * 1024 * 1024, "Данные постинга", 
                new PostingMediaStore(services, "posting-img", 11 * 1024 * 1024, 10 * 1024 * 1024, "Изображения постинга"));
            Drafts = new DraftDataStorage(services, "drafts", "Черновики", new DraftMediaStore(services, "drafts-img", "Изображения черновиков"));
            Archives = new ArchiveStore(services, "archive", "Архив");
            CacheFolders = new ICacheFolderInfo[]
            {
                ThreadData,
                SmallImages,
                FullSizeMediaFiles,
                PostData,
                PostData.MediaStorage,
                Drafts,
                Drafts.MediaStorage,
                Archives
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
    }
}