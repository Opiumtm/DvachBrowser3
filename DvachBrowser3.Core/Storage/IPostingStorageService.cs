using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Сервис хранения данных постинга.
    /// </summary>
    public interface IPostingStorageService : IStorageBase
    {
        /// <summary>
        /// Сохранить данные постинга.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SavePostingData(PostingData data);

        /// <summary>
        /// Загрузить данные постинга.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Данные.</returns>
        Task<PostingData> LoadPostingData(BoardLinkBase link);            
    }
}