using System;
using DvachBrowser3.Links;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Краткий статус треда.
    /// </summary>
    public interface IThreadStatusResult
    {
        /// <summary>
        /// Всего постов.
        /// </summary>
        int? TotalPosts { get; }

        /// <summary>
        /// Последнее обновление.
        /// </summary>
        DateTime LastUpdate { get; }

        /// <summary>
        /// Идентификатор последней модификации.
        /// </summary>
        string LastModifiedId { get; }

        /// <summary>
        /// Тред существует.
        /// </summary>
        bool IsFound { get; }
    }
}