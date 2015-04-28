using System;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим страницы борды.
    /// </summary>
    [Flags]
    public enum UpdateBoardPageMode : int
    {
        /// <summary>
        /// Проверить штамп изменений.
        /// </summary>
        CheckETag = 0x0001,

        /// <summary>
        /// По умолчанию.
        /// </summary>
        Default = CheckETag
    }
}