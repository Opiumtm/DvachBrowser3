using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище изображений для постинга.
    /// </summary>
    public class PostingMediaStore : FolderStorage, IPostingMediaStore
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="recycleConfig">Конфигурация очистки.</param>
        /// <param name="cacheDescription">Описание.</param>
        public PostingMediaStore(IServiceProvider services, string folderName, CacheRecycleConfig recycleConfig, string cacheDescription) : base(services, folderName, recycleConfig, cacheDescription)
        {
        }

        /// <summary>
        /// Добавить медиа файл.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <returns>Идентификатор файла.</returns>
        public async Task<PostingMediaStoreItem> AddMediaFile(StorageFile file)
        {
            var id = Guid.NewGuid().ToString();
            var fileName = string.Format("{0}.cache", id);
            var newFile = await (await GetCacheFolder()).CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await WriteCacheMediaFile(newFile, file, false);
            return new PostingMediaStoreItem()
            {
                Id = id,
                File = newFile
            };
        }

        /// <summary>
        /// Получить медиа файл.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Медиа файл (null - не найден).</returns>
        public async Task<PostingMediaStoreItem?> GetMediaFile(string id)
        {
            var fileName = string.Format("{0}.cache", id);
            var hasFile = await FindFileInCache(fileName);
            if (!hasFile)
            {
                return null;
            }
            var cacheDir = await GetCacheFolder();
            try
            {
                var file = await cacheDir.GetFileAsync(fileName);
                return new PostingMediaStoreItem()
                {
                    File = file,
                    Id = id
                };
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Удалить медиа файл.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Таск.</returns>
        public async Task DeleteMediaFile(string id)
        {
            var fileName = string.Format("{0}.cache", id);
            try
            {
                var cacheDir = await GetCacheFolder();
                var file = await cacheDir.GetFileAsync(fileName);
                await file.DeleteAsync();
                await RemoveFromSizeCache(fileName);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }
    }
}