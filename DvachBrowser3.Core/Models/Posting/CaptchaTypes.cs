using System;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Типы капчи.
    /// </summary>
    [Flags]
    public enum CaptchaTypes : int
    {
        /// <summary>
        /// Яндекс.
        /// </summary>
        Yandex = 0x0001,

        /// <summary>
        /// Рекапча.
        /// </summary>
        Recaptcha = 0x0002,
    }
}