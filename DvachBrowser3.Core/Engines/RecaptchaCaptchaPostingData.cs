namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Данные рекапчи.
    /// </summary>
    public class RecaptchaCaptchaPostingData : CaptchaPostingData
    {
        /// <summary>
        /// Хэш.
        /// </summary>
        public string RecaptchaHash { get; set; }
    }
}