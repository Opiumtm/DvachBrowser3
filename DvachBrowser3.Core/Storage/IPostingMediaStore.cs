using System.Threading.Tasks;
using Windows.Storage;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище изображений для постинга.
    /// </summary>
    public interface IPostingMediaStore : ICacheFolderInfo
    {
        /// <summary>
        /// Добавить медиа файл.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <returns>Идентификатор файла.</returns>
        Task<string> AddMediaFile(StorageFile file);

        /// <summary>
        /// Получить медиа файл.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Медиа файл (null - не найден).</returns>
        Task<StorageFile> GetMediaFile(string id);

        /// <summary>
        /// Удалить медиа файл.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Таск.</returns>
        Task DeleteMediaFile(string id);
    }
}