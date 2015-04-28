using DvachBrowser3.Engines;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Параметр операции постинга.
    /// </summary>
    public struct PostOperationParameter
    {
        /// <summary>
        /// Данные постинга.
        /// </summary>
        public PostingData Data;

        /// <summary>
        /// Капча.
        /// </summary>
        public CaptchaPostingData Captcha;

        /// <summary>
        /// Режим.
        /// </summary>
        public PostingMode Mode;
    }
}