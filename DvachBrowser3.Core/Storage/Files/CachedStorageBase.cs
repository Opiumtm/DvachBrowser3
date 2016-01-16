using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище с кэшем.
    /// </summary>
    public abstract class CachedStorageBase : StorageBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Директория.</param>
        protected CachedStorageBase(IServiceProvider services, string folderName) : base(services, folderName)
        {
            syncAggregator = new TimePeriodDataAggregator<string, StorageSizeCacheItem>(TimeSpan.FromSeconds(0.5), SaveSyncData);
        }

        protected static readonly SerializedAccessManager<object> SizeAccessManager = new SerializedAccessManager<object>();

        protected static readonly object EmptyResult = new object();

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

        protected abstract Task DoRecycleCache(IStorageSizeCache sizes);

        private readonly TimePeriodDataAggregator<string, StorageSizeCacheItem> syncAggregator;

        /// <summary>
        /// Синхронизировать кэш файла.
        /// </summary>
        /// <param name="fn">Имя файла.</param>
        /// <param name="fs">Размер файла.</param>
        /// <returns>Таск.</returns>
        protected Task DoSyncCacheFileSize(string fn, StorageSizeCacheItem fs)
        {
            syncAggregator.Push(fn, fs);
            return Task.FromResult(true);
        }

        private async Task SaveSyncData(KeyValuePair<string, StorageSizeCacheItem>[] files)
        {
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    await sizes.SetFilesSize(files);
                    await DoRecycleCache(sizes);
                    await sizes.Commit();
                    return EmptyResult;
                }
            });
        }
    }
}