using System;
using System.Threading.Tasks;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранение данных для постинга.
    /// </summary>
    public class PostDataStorage : FolderStorage, IPostDataStorage
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="maxCacheSize">Максимальный размер кэша.</param>
        /// <param name="normalCacheSize">Нормальный размер кэша.</param>
        /// <param name="cacheDescription">Описание.</param>
        /// <param name="mediaStorage">Хранилище медиа файлов.</param>
        public PostDataStorage(IServiceProvider services, string folderName, ulong maxCacheSize, ulong normalCacheSize, string cacheDescription, IPostingMediaStore mediaStorage) : base(services, folderName, maxCacheSize, normalCacheSize, cacheDescription)
        {
            MediaStorage = mediaStorage;
        }

        /// <summary>
        /// Хранилище медиа файлов.
        /// </summary>
        public IPostingMediaStore MediaStorage { get; private set; }

        /// <summary>
        /// Сохранить данные постинга.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SavePostData(PostingData data)
        {
            var fileName = string.Format("{0}.cache", Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(data.Link));
            var file = await GetCacheFile(fileName);
            var folder = await GetCacheFolder();
            await WriteCacheXmlObject(file, folder, data, true);            
        }

        /// <summary>
        /// Удлаить данные постинга.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Таск.</returns>
        public async Task DeletePostingData(BoardLinkBase link)
        {
            var fileName = string.Format("{0}.cache", Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
            var file = await GetCacheFile(fileName);
            await file.DeleteAsync();
            BackgroundRemoveFromSizeCache(fileName);
        }

        /// <summary>
        /// Загрузить данные постинга.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Данные постинга.</returns>
        public async Task<PostingData> LoadPostData(BoardLinkBase link)
        {
            try
            {
                if (link == null) return null;
                var fileName = string.Format("{0}.cache", Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
                var file = await GetCacheFile(fileName);
                return await ReadXmlObject<PostingData>(file, true);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }
    }
}