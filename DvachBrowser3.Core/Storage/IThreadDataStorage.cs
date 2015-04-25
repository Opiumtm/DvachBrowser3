using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище тредов.
    /// </summary>
    public interface IThreadDataStorage : ICacheFolderInfo
    {
        /// <summary>
        /// Сохранить данные борды.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SaveBoardPage(BoardPageTree data);

        /// <summary>
        /// Загрузить данные борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Страница борды.</returns>
        Task<BoardPageTree> LoadBoardPage(BoardLinkBase link);

        /// <summary>
        /// Сохранить тред.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SaveThread(ThreadTree data);

        /// <summary>
        /// Загрузить тред.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Тред.</returns>
        Task<ThreadTree> LoadThread(BoardLinkBase link);

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
        Task LoadBoardReferences(BoardLinkBase rootLink);
    }
}