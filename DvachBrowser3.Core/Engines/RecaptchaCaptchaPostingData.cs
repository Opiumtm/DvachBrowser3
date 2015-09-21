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

    /// <summary>
    /// Данные рекапчи (второй вариант).
    /// </summary>
    public class RecaptchaV1CaptchaPostingData : CaptchaPostingData
    {
        /// <summary>
        /// Челлендж.
        /// </summary>
        public string Challenge { get; set; }
    }
}