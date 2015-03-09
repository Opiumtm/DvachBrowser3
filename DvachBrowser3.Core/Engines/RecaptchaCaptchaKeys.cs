using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Ключи капчи Yandex.
    /// </summary>
    public class RecaptchaCaptchaKeys : CaptchaKeys
    {
        /// <summary>
        /// Ключ.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Тип капчи.
        /// </summary>
        public override CaptchaType Kind
        {
            get { return CaptchaType.Recaptcha; }
        }
    }
}