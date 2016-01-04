using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

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
        protected CachedStorageBase(IServiceProvider services, string folderName) : base(services, folderName)
        {
        }

        protected readonly static SerializedAccessManager<object> SizeAccessManager = new SerializedAccessManager<object>();

        //private StorageSizeCache sizeCache;

        protected readonly static object EmptyResult = new object();

        /*
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
        }*/

        /// <summary>
        /// Фабрика сервиса хранения кэша.
        /// </summary>
        protected IStorageSizeCacheFactory SizeCacheFactory => Services.GetServiceOrThrow<IStorageSizeCacheFactory>();

        /// <summary>
        /// Получить кэш размеров.
        /// </summary>
        /// <param name="readOnly">Только для чтения.</param>
        /// <returns>Результат.</returns>
        protected async Task<IStorageSizeCache> GetSizeCacheImpl(bool readOnly)
        {
            await SizeCacheFactory.InitializeCache(FolderName);
            return await SizeCacheFactory.Get(FolderName, readOnly);
        }
    }
}