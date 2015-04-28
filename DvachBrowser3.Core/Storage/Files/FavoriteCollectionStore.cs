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
        public FavoriteCollectionStore(IServiceProvider services, string folderName, string fileName)
            : base(services, folderName, fileName)
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