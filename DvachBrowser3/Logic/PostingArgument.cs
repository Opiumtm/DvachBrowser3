using DvachBrowser3.Engines;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Аргумент операции постинга.
    /// </summary>
    public struct PostingArgument
    {
        /// <summary>
        /// Данные.
        /// </summary>
        public PostingData Data;

        /// <summary>
        /// Капча.
        /// </summary>
        public CaptchaPostingData Captcha;

        /// <summary>
        /// Тип капчи.
        /// </summary>
        public CaptchaType? CaptchaType;
    }
}