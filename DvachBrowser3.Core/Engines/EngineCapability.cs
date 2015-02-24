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
    }
}