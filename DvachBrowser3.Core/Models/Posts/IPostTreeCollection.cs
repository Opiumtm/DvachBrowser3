using System.Collections.Generic;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Коллекция постов.
    /// </summary>
    public interface IPostTreeCollection
    {
        /// <summary>
        /// Режим коллекции.
        /// </summary>
        PostTreeCollectionMode CollectionMode { get; }

        /// <summary>
        /// Получить посты (только для режима Internal).
        /// </summary>
        /// <returns>Посты.</returns>
        List<PostTree> GetInternalPosts();
    }
}