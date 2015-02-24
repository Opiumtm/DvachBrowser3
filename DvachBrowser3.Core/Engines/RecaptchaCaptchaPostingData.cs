namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Данные рекапчи.
    /// </summary>
    public class RecaptchaCaptchaPostingData : CaptchaPostingData
    {
        /// <summary>
        /// Ключ 1.
        /// </summary>
        public string Key1 { get; set; }

        /// <summary>
        /// Ключ 2.
        /// </summary>
        public string Key2 { get; set; }
    }
}