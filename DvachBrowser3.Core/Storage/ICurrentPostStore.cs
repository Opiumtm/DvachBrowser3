using System.Threading.Tasks;
using DvachBrowser3.Links;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище текущих постов.
    /// </summary>
    public interface ICurrentPostStore
    {
        /// <summary>
        /// Получить текущий пост.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <returns>Пост.</returns>
        Task<BoardLinkBase> GetCurrentPost(BoardLinkBase threadLink);

        /// <summary>
        /// Установить текущий пост.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <param name="postLink">Ссылка на пост.</param>
        /// <returns>Таск.</returns>
        Task SetCurrentPost(BoardLinkBase threadLink, BoardLinkBase postLink);

        /// <summary>
        /// Установить текущий пост синхронно.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <param name="postLink">Ссылка на пост.</param>
        /// <returns>Таск.</returns>
        Task SetCurrentPostSync(BoardLinkBase threadLink, BoardLinkBase postLink);
    }
}