using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Links;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище ссылок на борды.
    /// </summary>
    public interface IBoardReferencesStore
    {
        /// <summary>
        /// Сохранить ссылки на борды.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SaveBoardReferences(BoardReferences data);

        /// <summary>
        /// Загрузить ссылки на борды.
        /// </summary>
        /// <param name="rootLink">Корневая ссылка.</param>
        /// <returns>Таск.</returns>
        Task<BoardReferences> LoadBoardReferences(BoardLinkBase rootLink);

        /// <summary>
        /// Получить ссылку на борду.
        /// </summary>
        /// <param name="boardLink">Ссылка.</param>
        /// <returns>Ссылка на борду.</returns>
        Task<BoardReference> GetBoardReference(BoardLinkBase boardLink);
    }
}