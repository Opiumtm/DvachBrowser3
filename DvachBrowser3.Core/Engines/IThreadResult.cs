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
    }
}