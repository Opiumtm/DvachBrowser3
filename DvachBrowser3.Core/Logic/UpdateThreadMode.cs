using System;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим обновления треда.
    /// </summary>
    [Flags]
    public enum UpdateThreadMode : int
    {
        /// <summary>
        /// Проверить штамп изменений.
        /// </summary>
        CheckETag = 0x0001,

        /// <summary>
        /// Частичная загрузка.
        /// </summary>
        Partial = 0x0002,

        /// <summary>
        /// По умолчанию.
        /// </summary>
        Default = CheckETag | Partial
    }
}