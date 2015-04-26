using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище коллекций ссылок.
    /// </summary>
    public class LinkCollectionStore : StorageBase, ILinkCollectionStore
    {
        private LinkCollection cachedValue;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Директория.</param>
        /// <param name="fileName">Файл базы ссылок.</param>
        public LinkCollectionStore(IServiceProvider services, string folderName, string fileName) : base(services, folderName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Файл базы ссылок.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Получить файл коллекции.
        /// </summary>
        /// <returns>Файл коллекции.</returns>
        protected async Task<StorageFile> GetCollectionFile()
        {
            return await (await GetDataFolder()).CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// Сохранить коллекцию ссылок.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SaveLinkCollection(LinkCollection data)
        {
            try
            {
                Interlocked.Exchange(ref cachedValue, await DeepCloneObject(data));
                BackgroundSaveLinkCollection(data);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private void BackgroundSaveLinkCollection(LinkCollection data)
        {
            Task.Factory.StartNew(new Action(async () =>
            {
                try
                {
                    await WriteXmlObject(await GetCollectionFile(), await GetDataFolder(), data, false);
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            }));
        }

        /// <summary>
        /// Загрузить коллекцию ссылок.
        /// </summary>
        /// <returns>Коллекция ссылок.</returns>
        public async Task<LinkCollection> LoadLinkCollection()
        {
            try
            {
                var result = Interlocked.CompareExchange(ref cachedValue, null, null);
                if (result == null)
                {
                    result = await ReadXmlObject<LinkCollection>(await GetCollectionFile(), false);
                    Interlocked.Exchange(ref cachedValue, await DeepCloneObject(result));
                }
                else
                {
                    result = await DeepCloneObject(result);
                }
                return result;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Загрузить коллекцию ссылок только для чтения.
        /// </summary>
        /// <returns>Коллекция ссылок.</returns>
        public async Task<LinkCollection> LoadLinkCollectionForReadOnly()
        {
            try
            {
                var result = Interlocked.CompareExchange(ref cachedValue, null, null);
                if (result == null)
                {
                    result = await ReadXmlObject<LinkCollection>(await GetCollectionFile(), false);
                    Interlocked.Exchange(ref cachedValue, result);
                }
                return result;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }
    }
}