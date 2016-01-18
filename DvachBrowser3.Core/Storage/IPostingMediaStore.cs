using System;
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
        /// Добавить медиа файл (оригинальный файл не удаляется).
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <returns>Идентификатор файла.</returns>
        Task<PostingMediaStoreItem> AddMediaFile(StorageFile file);

        /// <summary>
        /// Получить медиа файл.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Медиа файл (null - не найден).</returns>
        Task<PostingMediaStoreItem?> GetMediaFile(string id);

        /// <summary>
        /// Удалить медиа файл.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Таск.</returns>
        Task DeleteMediaFile(string id);

        /// <summary>
        /// Получить ссылку на файл в хранилище.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Ссылка.</returns>
        Uri GetMediaFileUri(string id);
    }
}