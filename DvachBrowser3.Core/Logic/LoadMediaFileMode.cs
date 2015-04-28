using System;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Режим загрузки медиафайла.
    /// </summary>
    [Flags]
    public enum LoadMediaFileMode : int
    {
        /// <summary>
        /// Полноразмерный медиафайл.
        /// </summary>
        FullSizeMedia = 0x0001,

        /// <summary>
        /// Сохранить в кэш.
        /// </summary>
        SaveToCache = 0x0002,
        
        /// <summary>
        /// По умолчанию для маленького размера.
        /// </summary>
        DefaultSmallSize = SaveToCache,

        /// <summary>
        /// По умолчанию для большого размера.
        /// </summary>
        DefaultFullSize = SaveToCache | FullSizeMedia
    }
}