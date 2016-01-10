using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище текущих постов.
    /// </summary>
    public class CurrentPostStore : StorageBase, ICurrentPostStore
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        /// <param name="fileName">Имя файла.</param>
        public CurrentPostStore(IServiceProvider services, string folderName, string fileName) : base(services, folderName)
        {
            this.FileName = fileName;
            CachedFile = new CachedFile<CurrentPostStoreData>(Services, this, GetCollectionFile, GetDataFolder, true);
        }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Кэшированный файл.
        /// </summary>
        protected readonly CachedFile<CurrentPostStoreData> CachedFile;

        /// <summary>
        /// Получить файл коллекции.
        /// </summary>
        /// <returns>Файл коллекции.</returns>
        protected async Task<StorageFile> GetCollectionFile()
        {
            return await (await GetDataFolder()).CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Получить текущий пост.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <returns>Пост.</returns>
        public async Task<BoardLinkBase> GetCurrentPost(BoardLinkBase threadLink)
        {
            try
            {
                if (threadLink == null)
                {
                    return null;
                }
                var idService = Services.GetServiceOrThrow<ILinkHashService>();
                var data = await CachedFile.LoadForReadOnly();
                if (data == null)
                {
                    return null;
                }
                if (data.Posts == null)
                {
                    return null;
                }
                var id = idService.GetLinkHash(threadLink);
                if (data.Posts.ContainsKey(id))
                {
                    var d = data.Posts[id];
                    if (d != null)
                    {
                        return await DeepCloneObject(d);                        
                    }
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
            return null;
        }

        /// <summary>
        /// Обновить данные.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <param name="postLink">Ссылка на пост.</param>
        protected void UpdateData(ref CurrentPostStoreData data, BoardLinkBase threadLink, BoardLinkBase postLink)
        {
            var idService = Services.GetServiceOrThrow<ILinkHashService>();
            if (data == null)
            {
                data = new CurrentPostStoreData();
            }
            if (data.Threads == null)
            {
                data.Threads = new List<BoardLinkBase>();
            }
            if (data.Posts == null)
            {
                data.Posts = new Dictionary<string, BoardLinkBase>();
            }
            if (threadLink == null)
            {
                return;
            }
            var thid = idService.GetLinkHash(threadLink);
            data.Threads = data.Threads.Where(t => idService.GetLinkHash(t) != thid).ToList();
            data.Threads.Insert(0, threadLink);
            data.Threads = data.Threads.Take(50).ToList();
            var ids = new HashSet<string>(data.Threads.Select(idService.GetLinkHash));
            var toDelete = data.Posts.Keys.Where(k => !ids.Contains(k)).ToArray();
            foreach (var td in toDelete)
            {
                data.Posts.Remove(td);
            }
            if (postLink != null)
            {
                data.Posts[thid] = postLink;                
            }
            else
            {
                data.Posts.Remove(thid);
            }
        }

        /// <summary>
        /// Установить текущий пост.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <param name="postLink">Ссылка на пост.</param>
        /// <returns>Таск.</returns>
        public async Task SetCurrentPost(BoardLinkBase threadLink, BoardLinkBase postLink)
        {
            try
            {
                if (threadLink == null)
                {
                    return;
                }
                var data = await CachedFile.Load();
                UpdateData(ref data, threadLink, postLink);
                await CachedFile.Save(data);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Установить текущий пост синхронно.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <param name="postLink">Ссылка на пост.</param>
        /// <returns>Таск.</returns>
        public async Task SetCurrentPostSync(BoardLinkBase threadLink, BoardLinkBase postLink)
        {
            try
            {
                if (threadLink == null)
                {
                    return;
                }
                var data = await CachedFile.Load();
                UpdateData(ref data, threadLink, postLink);
                await CachedFile.SaveSync(data);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Получить корневую директорию.
        /// </summary>
        /// <returns>Корневая директорию.</returns>
        public override async Task<StorageFolder> GetRootDataFolder()
        {
            return await ApplicationData.Current.RoamingFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);
        }
    }
}