using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Хранилище тредов.
    /// </summary>
    public interface IThreadDataStorage : ICacheFolderInfo
    {
        /// <summary>
        /// Избранные треды.
        /// </summary>
        ILinkCollectionStore FavoriteThreads { get; }

        /// <summary>
        /// Посещённые треды.
        /// </summary>
        ILinkCollectionStore VisitedThreads { get; }

        /// <summary>
        /// Избранные борды.
        /// </summary>
        ILinkCollectionStore FavoriteBoards { get; }

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
        Task<BoardReferences> LoadBoardReferences(BoardLinkBase rootLink);

        /// <summary>
        /// Сохранить информацию о количестве постов.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SavePostCountInfo(PostCountInfo data);

        /// <summary>
        /// Загрузить информацию о количестве постов.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Таск.</returns>
        Task<PostCountInfo> LoadPostCountInfo(BoardLinkBase link);

        /// <summary>
        /// Сохранить информацию о моих постах.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SaveMyPostsInfo(MyPostsInfo data);

        /// <summary>
        /// Загрузить информацию о моих постах.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Информация о моих постах.</returns>
        Task<MyPostsInfo> LoadMyPostsInfo(BoardLinkBase link);

        /// <summary>
        /// Сохранить штамп изменений.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="stamp">Штамп.</param>
        /// <returns>Таск.</returns>
        Task SaveStamp(BoardLinkBase link, string stamp);

        /// <summary>
        /// Загрузить штамп.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Штамп.</returns>
        Task<string> LoadStamp(BoardLinkBase link);

        /// <summary>
        /// Сохранить каталог.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Каталог.</returns>
        Task SaveCatalog(CatalogTree data);

        /// <summary>
        /// Загрузить каталог.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="sortMode">Режим сортировки.</param>
        /// <returns>Каталог.</returns>
        Task<CatalogTree> LoadCatalog(BoardLinkBase link, CatalogSortMode sortMode);

            /// <summary>
        /// Сохранить штамп изменений.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="sortMode">Режим сортировки.</param>
        /// <param name="stamp">Штамп.</param>
        /// <returns>Таск.</returns>
        Task SaveCatalogStamp(BoardLinkBase link, CatalogSortMode sortMode, string stamp);

        /// <summary>
        /// Загрузить штамп.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="sortMode">Режим сортировки.</param>
        /// <returns>Штамп.</returns>
        Task<string> LoadCatalogStamp(BoardLinkBase link, CatalogSortMode sortMode);
    }
}