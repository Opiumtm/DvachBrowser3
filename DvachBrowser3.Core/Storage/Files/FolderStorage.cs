using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище в файловой системе.
    /// </summary>
    public class FolderStorage : CachedStorageBase, ICacheFolderInfo
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="maxCacheSize">Максимальный размер кэша в байтах.</param>
        /// <param name="normalCacheSize">Нормальный размер кэша в байтах.</param>
        /// <param name="cacheDescription">Описание кэша.</param>
        public FolderStorage(IServiceProvider services, string folderName, ulong maxCacheSize, ulong normalCacheSize, string cacheDescription)
            : base(services, folderName)
        {
            MaxCacheSize = maxCacheSize;
            NormalCacheSize = normalCacheSize;
            CacheDescription = cacheDescription;
        }

        /// <summary>
        /// Максимальный размер кэша в байтах.
        /// </summary>
        public ulong MaxCacheSize { get; private set; }

        /// <summary>
        /// Нормальный размер кэша в байтах.
        /// </summary>
        public ulong NormalCacheSize { get; private set; }

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
        /// Получить файл в кэше.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Файл.</returns>
        public async Task<StorageFile> GetCacheFile(string fileName)
        {
            var cacheDir = await GetCacheFolder();
            return await cacheDir.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Получить файл в кэше (null, если нет такого файла).
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Файл.</returns>
        public async Task<StorageFile> GetCacheFileOrNull(string fileName)
        {
            var cacheDir = await GetCacheFolder();
            if (!(await FindFileInCache(fileName)))
            {
                return null;
            }
            return await cacheDir.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        }
        
        /// <summary>
        /// Описание.
        /// </summary>
        public string CacheDescription { get; private set; }

        /// <summary>
        /// Получить размер кэша.
        /// </summary>
        /// <returns>Размер кэша.</returns>
        public virtual async Task<ulong> GetCacheSize()
        {
            return (ulong) await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(true))
                {
                    return sizes.GetTotalSize();
                }
            });
        }

        /// <summary>
        /// Получить список файлов, имеющих иммунитет к очистке старых файлов кэша.
        /// </summary>
        /// <returns>Список файлов.</returns>
        protected virtual Task<HashSet<string>> GetRecycleImmunity()
        {
            return Task.FromResult(new HashSet<string>());
        }

        /// <summary>
        /// Пересинхронизировать размер кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual async Task ResyncCacheSize()
        {
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    var cacheDir = await GetCacheFolder();
                    var files = await cacheDir.GetFilesAsync();
                    sizes.DeleteAllItems();
                    foreach (var f in files)
                    {
                        try
                        {
                            var p = await f.GetBasicPropertiesAsync();
                            sizes.SetFileSize(f.Name, new StorageSizeCacheItem { Size = p.Size, Date = p.DateModified });
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                            // игнорируем
                        }
                    }
                    sizes.Commit();
                    return EmptyResult;
                }
            });
        }

        /// <summary>
        /// Очистить кэш.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual async Task ClearCache()
        {
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    var cacheDir = await GetCacheFolder();
                    var files = await cacheDir.GetFilesAsync();
                    var immunitySet = await GetRecycleImmunity();
                    var toDelete = files.ToArray();
                    var visitedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var f in toDelete)
                    {
                        visitedFiles.Add(f.Name);
                        if (!immunitySet.Contains(f.Name))
                        {
                            try
                            {
                                await f.DeleteAsync();
                                sizes.DeleteItem(f.Name);
                            }
                            // ReSharper disable once EmptyGeneralCatchClause
                            catch
                            {
                                // ignore
                            }
                        }
                    }                    
                    foreach (var n in sizes.GetAllFiles().ToArray().Where(n => !visitedFiles.Contains(n)))
                    {
                        sizes.DeleteItem(n);
                    }
                    sizes.Commit();
                    return EmptyResult;
                }
            });
        }

        /// <summary>
        /// Синхронизировать размер файла кэша.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <returns>Таск.</returns>
        protected async Task SyncCacheFileSize(StorageFile file)
        {
            var p = await file.GetBasicPropertiesAsync();
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    sizes.SetFileSize(file.Name, new StorageSizeCacheItem { Size = p.Size, Date = p.DateModified });
                    sizes.Commit();
                    return EmptyResult;
                }
            });
            await RecycleCache();
        }

        /// <summary>
        /// Удалить файл из кэша.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Таск.</returns>
        protected async Task RemoveFromSizeCache(string fileName)
        {
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    sizes.DeleteItem(fileName);
                    sizes.Commit();
                    return EmptyResult;
                }
            });
        }

        /// <summary>
        /// Удалить старые данные из кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual async Task RecycleCache()
        {
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    var totalSize = sizes.GetTotalSize();
                    if (totalSize < MaxCacheSize) return EmptyResult;
                    var cacheDir = await GetCacheFolder();
                    var immunitySet = await GetRecycleImmunity();
                    var toCheck = sizes.GetAllItems().OrderBy(kv => kv.Value.Date).ToArray();
                    foreach (var f in toCheck)
                    {
                        if (totalSize <= NormalCacheSize)
                        {
                            break;
                        }
                        if (!immunitySet.Contains(f.Key))
                        {
                            try
                            {
                                var file = await cacheDir.GetFileAsync(f.Key);
                                await file.DeleteAsync();
                                totalSize -= f.Value.Size;
                                sizes.DeleteItem(f.Key);
                            }
                            // ReSharper disable once EmptyGeneralCatchClause
                            catch
                            {
                                // игнорируем
                            }
                        }
                    }
                    sizes.Commit();
                    return EmptyResult;
                }
            });
        }

        /// <summary>
        /// Найти файл в кэше.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Результат поиска.</returns>
        public async Task<bool> FindFileInCache(string fileName)
        {
            try
            {
                return (bool) await SizeAccessManager.QueueAction(async () =>
                {
                    using (var sizes = await GetSizeCacheImpl(true))
                    {
                        return sizes.IsItemPresent(fileName);
                    }
                });
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return false;
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
        public async Task WriteCacheXmlObject<T>(StorageFile file, StorageFolder tempFolder, T obj, bool compress) where T : class 
        {
            await WriteXmlObject(file, tempFolder, obj, compress);
            await SyncCacheFileSize(file);
        }

        /// <summary>
        /// Сохранить объект в файл (с обновлением статистики в кэше размеров файлов).
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="tempFolder">Временная директория.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="compress">Сжимать файл.</param>
        /// <returns>Таск.</returns>
        public async Task WriteCacheString(StorageFile file, StorageFolder tempFolder, string obj, bool compress)
        {
            await WriteString(file, tempFolder, obj, compress);
            await SyncCacheFileSize(file);
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
            await SyncCacheFileSize(file);           
        }
    }
}