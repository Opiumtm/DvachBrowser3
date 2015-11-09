using System.Collections.Generic;
using System.Threading.Tasks;

namespace DvachBrowser3.Posts
{
    /// <summary>
    /// Источник списка постов.
    /// </summary>
    public interface IPostTreeListSource
    {
        /// <summary>
        /// Получить посты.
        /// </summary>
        /// <returns>Посты.</returns>
        Task<IList<PostTree>> GetPosts();
    }
}