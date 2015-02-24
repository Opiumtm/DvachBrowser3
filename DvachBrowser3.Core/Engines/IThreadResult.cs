using System.Collections.Generic;
using System.Threading.Tasks;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Результат запроса треда.
    /// </summary>
    public interface IThreadResult
    {
        /// <summary>
        /// Результат.
        /// </summary>
        PostTreeCollection CollectionResult { get; }

        /// <summary>
        /// Частичный результат.
        /// </summary>
        bool IsPartial { get; }

        /// <summary>
        /// Получить посты.
        /// </summary>
        /// <param name="rewind">Начать сначала.</param>
        /// <param name="maxPosts">Максимальное количество постов.</param>
        /// <returns>Посты.</returns>
        Task<PostTreeContainer> ListExternalPosts(bool rewind, int maxPosts = CoreConstants.PostContainerMaxCount);
    }
}