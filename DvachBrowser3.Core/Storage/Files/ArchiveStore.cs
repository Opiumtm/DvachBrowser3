using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Other;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище архивов.
    /// </summary>
    public class ArchiveStore : GroupFolderStorage, IArchiveStore
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="cacheDescription">Описание.</param>
        public ArchiveStore(IServiceProvider services, string folderName, string cacheDescription) : base(services, folderName, ulong.MaxValue, ulong.MaxValue, cacheDescription)
        {
            CachedDb = new CachedFile<ArchiveCollection>(services, this, GetArchivesBaseFile, GetDataFolder, false);
        }

        /// <summary>
        /// Кэшированный файл.
        /// </summary>
        protected readonly CachedFile<ArchiveCollection> CachedDb;

        /// <summary>
        /// Сериализованный доступ.
        /// </summary>
        protected readonly SerializedAccessManager<object> DbAccessManager = new SerializedAccessManager<object>();

        /// <summary>
        /// Удалить старые данные из кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        /// <remarks>Хранилище архивов не поддерживает автоматическое удаление.</remarks>
        public override Task RecycleCache()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Очистить кэш.
        /// </summary>
        /// <returns>Таск.</returns>
        public override async Task ClearCache()
        {
            await DbAccessManager.QueueAction(async () =>
            {
                await base.ClearCache();
                await CachedDb.Delete();
                return EmptyResult;
            });
        }

        /// <summary>
        /// Записать файл в медиа-хранилище (временный файл будет удалён).
        /// </summary>
        /// <param name="archiveId">Идентификатор архива.</param>
        /// <param name="link">Ссылка.</param>
        /// <param name="tempFile">Временный файл.</param>
        /// <returns>Файл в хранилище.</returns>
        public async Task<StorageFile> MoveToMediaStorage(Guid archiveId, BoardLinkBase link, StorageFile tempFile)
        {
            var cacheDir = await GetCacheFolder();
            var dir = await cacheDir.CreateFolderAsync(archiveId.ToString(), CreationCollisionOption.OpenIfExists);
            var newFile = await cacheDir.CreateFileAsync(GetMediaFileName(link), CreationCollisionOption.OpenIfExists);
            await WriteCacheMediaFile(dir, newFile, tempFile, true);
            return newFile;
        }

        /// <summary>
        /// Получить файл из медиа-хранилища.
        /// </summary>
        /// <param name="archiveId">Идентификатор архива.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Файл (null, если не найден).</returns>
        public async Task<StorageFile> GetFromMediaStorage(Guid archiveId, BoardLinkBase link)
        {
            var fileName = GetMediaFileName(link);
            var fullName = string.Format("{0}/{1}", archiveId, fileName);
            var hasFile = await FindFileInCache(fullName);
            if (!hasFile)
            {
                return null;
            }
            var cacheDir = await GetCacheFolder();
            var dir = await cacheDir.CreateFolderAsync(archiveId.ToString(), CreationCollisionOption.OpenIfExists);
            try
            {
                return await dir.GetFileAsync(fileName);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Сохранить архив.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Черновик.</returns>
        public async Task SaveArchive(ArchiveThreadTree data)
        {
            await DbAccessManager.QueueAction(async () =>
            {
                var db = await CachedDb.Load();
                if (db == null)
                {
                    db = new ArchiveCollection() {Archives = new Dictionary<Guid, ArchiveReference>()};
                }
                db.Archives[data.Reference.Id] = data.Reference;
                const string fileName = "archive.cache";
                var folder = await (await GetCacheFolder()).CreateFolderAsync(data.Reference.Id.ToString());
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                await WriteCacheXmlObject(folder, file, folder, data, true);
                await CachedDb.SaveSync(db);
                return EmptyResult;
            });
        }

        /// <summary>
        /// Загрузить архив.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Архив.</returns>
        public async Task<ArchiveThreadTree> LoadArchive(Guid id)
        {
            try
            {
                const string fileName = "archive.cache";
                var folder = await (await GetCacheFolder()).CreateFolderAsync(id.ToString());
                var file = await folder.GetFileAsync(fileName);
                return await ReadXmlObject<ArchiveThreadTree>(file, true);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Удалить архив.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Таск.</returns>
        public async Task DeleteArchive(Guid id)
        {
            await DbAccessManager.QueueAction(async () =>
            {
                var db = await CachedDb.Load();
                if (db == null)
                {
                    db = new ArchiveCollection() {Archives = new Dictionary<Guid, ArchiveReference>()};
                }
                db.Archives.Remove(id);
                var folder = await GetCacheFolder();
                var folder2 = await folder.GetFolderAsync(id.ToString());
                await folder2.DeleteAsync();
                await RemoveFolderFromSizeCache(id.ToString());
                await CachedDb.SaveSync(db);
                return EmptyResult;
            });
        }

        /// <summary>
        /// Перечислить архивы.
        /// </summary>
        /// <returns>Архивы.</returns>
        public async Task<ArchiveReference[]> ListArchives()
        {
            return (ArchiveReference[]) await DbAccessManager.QueueAction(async () =>
            {
                var db = await CachedDb.Load();
                if (db == null)
                {
                    db = new ArchiveCollection() {Archives = new Dictionary<Guid, ArchiveReference>()};
                }
                return db.Archives.Values.ToArray();
            });
        }

        /// <summary>
        /// Получить размер архива.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Размер.</returns>
        public async Task<ulong> GetArchiveSize(Guid id)
        {
            var l = await SizeAccessManager.QueueAction(async () =>
            {
                using (var sizes = await GetSizeCacheImpl(true))
                {
                    var fn = $"{id}/";
                    return (await sizes.GetAllItems()).Where(s => s.Key.StartsWith(fn, StringComparison.OrdinalIgnoreCase)).Select(s => s.Value).Aggregate<StorageSizeCacheItem, ulong>(0, (current, r) => current + r.Size);                
                }
            });
            return (ulong) l;
        }

        /// <summary>
        /// Получить файл с данными черновиков.
        /// </summary>
        /// <returns>Файл с данными черновиков.</returns>
        protected async Task<StorageFile> GetArchivesBaseFile()
        {
            return await (await GetDataFolder()).CreateFileAsync("archives.dat", CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Получить имя медиа файла.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public string GetMediaFileName(BoardLinkBase link)
        {
            return string.Format("{0}.cache", Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link));
        }
    }
}