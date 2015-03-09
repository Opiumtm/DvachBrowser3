using DvachBrowser3.Engines;

namespace DvachBrowser3.Captcha
{
    /// <summary>
    /// Сервис работы с капчей.
    /// </summary>
    public interface ICaptchaService
    {
        /// <summary>
        /// Создать капчу.
        /// </summary>
        /// <param name="keys">Ключи.</param>
        /// <returns>Капча.</returns>
        ICaptcha Create(CaptchaKeys keys);
    }
}