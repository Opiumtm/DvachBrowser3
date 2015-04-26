using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Posting;
using Nito.AsyncEx;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище черновиков.
    /// </summary>
    public class DraftDataStorage : FolderStorage, IDraftDataStorage
    {
        /// <summary>
        /// Кэшированный файл.
        /// </summary>
        protected readonly CachedFile<DraftCollection> CachedDb;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="maxCacheSize">Максимальный размер кэша.</param>
        /// <param name="normalCacheSize">Нормальный размер кэша.</param>
        /// <param name="cacheDescription">Описание.</param>
        /// <param name="mediaStorage">Хранилище медиа файлов.</param>
        public DraftDataStorage(IServiceProvider services, string folderName, ulong maxCacheSize, ulong normalCacheSize, string cacheDescription, IPostingMediaStore mediaStorage) : base(services, folderName, maxCacheSize, normalCacheSize, cacheDescription)
        {
            MediaStorage = mediaStorage;
            CachedDb = new CachedFile<DraftCollection>(services, this, GetDraftBaseFile, GetDataFolder, false);
        }

        /// <summary>
        /// Хранилище медиа файлов.
        /// </summary>
        public IPostingMediaStore MediaStorage { get; private set; }

        /// <summary>
        /// Лок базы.
        /// </summary>
        protected AsyncLock DbLock = new AsyncLock();

        /// <summary>
        /// Сохранить черновик.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Черновик.</returns>
        public async Task SaveDraft(DraftPostingData data)
        {
            using (await DbLock.LockAsync())
            {
                var db = await CachedDb.Load();
                if (db == null)
                {
                    db = new DraftCollection() { Drafts = new Dictionary<Guid, DraftReference>() };
                }
                db.Drafts[data.Reference.Id] = data.Reference;
                var fileName = string.Format("{0}.cache", data.Reference.Id);
                var folder = await GetCacheFolder();
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                await WriteCacheXmlObject(file, folder, data, true);
                await CachedDb.SaveSync(db);
            }
        }

        /// <summary>
        /// Загрузить черновик.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Черновик.</returns>
        public async Task<DraftPostingData> LoadDraft(Guid id)
        {
            try
            {
                var fileName = string.Format("{0}.cache", id);
                var folder = await GetCacheFolder();
                var file = await folder.GetFileAsync(fileName);
                return await ReadXmlObject<DraftPostingData>(file, true);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Удалить черновик.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Таск.</returns>
        public async Task DeleteDraft(Guid id)
        {
            using (await DbLock.LockAsync())
            {
                var db = await CachedDb.Load();
                if (db == null)
                {
                    db = new DraftCollection() { Drafts = new Dictionary<Guid, DraftReference>() };
                }
                db.Drafts.Remove(id);
                var folder = await GetCacheFolder();
                var fileName = string.Format("{0}.cache", id);
                var file = await folder.GetFileAsync(fileName);
                await file.DeleteAsync();
                await CachedDb.SaveSync(db);
            }
        }

        /// <summary>
        /// Перечислить черновики.
        /// </summary>
        /// <returns>Черновики.</returns>
        public async Task<DraftReference[]> ListDrafts()
        {
            using (await DbLock.LockAsync())
            {
                var db = await CachedDb.Load();
                if (db == null)
                {
                    db = new DraftCollection() { Drafts = new Dictionary<Guid, DraftReference>() };
                }
                return db.Drafts.Values.ToArray();
            }
        }

        /// <summary>
        /// Удалить старые данные из кэша.
        /// </summary>
        /// <returns>Таск.</returns>
        /// <remarks>Хранилище черновиков не поддерживает автоматическое удаление старых файлов.</remarks>
        public override Task RecycleCache()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Получить файл с данными черновиков.
        /// </summary>
        /// <returns>Файл с данными черновиков.</returns>
        protected async Task<StorageFile> GetDraftBaseFile()
        {
            return await (await GetDataFolder()).CreateFileAsync("drafts.dat", CreationCollisionOption.OpenIfExists);
        }
    }
}