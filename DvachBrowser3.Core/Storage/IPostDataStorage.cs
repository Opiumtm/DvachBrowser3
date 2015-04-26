using System.Threading.Tasks;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранение данных для постинга.
    /// </summary>
    public interface IPostDataStorage : ICacheFolderInfo
    {
        /// <summary>
        /// Хранилище медиа файлов.
        /// </summary>
        IPostingMediaStore MediaStorage { get; }

        /// <summary>
        /// Сохранить данные постинга.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SavePostData(PostingData data);

        /// <summary>
        /// Удлаить данные постинга.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Таск.</returns>
        Task DeletePostingData(BoardLinkBase link);

        /// <summary>
        /// Загрузить данные постинга.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Данные постинга.</returns>
        Task<PostingData> LoadPostData(BoardLinkBase link);
    }
}