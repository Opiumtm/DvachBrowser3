﻿using System;
using System.Collections.Generic;
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
        public async Task<ulong> GetCacheSize()
        {
            using (await CacheLock.LockAsync())
            {
                var sizes = await GetSizeCacheForRead(GetSizesCacheFile);
                return sizes.Sizes.Values.Aggregate<StorageSizeCacheItem, ulong>(0, (current, r) => current + r.Size);
            }
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
                var dataDir = await GetDataFolder();
                var cacheDir = await GetCacheFolder();
                var sizesFile = await GetSizesCacheFile();
                var sizes = await GetSizeCacheForChange(sizesFile);
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
                            sizes.Sizes.Remove(f.Name);
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                            // ignore
                        }                        
                    }
                }
                foreach (var n in sizes.Sizes.Keys.Where(n => !visitedFiles.Contains(n)))
                {
                    sizes.Sizes.Remove(n);
                }
                SetSizeCache(sizes);
                await sizes.Save(sizesFile, dataDir);
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
        /// Синхронизировать размер файла кэша.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Таск.</returns>
        protected async Task RemoveFromSizeCache(string fileName)
        {
            using (await CacheLock.LockAsync())
            {
                var dataDir = await GetDataFolder();
                var sizesFile = await GetSizesCacheFile();
                var sizes = await GetSizeCacheForChange(sizesFile);
                sizes.Sizes.Remove(fileName);
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
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
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
                var sizesFile = await GetSizesCacheFile();
                var sizes = await GetSizeCacheForChange(sizesFile);
                var totalSize = sizes.Sizes.Values.Aggregate<StorageSizeCacheItem, ulong>(0, (current, r) => current + r.Size);
                if (totalSize < MaxCacheSize) return;
                var dataDir = await GetDataFolder();
                var cacheDir = await GetCacheFolder();
                var immunitySet = await GetRecycleImmunity();
                var toCheck = sizes.Sizes.OrderBy(kv => kv.Value.Date).ToArray();
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
                            sizes.Sizes.Remove(f.Key);
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                            // игнорируем
                        }                        
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
        public async Task WriteCacheXmlObject<T>(StorageFile file, StorageFolder tempFolder, T obj, bool compress) where T : class 
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