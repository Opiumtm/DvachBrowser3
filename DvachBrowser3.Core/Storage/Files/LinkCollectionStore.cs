using System;
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
        /// <summary>
        /// Кэшированный файл.
        /// </summary>
        protected readonly CachedFile<LinkCollection> CachedFile;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Директория.</param>
        /// <param name="fileName">Файл базы ссылок.</param>
        public LinkCollectionStore(IServiceProvider services, string folderName, string fileName) : base(services, folderName)
        {
            FileName = fileName;
            CachedFile = new CachedFile<LinkCollection>(services, this, GetCollectionFile, GetDataFolder, false);
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
            await CachedFile.Save(data);
        }

        /// <summary>
        /// Сохранить коллекцию ссылок синхронно.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        public async Task SaveLinkCollectionSync(LinkCollection data)
        {
            await CachedFile.SaveSync(data);
        }

        /// <summary>
        /// Загрузить коллекцию ссылок.
        /// </summary>
        /// <returns>Коллекция ссылок.</returns>
        public async Task<LinkCollection> LoadLinkCollection()
        {
            return await CachedFile.Load();
        }

        /// <summary>
        /// Загрузить коллекцию ссылок только для чтения.
        /// </summary>
        /// <returns>Коллекция ссылок.</returns>
        public async Task<LinkCollection> LoadLinkCollectionForReadOnly()
        {
            return await CachedFile.LoadForReadOnly();
        }
    }
}