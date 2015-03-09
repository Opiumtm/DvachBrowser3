using System;
using DvachBrowser3.Engines;

namespace DvachBrowser3.Captcha
{
    /// <summary>
    /// Сервис капчи.
    /// </summary>
    public sealed class CaptchaService : ServiceBase, ICaptchaService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public CaptchaService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Создать капчу.
        /// </summary>
        /// <param name="keys">Ключи.</param>
        /// <returns>Капча.</returns>
        public ICaptcha Create(CaptchaKeys keys)
        {
            if (keys is YandexCaptchaKeys)
            {
                return new YandexCaptcha(Services, keys as YandexCaptchaKeys);
            }
            if (keys is RecaptchaCaptchaKeys)
            {
                return new RecaptchaCaptcha(Services, keys as RecaptchaCaptchaKeys);
            }
            throw new InvalidOperationException(string.Format("Не поддерживаемый тип капчи {0}", keys.GetType().Name));
        }
    }
}