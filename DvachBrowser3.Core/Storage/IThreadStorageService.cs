using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Links;
using DvachBrowser3.Other;
using DvachBrowser3.Posting;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Storage
{
    /// <summary>
    /// Сервис хранения данных треда.
    /// </summary>
    public interface IThreadStorageService : IStorageBase
    {
        /// <summary>
        /// Сохранить.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task Save(ThreadTree data);

        /// <summary>
        /// Загрузить.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Данные.</returns>
        Task<ThreadTree> Load(BoardLinkBase link);

        /// <summary>
        /// Сохранить количество постов.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SavePostCount(PostCountInfo data);

        /// <summary>
        /// Загрузить информцию о постах.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Информация о постах.</returns>
        Task<PostCountInfo> LoadPostCount(BoardLinkBase link);

        /// <summary>
        /// Установить последнее изменение.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="lastModified">Штамп.</param>
        /// <returns>Таск.</returns>
        Task SetLastModified(BoardLinkBase link, string lastModified);

        /// <summary>
        /// Взять последнее изменение.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Штамп.</returns>
        Task<string> GetLastModified(BoardLinkBase link);

        /// <summary>
        /// Сохранить информацию о моих постах.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Таск.</returns>
        Task SaveMyPosts(MyPostsInfo data);

        /// <summary>
        /// Загрузить информацию о моих постах.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Данные.</returns>
        Task<MyPostsInfo> LoadMyPosts(BoardLinkBase link);

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