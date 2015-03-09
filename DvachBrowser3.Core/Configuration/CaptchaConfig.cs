using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Captcha;

namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Конфигурация капчи.
    /// </summary>
    public sealed class CaptchaConfig : AppDataConfigBase, ICaptchaConfig
    {
        /// <summary>
        /// URI капчи яндекса.
        /// </summary>
        public string YandexCaptchaUri
        {
            get
            {
                return Storage.ContainsKey("YandexCaptchaUri") ? (string)Storage["YandexCaptchaUri"] : "http://i.captcha.yandex.net/";
            }
            set { Storage["YandexCaptchaUri"] = value; }
        }

        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        protected override ApplicationDataCompositeValue GetValue()
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey("Captcha")
                ? (ApplicationDataCompositeValue) ApplicationData.Current.LocalSettings.Values["Captcha"]
                : new ApplicationDataCompositeValue();
        }

        /// <summary>
        /// Установить значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        protected override void SetValue(ApplicationDataCompositeValue value)
        {
            ApplicationData.Current.LocalSettings.Values["Captcha"] = value;
        }
    }
}