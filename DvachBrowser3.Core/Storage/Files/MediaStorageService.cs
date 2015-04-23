using System;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Сервис хранения медиа данных.
    /// </summary>
    public class MediaStorageService : StorageBase, IMediaStorageService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="folderName">Имя директории.</param>
        public MediaStorageService(IServiceProvider services, string folderName) : base(services, folderName)
        {
        }

        /// <summary>
        /// Сохранение файла.
        /// </summary>
        /// <param name="file">Файл.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Таск.</returns>
        public Task Save(StorageFile file, BoardLinkBase link)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Загрузить.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Файл.</returns>
        public Task<StorageFile> Load(BoardLinkBase link)
        {
            throw new NotImplementedException();
        }
    }
}