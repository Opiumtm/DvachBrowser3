using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// База хранилища.
    /// </summary>
    public abstract class StorageBase : ServiceBase, IStorageBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        protected StorageBase(IServiceProvider services, string folderName) : base(services)
        {
            FolderName = folderName;
        }

        /// <summary>
        /// Имя директории.
        /// </summary>
        protected string FolderName { get; private set; }

        /// <summary>
        /// Удалить старые данные.
        /// </summary>
        /// <param name="maxMb">Максимальное количество, Мб.</param>
        /// <returns>Таск.</returns>
        public virtual async Task Recycle(int maxMb)
        {
            await RecycleHelper.Recycle(await GetFolder(), maxMb);
        }

        /// <summary>
        /// Очистить данные.
        /// </summary>
        /// <returns>Таск.</returns>
        public virtual async Task Clear()
        {
            await RecycleHelper.Clear(await GetFolder());
        }

        /// <summary>
        /// Получить общий размер.
        /// </summary>
        /// <returns>Общий размер.</returns>
        public virtual async Task<ulong> GetSize()
        {
            return await RecycleHelper.GetSize(await GetFolder());
        }

        /// <summary>
        /// Получить корневую директорию данных.
        /// </summary>
        /// <returns>Директория.</returns>
        protected async Task<StorageFolder> GetRootFolder()
        {
            return await ApplicationData.Current.LocalFolder.EnsureFolder("data");
        }

        /// <summary>
        /// Получить директорию.
        /// </summary>
        /// <returns>Директория.</returns>
        protected async Task<StorageFolder> GetFolder()
        {
            return await GetFolder(FolderName);
        }

        /// <summary>
        /// Получить директорию.
        /// </summary>
        /// <param name="folderName">Имя директории.</param>
        /// <returns>Директория.</returns>
        protected async Task<StorageFolder> GetFolder(string folderName)
        {
            return await (await GetRootFolder()).EnsureFolder(folderName);
        }
    }
}