using System;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим загрузки каталога.
    /// </summary>
    [Flags]
    public enum UpdateCatalogMode : int
    {
        /// <summary>
        /// Проверить штамп изменений.
        /// </summary>
        CheckETag = 0x0001,

        /// <summary>
        /// Сохранить в кэш.
        /// </summary>
        SaveToCache = 0x0002,

        /// <summary>
        /// По умолчанию.
        /// </summary>
        Default = CheckETag | SaveToCache
    }
}