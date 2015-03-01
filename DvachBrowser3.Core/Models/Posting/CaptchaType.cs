namespace DvachBrowser3.Posting
{
    /// <summary>
    /// Тип капчи.
    /// </summary>
    public enum CaptchaType : int
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