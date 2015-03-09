using System;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Captcha
{
    /// <summary>
    /// Капча.
    /// </summary>
    public interface ICaptcha
    {
        /// <summary>
        /// Тип.
        /// </summary>
        CaptchaType Kind { get; }

        /// <summary>
        /// Подготовка данных капчи (после этого доступен ImageUri).
        /// </summary>
        /// <returns>Таск.</returns>
        Task Prepare();

        /// <summary>
        /// URI капчи.
        /// </summary>
        Uri ImageUri { get; }

        /// <summary>
        /// Проверить капчу перед постингом.
        /// </summary>
        /// <param name="entry">Введённая строка.</param>
        /// <returns>Таск.</returns>
        Task<CaptchaPostingData> Validate(string entry);
    }
}