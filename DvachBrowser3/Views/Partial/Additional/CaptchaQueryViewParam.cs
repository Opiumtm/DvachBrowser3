using DvachBrowser3.Posting;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Параметр запроса на капчу.
    /// </summary>
    public class CaptchaQueryViewParam
    {
        /// <summary>
        /// Тип капчи.
        /// </summary>
        public CaptchaType CaptchaType { get; set; }         

        /// <summary>
        /// Движок.
        /// </summary>
        public string Engine { get; set; }
    }
}