using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Провайдер директории хранилища.
    /// </summary>
    public interface ILocalFolderProvider
    {
        /// <summary>
        /// Получить хранилище.
        /// </summary>
        /// <returns>Хранилище.</returns>
        Task<StorageFolder> GetFolder();
    }
}