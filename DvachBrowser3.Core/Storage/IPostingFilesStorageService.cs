using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Сервис хранения файлов для постинга.
    /// </summary>
    public interface IPostingFilesStorageService : IStorageBase
    {
        /// <summary>
        /// Добавить файл для постинга.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <returns>Идентификатор.</returns>
        Task<string> Add(StorageFile file);

        /// <summary>
        /// Получить файл постинга.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Файл.</returns>
        Task<StorageFile> Get(string id);

        /// <summary>
        /// Удалить файл постинга.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Таск.</returns>
        Task Delete(string id);
    }
}