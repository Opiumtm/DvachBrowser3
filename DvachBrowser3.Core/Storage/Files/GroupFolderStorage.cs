using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище в файловой системе с группировкой по директориям.
    /// </summary>
    public class GroupFolderStorage : CachedStorageBase, ICacheFolderInfo
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="maxCacheSize">Максимальный размер кэша в байтах.</param>
        /// <param name="normalCacheSize">Нормальный размер кэша в байтах.</param>
        /// <param name="cacheDescription">Описание кэша.</param>
        public GroupFolderStorage(IServiceProvider services, string folderName, ulong maxCacheSize, ulong normalCacheSize, string cacheDescription)
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
        /// Описание.
        /// </summary>
        public string CacheDescription { get; private set; }

        /// <summary>
        /// Получить размер кэша.
        /// </summary>
        /// <returns>Размер кэша.</returns>
        public virtual async Task<ulong> GetCacheSize()
        {
            return (ulong)await SizeAccessManager.QueueAction(async () =>
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
        /// <remarks>Хранилище архивов не поддерживает автоматическое удаление старых файлов.</remarks>
        public async Task ResyncCacheSize()
        {
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    var cacheDir = await GetCacheFolder();
                    var folders = await cacheDir.GetFoldersAsync();
                    sizes.DeleteAllItems();
                    foreach (var fld in folders)
                    {
                        try
                        {
                            var files = await fld.GetFilesAsync();
                            foreach (var f in files)
                            {
                                try
                                {
                                    var p = await f.GetBasicPropertiesAsync();
                                    var fn = $"{fld.Name}/{f.Name}";
                                    var fs = new StorageSizeCacheItem
                                    {
                                        Size = p.Size,
                                        Date = p.DateModified
                                    };
                                    sizes.SetFileSize(fn, fs);
                                }
                                // ReSharper disable once EmptyGeneralCatchClause
                                catch
                                {
                                    // игнорируем
                                }
                            }
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                            // игнорируем
                        }
                        sizes.Commit();
                    }
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
                    var folders = await cacheDir.GetFoldersAsync();
                    var toDelete = folders.ToArray();
                    var visitedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var immunity = await GetRecycleImmunity();
                    foreach (var f in toDelete)
                    {
                        var toRemove = sizes.GetAllFiles().Where(k => k.StartsWith(f.Name + "/", StringComparison.OrdinalIgnoreCase)).ToArray();
                        foreach (var fn in toRemove)
                        {
                            visitedFiles.Add(fn);
                        }
                        if (!immunity.Contains(f.Name))
                        {
                            try
                            {
                                await f.DeleteAsync();
                                foreach (var fn in toRemove)
                                {
                                    sizes.DeleteItem(fn);
                                }
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
        /// Удалить старые данные из кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual async Task RecycleCache()
        {
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    var sizesFile = await GetSizesCacheFile();
                    var totalSize = sizes.GetTotalSize();
                    if (totalSize < MaxCacheSize) return EmptyResult;
                    var dataDir = await GetDataFolder();
                    var cacheDir = await GetCacheFolder();
                    var immunitySet = await GetRecycleImmunity();
                    var toCheck = sizes.GetAllItems()
                        .Select(kv => new { kv, folder = kv.Key.Split('/').FirstOrDefault(), file = kv.Key.Split('/').Skip(1).FirstOrDefault() })
                        .Where(kv => kv.folder != null && kv.file != null)
                        .GroupBy(kv => kv.folder, StringComparer.OrdinalIgnoreCase)
                        .Select(kv => new { folder = kv.Key, total = new StorageSizeCacheItem { Date = kv.Max(d => d.kv.Value.Date), Size = kv.Aggregate(0ul, (s, d) => s + d.kv.Value.Size) }, files = kv.Select(d => d.kv.Key).Distinct(StringComparer.OrdinalIgnoreCase).ToArray() })
                        .OrderBy(kv => kv.total.Date).ToArray();
                    foreach (var f in toCheck)
                    {
                        if (totalSize <= NormalCacheSize)
                        {
                            break;
                        }
                        if (!immunitySet.Contains(f.folder))
                        {
                            try
                            {
                                var folder = await cacheDir.GetFolderAsync(f.folder);
                                await folder.DeleteAsync();
                                totalSize -= f.total.Size;
                                foreach (var fn in f.files)
                                {
                                    sizes.DeleteItem(fn);
                                }
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
        /// Удалить файл из кэша.
        /// </summary>
        /// <param name="folderName">Имя директории.</param>
        /// <returns>Таск.</returns>
        protected async Task RemoveFolderFromSizeCache(string folderName)
        {
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    var toRemove = sizes.GetAllFiles().Where(k => k.StartsWith(folderName + "/", StringComparison.OrdinalIgnoreCase)).ToArray();
                    foreach (var fileName in toRemove)
                    {
                        sizes.DeleteItem(fileName);
                    }
                    sizes.Commit();
                    return EmptyResult;
                }
            });
        }

        /// <summary>
        /// Синхронизировать размер файла кэша.
        /// </summary>
        /// <param name="folder">Директория.</param>
        /// <param name="file">Файл.</param>
        /// <returns>Таск.</returns>
        protected async Task SyncCacheFileSize(StorageFolder folder, StorageFile file)
        {
            var p = await file.GetBasicPropertiesAsync();
            await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(false))
                {
                    var fn = $"{folder.Name}/{file.Name}";
                    var fs = new StorageSizeCacheItem { Size = p.Size, Date = p.DateModified };
                    sizes.SetFileSize(fn, fs);
                    sizes.Commit();
                    return EmptyResult;
                }
            });
            await RecycleCache();
        }

        /// <summary>
        /// Сохранить объект в файл (с обновлением статистики в кэше размеров файлов).
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="folder">Директория.</param>
        /// <param name="file">Файл.</param>
        /// <param name="tempFolder">Временная директория.</param>
        /// <param name="obj">Объект.</param>
        /// <param name="compress">Сжимать файл.</param>
        /// <returns>Таск.</returns>
        public async Task WriteCacheXmlObject<T>(StorageFolder folder, StorageFile file, StorageFolder tempFolder, T obj, bool compress) where T : class
        {
            await WriteXmlObject(file, tempFolder, obj, compress);
            await SyncCacheFileSize(folder, file);
        }

        /// <summary>
        /// Сохранить медиа файл в кэше.
        /// </summary>
        /// <param name="folder">Директория.</param>
        /// <param name="file">Файл.</param>
        /// <param name="originalFile">Оригинальный файл.</param>
        /// <param name="deleteOriginal">Удалить оригинальный файл.</param>
        /// <returns>Результат.</returns>
        public async Task WriteCacheMediaFile(StorageFolder folder, StorageFile file, StorageFile originalFile, bool deleteOriginal)
        {
            await WriteMediaFile(file, originalFile, deleteOriginal);
            await SyncCacheFileSize(folder, file);
        }
    }
}