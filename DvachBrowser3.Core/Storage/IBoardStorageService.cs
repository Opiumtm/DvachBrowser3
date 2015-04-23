using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище данных борды.
    /// </summary>
    public interface IBoardStorageService : IStorageBase
    {
        /// <summary>
        /// Сохранить.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Результат.</returns>
        Task Save(BoardPageTree data);

        /// <summary>
        /// Загрузить.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Данные.</returns>
        Task<BoardPageTree> Load(BoardLinkBase link);

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
        /// <returns>Данные.</returns>
        Task<BoardReferences> LoadBoardReferences(BoardLinkBase rootLink);

        /// <summary>
        /// Сохранить избранное.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SaveFavorites(LinkCollection data);

        /// <summary>
        /// Загрузить избранное.
        /// </summary>
        /// <returns>Данные.</returns>
        Task<LinkCollection> LoadFavorites();

        /// <summary>
        /// Сохранить последнее.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SaveLast(LinkCollection data);

        /// <summary>
        /// Загрузить последнее.
        /// </summary>
        /// <returns>Данные.</returns>
        Task<LinkCollection> LoadLast();
    }
}