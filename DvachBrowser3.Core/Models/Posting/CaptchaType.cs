namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Тип капчи.
    /// </summary>
    public enum CaptchaType : ulong
    {
        /// <summary>
        /// Яндекс.
        /// </summary>
        Yandex = 0x0001,

        /// <summary>
        /// Рекапча.
        /// </summary>
        Recaptcha = 0x0002,

        /// <summary>
        /// Реализация рекапчи v1 двача.
        /// </summary>
        GoogleRecaptcha2СhV1 = 0x0004,

        /// <summary>
        /// Реализация рекапчи v2 двача.
        /// </summary>
        GoogleRecaptcha2СhV2 = 0x0004,
    }
}