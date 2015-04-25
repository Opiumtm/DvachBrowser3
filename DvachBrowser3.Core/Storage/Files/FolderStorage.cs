using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Nito.AsyncEx;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище в файловой системе.
    /// </summary>
    public class FolderStorage : StorageBase, ICacheFolderInfo
    {
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
        private async Task<StorageSizeCache> GetSizeCacheForChange(StorageFile sizesFile)
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
        private async Task<StorageSizeCache> GetSizeCacheForRead(Func<Task<StorageFile>> sizesFile)
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
        private void SetSizeCache(StorageSizeCache value)
        {
            Interlocked.Exchange(ref sizeCache, value);            
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="maxCacheSize">Максимальный размер кэша в байтах.</param>
        /// <param name="cacheDescription">Описание кэша.</param>
        public FolderStorage(IServiceProvider services, string folderName, ulong maxCacheSize, string cacheDescription)
            : base(services, folderName)
        {
            MaxCacheSize = maxCacheSize;
            CacheDescription = cacheDescription;
        }

        /// <summary>
        /// Максимальный размер кэша в байтах.
        /// </summary>
        public ulong MaxCacheSize { get; private set; }

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
                var sizes = await GetSizeCacheForRead(GetSizesCacheFile);
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
                SetSizeCache(sizes);
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
                SetSizeCache(null);
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
                var sizes = await GetSizeCacheForChange(sizesFile);
                var p = await file.GetBasicPropertiesAsync();
                sizes.Sizes[file.Name] = new StorageSizeCacheItem { Size = p.Size, Date = p.DateModified };
                SetSizeCache(sizes);
                await sizes.Save(sizesFile, dataDir);
            }            
        }

        /// <summary>
        /// Синхронизировать размер файла кэша в фоновом режиме.
        /// </summary>
        /// <param name="file">Файл.</param>
        protected void BackgroundSyncCacheFileSize(StorageFile file)
        {
            Task.Factory.StartNew(new Action(async () =>
            {
                try
                {
                    await SyncCacheFileSize(file);
                }
                catch
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                }
            }));
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
                var sizes = await GetSizeCacheForChange(sizesFile);
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
                SetSizeCache(sizes);
                await sizes.Save(sizesFile, dataDir);
            }
        }

        /// <summary>
        /// Найти файл в кэше.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Результат поиска.</returns>
        public async Task<bool> FindFileInCache(string fileName)
        {
            using (await CacheLock.LockAsync())
            {
                var sizes = await GetSizeCacheForRead(GetSizesCacheFile);
                return sizes.Sizes.ContainsKey(fileName);
            }
        }

        /// <summary>
        /// Сохранить объект в файл (с обновлением статистики в кэше размеров файлов).
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="file">Файл.</param>
        /// <param name="tempFolder">Временная директория.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="compress">Сжимать файл.</param>
        /// <returns>Таск.</returns>
        public async Task WriteCacheXmlObject<T>(StorageFile file, StorageFolder tempFolder, T obj, bool compress)
        {
            await WriteXmlObject(file, tempFolder, obj, compress);
            BackgroundSyncCacheFileSize(file);
        }

        /// <summary>
        /// Сохранить медиа файл в кэше.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="originalFile">Оригинальный файл.</param>
        /// <param name="deleteOriginal">Удалить оригинальный файл.</param>
        /// <returns>Результат.</returns>
        public async Task WriteCacheMediaFile(StorageFile file, StorageFile originalFile, bool deleteOriginal)
        {
            await WriteMediaFile(file, originalFile, deleteOriginal);
            BackgroundSyncCacheFileSize(file);           
        }
    }
}