using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Nito.AsyncEx;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище с кэшем.
    /// </summary>
    public class CachedStorageBase : StorageBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Директория.</param>
        public CachedStorageBase(IServiceProvider services, string folderName) : base(services, folderName)
        {
        }

        /// <summary>
        /// Объект синхронизации доступа к кэшу.
        /// </summary>
        protected readonly AsyncLock CacheLock = new AsyncLock();

        private StorageSizeCache sizeCache;

        /// <summary>
        /// Получить кэш для изменений.
        /// </summary>
        /// <param name="sizesFile">Файл кэша.</param>
        /// <returns>Кэш для изменений.</returns>
        protected async Task<StorageSizeCache> GetSizeCacheForChange(StorageFile sizesFile)
        {
            var result = Interlocked.CompareExchange(ref sizeCache, null, null);
            if (result == null)
            {
                result = new StorageSizeCache();
                await result.Load(sizesFile);
            }
            else
            {
                result = result.Clone();
            }
            return result;
        }

        /// <summary>
        /// Получить кэш для чтения.
        /// </summary>
        /// <param name="sizesFile">Файл кэша.</param>
        /// <returns>Кэш для изменений.</returns>
        protected async Task<StorageSizeCache> GetSizeCacheForRead(Func<Task<StorageFile>> sizesFile)
        {
            var result = Interlocked.CompareExchange(ref sizeCache, null, null);
            if (result == null)
            {
                result = new StorageSizeCache();
                await result.Load(await sizesFile());
                Interlocked.Exchange(ref sizeCache, result);
            }
            return result;
        }

        /// <summary>
        /// Установить данные кэша.
        /// </summary>
        /// <param name="value">Новые данные.</param>
        protected void SetSizeCache(StorageSizeCache value)
        {
            Interlocked.Exchange(ref sizeCache, value);
        }
    }
}