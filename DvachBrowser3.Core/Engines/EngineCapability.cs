using System;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Возможности движка.
    /// </summary>
    [Flags]
    public enum EngineCapability : int
    {
        /// <summary>
        /// Запрос на часть треда.
        /// </summary>
        PartialThreadRequest = 0x0001,

        /// <summary>
        /// Запрос к статусу треда.
        /// </summary>
        ThreadStatusRequest = 0x0002,

        /// <summary>
        /// Запрос к списку борд.
        /// </summary>
        BoardsListRequest = 0x0004,

        /// <summary>
        /// Поиск по борде.
        /// </summary>
        SearchRequest = 0x0008,

        /// <summary>
        /// Каталог топ постов.
        /// </summary>
        TopPostsRequest = 0x0010,

        /// <summary>
        /// Запрос на последнее изменение (last modified header).
        /// </summary>
        LastModifiedRequest = 0x0020,
    }
}