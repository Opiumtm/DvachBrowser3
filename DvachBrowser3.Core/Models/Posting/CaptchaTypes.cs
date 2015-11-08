using System;

namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Типы капчи.
    /// </summary>
    [Flags]
    public enum CaptchaTypes : ulong
    {
        /// <summary>
        /// Яндекс.
        /// </summary>
        Yandex = CaptchaType.Yandex,

        /// <summary>
        /// Рекапча.
        /// </summary>
        Recaptcha = CaptchaType.Recaptcha,

        /// <summary>
        /// Реализация рекапчи v1 двача.
        /// </summary>
        GoogleRecaptcha2СhV1 = CaptchaType.GoogleRecaptcha2СhV1,

        /// <summary>
        /// Реализация рекапчи v2 двача.
        /// </summary>
        GoogleRecaptcha2СhV2 = CaptchaType.GoogleRecaptcha2СhV2,

        /// <summary>
        /// Двач капча.
        /// </summary>
        DvachCaptcha = CaptchaType.DvachCaptcha
    }
}