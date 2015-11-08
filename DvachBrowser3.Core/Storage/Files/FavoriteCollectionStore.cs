using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище избранных ссылок.
    /// </summary>
    public class FavoriteCollectionStore : LinkCollectionStore
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Директория.</param>
        /// <param name="fileName">Файл базы ссылок.</param>
        /// <param name="compressData">Сжимать данные.</param>
        public FavoriteCollectionStore(IServiceProvider services, string folderName, string fileName, bool compressData = false)
            : base(services, folderName, fileName, compressData)
        {
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