using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;

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
        public async Task Save(StorageFile file, BoardLinkBase link)
        {
            try
            {
                var folder = await GetFolder();
                var id = Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link);
                await file.CopyAsync(folder, id + ".cache", NameCollisionOption.ReplaceExisting);
            }
            catch
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
        }

        /// <summary>
        /// Загрузить.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Файл.</returns>
        public async Task<StorageFile> Load(BoardLinkBase link)
        {
            try
            {
                var folder = await GetFolder();
                var id = Services.GetServiceOrThrow<ILinkHashService>().GetLinkHash(link);
                var file = await folder.GetFileAsync(id + ".cache");
                return file;
            }
            catch
            {
                return null;
            }
        }
    }
}