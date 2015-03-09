namespace DvachBrowser3.Captcha
{
    /// <summary>
    /// Конфигурация капчи.
    /// </summary>
    public interface ICaptchaConfig : IConfiguration
    {
        /// <summary>
        /// URI капчи яндекса.
        /// </summary>
        string YandexCaptchaUri { get; set; } 
    }
}