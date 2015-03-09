using System;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Captcha
{
    /// <summary>
    /// Капча Yandex.
    /// </summary>
    public sealed class YandexCaptcha : ICaptcha
    {
        private readonly IServiceProvider services;

        private readonly YandexCaptchaKeys keys;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="keys">Ключи.</param>
        public YandexCaptcha(IServiceProvider services, YandexCaptchaKeys keys)
        {
            this.services = services;
            this.keys = keys;
        }

        /// <summary>
        /// Тип.
        /// </summary>
        public CaptchaType Kind
        {
            get
            {
                return CaptchaType.Yandex;
            }
        }

        /// <summary>
        /// Подготовка данных капчи (после этого доступен ImageUri).
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task Prepare()
        {
            var config = services.GetServiceOrThrow<ICaptchaConfig>();
            ImageUri = new Uri(new Uri(config.YandexCaptchaUri, UriKind.Absolute), "image?key=" + keys.Key);
        }

        /// <summary>
        /// URI капчи.
        /// </summary>
        public Uri ImageUri { get; private set; }

        /// <summary>
        /// Проверить капчу перед постингом.
        /// </summary>
        /// <param name="entry">Введённая строка.</param>
        /// <returns>Таск.</returns>
        public async Task<CaptchaPostingData> Validate(string entry)
        {
            return new YandexCaptchaPostingData()
            {
                CaptchaEntry = entry,
                Key = keys.Key
            };
        }
    }
}