using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Ключи капчи Yandex.
    /// </summary>
    public class RecaptchaCaptchaKeys : CaptchaKeys
    {
        /// <summary>
        /// Ключ 1.
        /// </summary>
        public string Key1 { get; set; }

        /// <summary>
        /// Ключ 2.
        /// </summary>
        public string Key2 { get; set; }

        /// <summary>
        /// Тип капчи.
        /// </summary>
        public override CaptchaType Kind
        {
            get { return CaptchaType.Recaptcha; }
        }
    }
}