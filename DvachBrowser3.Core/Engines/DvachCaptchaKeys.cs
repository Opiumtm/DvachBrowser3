using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Капча двача.
    /// </summary>
    public class DvachCaptchaKeys : CaptchaKeys
    {
        /// <summary>
        /// Ключ капчи.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Тип капчи.
        /// </summary>
        public override CaptchaType Kind  => CaptchaType.DvachCaptcha;
    }
}