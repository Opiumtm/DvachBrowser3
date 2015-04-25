using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Хранилище медиа файлов.
    /// </summary>
    public interface IMediaStorage : ICacheFolderInfo
    {
        /// <summary>
        /// Записать файл в медиа-хранилище (временный файл будет удалён).
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="tempFile">Временный файл.</param>
        /// <returns>Таск.</returns>
        Task MoveToMediaStorage(BoardLinkBase link, StorageFile tempFile);

        /// <summary>
        /// Получить файл из медиа-хранилища.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Файл (null, если не найден).</returns>
        Task<StorageFile> GetFromMediaStorage(BoardLinkBase link);
    }
}