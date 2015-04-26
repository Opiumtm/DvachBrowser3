using System;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;
using DvachBrowser3.Other;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище архивов.
    /// </summary>
    public interface IArchiveStore : ICacheFolderInfo
    {
        /// <summary>
        /// Записать файл в медиа-хранилище (временный файл будет удалён).
        /// </summary>
        /// <param name="archiveId">Идентификатор архива.</param>
        /// <param name="link">Ссылка.</param>
        /// <param name="tempFile">Временный файл.</param>
        /// <returns>Файл в хранилище.</returns>
        Task<StorageFile> MoveToMediaStorage(Guid archiveId, BoardLinkBase link, StorageFile tempFile);

        /// <summary>
        /// Получить файл из медиа-хранилища.
        /// </summary>
        /// <param name="archiveId">Идентификатор архива.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Файл (null, если не найден).</returns>
        Task<StorageFile> GetFromMediaStorage(Guid archiveId, BoardLinkBase link);

        /// <summary>
        /// Сохранить архив.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Черновик.</returns>
        Task SaveArchive(ArchiveThreadTree data);

        /// <summary>
        /// Загрузить архив.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Архив.</returns>
        Task<ArchiveThreadTree> LoadArchive(Guid id);

        /// <summary>
        /// Удалить архив.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Таск.</returns>
        Task DeleteArchive(Guid id);

        /// <summary>
        /// Перечислить архивы.
        /// </summary>
        /// <returns>Архивы.</returns>
        Task<ArchiveReference[]> ListArchives();

        /// <summary>
        /// Получить размер архива.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Размер.</returns>
        Task<ulong> GetArchiveSize(Guid id);
    }
}