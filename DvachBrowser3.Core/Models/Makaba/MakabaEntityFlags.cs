using System;

namespace DvachBrowser3.Makaba
{
    /// <summary>
    /// Флаги данных макабы.
    /// </summary>
    [Flags]
    public enum MakabaEntityFlags : int
    {
        /// <summary>
        /// Разрешать аудио.
        /// </summary>
        EnableAudio = 0x0001,

        /// <summary>
        /// Разрешать кубики (?).
        /// </summary>
        EnableDices = 0x0002,

        /// <summary>
        /// Разрешать иконки.
        /// </summary>
        EnableIcons = 0x0004,

        /// <summary>
        /// Разрешать изображения.
        /// </summary>
        EnableImages = 0x0008,

        /// <summary>
        /// Разрешать имена.
        /// </summary>
        EnableNames = 0x0010,

        /// <summary>
        /// Разрешать сажу.
        /// </summary>
        EnableSage = 0x0020,

        /// <summary>
        /// Разрешать трипкоды.
        /// </summary>
        EnableTrips = 0x0040,

        /// <summary>
        /// Разрешать видео.
        /// </summary>
        EnableVideo = 0x0080,

        /// <summary>
        /// Борда.
        /// </summary>
        IsBoard = 0x0100,
    }
}