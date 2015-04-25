using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Nito.AsyncEx;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище в файловой системе.
    /// </summary>
    public class FolderStorage : ICacheFolderInfo
    {
        /// <summary>
        /// Объект синхронизации доступа к кэшу.
        /// </summary>
        protected readonly AsyncLock CacheLock = new AsyncLock();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="maxCacheSize">Максимальный размер кэша в байтах.</param>
        /// <param name="cacheDescription">Описание кэша.</param>
        public FolderStorage(IServiceProvider services, string folderName, ulong maxCacheSize, string cacheDescription)
        {
            Services = services;
            FolderName = folderName;
            MaxCacheSize = maxCacheSize;
            CacheDescription = cacheDescription;
        }

        /// <summary>
        /// Имя директории.
        /// </summary>
        public string FolderName { get; private set; }

        /// <summary>
        /// Максимальный размер кэша в байтах.
        /// </summary>
        public ulong MaxCacheSize { get; private set; }

        /// <summary>
        /// Сервисы.
        /// </summary>
        protected IServiceProvider Services { get; private set; }

        /// <summary>
        /// Получить корневую директорию.
        /// </summary>
        /// <returns>Корневая директорию.</returns>
        public async Task<StorageFolder> GetRootDataFolder()
        {
            return await ApplicationData.Current.LocalFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Получить директорию данных.
        /// </summary>
        /// <returns>Директория данных.</returns>
        public async Task<StorageFolder> GetDataFolder()
        {
            return await (await GetRootDataFolder()).CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Получить директорию данных.
        /// </summary>
        /// <returns>Директория данных.</returns>
        public async Task<StorageFolder> GetCacheFolder()
        {
            return await (await GetDataFolder()).CreateFolderAsync("cache", CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Получить файл кэша размеров файлов.
        /// </summary>
        /// <returns>Файл.</returns>
        public async Task<StorageFile> GetSizesCacheFile()
        {
            var dataDir = await GetDataFolder();
            return await dataDir.CreateFileAsync("sizes.dat", CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Описание.
        /// </summary>
        public string CacheDescription { get; private set; }

        /// <summary>
        /// Получить размер кэша.
        /// </summary>
        /// <returns>Размер кэша.</returns>
        public async Task<ulong> GetCacheSize()
        {
            using (await CacheLock.LockAsync())
            {
                var sizesFile = await GetSizesCacheFile();
                var sizes = new StorageSizeCache();
                await sizes.Load(sizesFile);
                return sizes.Sizes.Values.Aggregate<StorageSizeCacheItem, ulong>(0, (current, r) => current + r.Size);
            }
        }

        /// <summary>
        /// Пересинхронизировать размер кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task ResyncCacheSize()
        {
            using (await CacheLock.LockAsync())
            {
                var dataDir = await GetDataFolder();
                var cacheDir = await GetCacheFolder();
                var sizesFile = await GetSizesCacheFile();
                var sizes = new StorageSizeCache();
                var files = await cacheDir.GetFilesAsync();
                foreach (var f in files)
                {
                    try
                    {
                        var p = await f.GetBasicPropertiesAsync();
                        sizes.Sizes[f.Name] = new StorageSizeCacheItem {Size = p.Size, Date = p.DateModified};
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                        // игнорируем
                    }
                }
                await sizes.Save(sizesFile, dataDir);
            }
        }

        /// <summary>
        /// Очистить кэш.
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task ClearCache()
        {
            using (await CacheLock.LockAsync())
            {
                var cacheDir = await GetCacheFolder();
                var sizesFile = await GetSizesCacheFile();
                await cacheDir.DeleteAsync();
                await sizesFile.DeleteAsync();
            }
        }

        /// <summary>
        /// Синхронизировать размер файла кэша.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <returns>Таск.</returns>
        protected async Task SyncCacheFileSize(StorageFile file)
        {
            using (await CacheLock.LockAsync())
            {
                var dataDir = await GetDataFolder();
                var sizesFile = await GetSizesCacheFile();
                var sizes = new StorageSizeCache();
                await sizes.Load(sizesFile);
                var p = await file.GetBasicPropertiesAsync();
                sizes.Sizes[file.Name] = new StorageSizeCacheItem { Size = p.Size, Date = p.DateModified };
                await sizes.Save(sizesFile, dataDir);
            }            
        }

        /// <summary>
        /// Удалить старые данные из кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task RecycleCache()
        {
            using (await CacheLock.LockAsync())
            {
                var dataDir = await GetDataFolder();
                var cacheDir = await GetCacheFolder();
                var sizesFile = await GetSizesCacheFile();
                var sizes = new StorageSizeCache();
                await sizes.Load(sizesFile);
                var totalSize = sizes.Sizes.Values.Aggregate<StorageSizeCacheItem, ulong>(0, (current, r) => current + r.Size);
                var toCheck = sizes.Sizes.OrderBy(s => s.Value.Date).ToArray();
                foreach (var f in toCheck)
                {
                    if (totalSize <= MaxCacheSize)
                    {
                        break;
                    }
                    try
                    {
                        var file = await cacheDir.GetFileAsync(f.Key);
                        await file.DeleteAsync();
                        totalSize -= f.Value.Size;
                        sizes.Sizes.Remove(f.Key);
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                        // игнорируем
                    }
                }
                await sizes.Save(sizesFile, dataDir);
            }
        }
    }
}