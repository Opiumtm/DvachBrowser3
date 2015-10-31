using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines.Makaba
{
    /// <summary>
    /// Конфигурация движка Makaba.
    /// </summary>
    public interface IMakabaEngineConfig : IConfiguration
    {
        /// <summary>
        /// Базовый URI.
        /// </summary>
        Uri BaseUri { get; set; }

        /// <summary>
        /// Куки.
        /// </summary>
        /// <returns>Результат.</returns>
        Task<Dictionary<string, string>> GetCookies();

        /// <summary>
        /// Установить куки.
        /// </summary>
        /// <param name="cookies">Куки.</param>
        /// <returns>Таск.</returns>
        Task SetCookies(Dictionary<string, string> cookies);

        /// <summary>
        /// Тип капчи.
        /// </summary>
        CaptchaType CaptchaType { get; set; }

        /// <summary>
        /// Агент браузера.
        /// </summary>
        string BrowserUserAgent { get; set; }
    }
}