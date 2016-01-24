using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Аргумент запроса на капчу.
    /// </summary>
    public struct MakabaGetCaptchaArgument
    {
        /// <summary>
        /// Тип капчи.
        /// </summary>
        public CaptchaType CaptchaType;

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link;
    }
}