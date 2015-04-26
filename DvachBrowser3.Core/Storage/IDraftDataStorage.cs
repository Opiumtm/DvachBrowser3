using System;
using System.Threading.Tasks;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище черновиков.
    /// </summary>
    public interface IDraftDataStorage : ICacheFolderInfo
    {
        /// <summary>
        /// Хранилище медиа файлов.
        /// </summary>
        IPostingMediaStore MediaStorage { get; }

        /// <summary>
        /// Сохранить черновик.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Черновик.</returns>
        Task SaveDraft(DraftPostingData data);

        /// <summary>
        /// Загрузить черновик.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Черновик.</returns>
        Task<DraftPostingData> LoadDraft(Guid id);

        /// <summary>
        /// Удалить черновик.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Таск.</returns>
        Task DeleteDraft(Guid id);

        /// <summary>
        /// Перечислить черновики.
        /// </summary>
        /// <returns>Черновики.</returns>
        Task<DraftReference[]> ListDrafts();
    }
}