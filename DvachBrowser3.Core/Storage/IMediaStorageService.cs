using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Сервис хранения медиафайлов.
    /// </summary>
    public interface IMediaStorageService : IStorageBase
    {
        /// <summary>
        /// Сохранение файла.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Таск.</returns>
        Task Save(StorageFile file, BoardLinkBase link);

        /// <summary>
        /// Загрузить.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Файл.</returns>
        Task<StorageFile> Load(BoardLinkBase link);
    }
}