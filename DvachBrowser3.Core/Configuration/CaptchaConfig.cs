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
            get { return GetValue("YandexCaptchaUri", "http://i.captcha.yandex.net/"); }
            set
            {
                SetValue("YandexCaptchaUri", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        protected override ApplicationDataCompositeValue GetValue()
        {
            return ApplicationData.Current.RoamingSettings.Values.ContainsKey("Captcha")
                ? (ApplicationDataCompositeValue) ApplicationData.Current.RoamingSettings.Values["Captcha"]
                : new ApplicationDataCompositeValue();
        }

        /// <summary>
        /// Установить значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        protected override void SetValue(ApplicationDataCompositeValue value)
        {
            ApplicationData.Current.RoamingSettings.Values["Captcha"] = value;
        }
    }
}