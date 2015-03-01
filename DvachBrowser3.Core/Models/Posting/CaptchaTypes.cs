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
        Yandex = CaptchaType.Yandex,

        /// <summary>
        /// Рекапча.
        /// </summary>
        Recaptcha = CaptchaType.Recaptcha,
    }
}