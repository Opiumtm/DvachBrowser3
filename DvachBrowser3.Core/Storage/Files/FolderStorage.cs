using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using Windows.Storage.Compression;
using Windows.Storage.Streams;
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

        private StorageSizeCache sizeCache;

        /// <summary>
        /// Актуальный кэш размеров.
        /// </summary>
        protected StorageSizeCache SizeCache
        {
            get
            {
                var result =  Interlocked.CompareExchange(ref sizeCache, null, null);
                return result != null ? result.Clone() : null;
            }
            set { Interlocked.Exchange(ref sizeCache, value); }
        }

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
                var sizes = SizeCache;
                if (sizes == null)
                {
                    sizes = new StorageSizeCache();
                    await sizes.Load(sizesFile);
                    SizeCache = sizes;
                }
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
                SizeCache = sizes;
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
                SizeCache = null;
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
                var sizes = SizeCache;
                if (sizes == null)
                {
                    sizes = new StorageSizeCache();
                    await sizes.Load(sizesFile);                    
                }
                var p = await file.GetBasicPropertiesAsync();
                sizes.Sizes[file.Name] = new StorageSizeCacheItem { Size = p.Size, Date = p.DateModified };
                SizeCache = sizes;
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
                var sizes = SizeCache;
                if (sizes == null)
                {
                    sizes = new StorageSizeCache();
                    await sizes.Load(sizesFile);                    
                }
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
                SizeCache = sizes;
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
                var sizes = SizeCache;
                if (sizes == null)
                {
                    var sizesFile = await GetSizesCacheFile();
                    sizes = new StorageSizeCache();
                    await sizes.Load(sizesFile);
                    SizeCache = sizes;
                }
                return sizes.Sizes.ContainsKey(fileName);
            }
        }

        /// <summary>
        /// Сохранить объект в файл.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="file">Файл.</param>
        /// <param name="tempFolder">Временная директория.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="compress">Сжимать файл.</param>
        /// <returns>Таск.</returns>
        public async Task WriteXmlObject<T>(StorageFile file, StorageFolder tempFolder, T obj, bool compress)
        {
            var serializer = Services.GetServiceOrThrow<ISerializerCacheService>().GetSerializer<T>();
            await file.ReplaceContent(tempFolder, async str =>
            {
                if (compress)
                {
                    using (var comp = new Compressor(str, CompressAlgorithm.Mszip, 0))
                    {
                        using (var wr = new StreamWriter(comp.AsStreamForWrite(), Encoding.UTF8))
                        {
                            using (var xml = XmlWriter.Create(wr))
                            {
                                await serializer.WriteObjectAsync(xml, obj);
                            }
                        }                        
                    }
                }
                else
                {
                    using (var wr = new StreamWriter(str.AsStream(), Encoding.UTF8))
                    {
                        using (var xml = XmlWriter.Create(wr))
                        {
                            await serializer.WriteObjectAsync(xml, obj);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Читать объект из файла.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="file">Файл.</param>
        /// <param name="compress">Сжимать файл.</param>
        /// <returns>Объект.</returns>
        public async Task<T> ReadXmlObject<T>(StorageFile file, bool compress)
        {
            var serializer = Services.GetServiceOrThrow<ISerializerCacheService>().GetSerializer<T>();
            return await file.PoliteRead(async str =>
            {
                if (compress)
                {
                    using (var comp = new Decompressor(str))
                    {
                        using (var rd = new StreamReader(comp.AsStreamForRead(), Encoding.UTF8))
                        {
                            using (var xml = XmlReader.Create(rd))
                            {
                                return (T) await serializer.ReadObjectAsync(xml);
                            }
                        }
                    }
                }
                else
                {
                    using (var rd = new StreamReader(str.AsStream(), Encoding.UTF8))
                    {
                        using (var xml = XmlReader.Create(rd))
                        {
                            return (T)await serializer.ReadObjectAsync(xml);
                        }
                    }                    
                }
            }, TimeSpan.FromSeconds(3));
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
        /// Сохранить медиа файл.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="originalFile">Оригинальный файл.</param>
        /// <param name="deleteOriginal">Удалить оригинальный файл.</param>
        /// <returns>Результат.</returns>
        public async Task WriteMediaFile(StorageFile file, StorageFile originalFile, bool deleteOriginal)
        {
            if (deleteOriginal)
            {
                bool ok;
                try
                {
                    await originalFile.MoveAndReplaceAsync(file);
                    ok = true;
                }
                catch (Exception)
                {
                    ok = false;
                }
                if (!ok)
                {
                    await originalFile.CopyAndReplaceAsync(file);
                    await originalFile.DeleteAsync();
                }
            }
            else
            {
                await originalFile.CopyAndReplaceAsync(file);
            }
        }

        /// <summary>
        /// Сохранить медиа файл.
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