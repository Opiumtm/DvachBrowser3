using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using DvachBrowser3.Engines.Makaba;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Configuration.Makaba
{
    /// <summary>
    /// Конфигурация движка Makaba.
    /// </summary>
    public class MakabaEngineConfig : AppDataConfigBase, IMakabaEngineConfig
    {
        /// <summary>
        /// Получить значение.
        /// </summary>
        /// <returns>Значение.</returns>
        protected override ApplicationDataCompositeValue GetValue()
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey("MakabaEngine")
                ? (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["MakabaEngine"]
                : new ApplicationDataCompositeValue();
        }

        /// <summary>
        /// Установить значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        protected override void SetValue(ApplicationDataCompositeValue value)
        {
            ApplicationData.Current.LocalSettings.Values["MakabaEngine"] = value;
        }

        /// <summary>
        /// Базовый URI.
        /// </summary>
        public Uri BaseUri
        {
            get
            {
                try
                {
                    var v = GetValue("BaseUri", "https://2ch.hk/");
                    if (string.IsNullOrWhiteSpace(v))
                    {
                        return new Uri("https://2ch.hk/", UriKind.Absolute);
                    }
                    return new Uri(v, UriKind.Absolute);
                }
                catch
                {
                    return new Uri("https://2ch.hk/", UriKind.Absolute);
                }
            }
            set
            {
                SetValue("BaseUri", value != null ? value.ToString() : "");
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Куки.
        /// </summary>
        /// <returns>Результат.</returns>
        public async Task<Dictionary<string, string>> GetCookies()
        {
            var composite = Storage.ContainsKey("EngineCookies")
                ? (ApplicationDataCompositeValue) Storage["EngineCookies"]
                : new ApplicationDataCompositeValue();
            return composite.ToDictionary(kv => kv.Key, kv => (string) kv.Value);
        }

        /// <summary>
        /// Установить куки.
        /// </summary>
        /// <param name="cookies">Куки.</param>
        /// <returns>Таск.</returns>
        public async Task SetCookies(Dictionary<string, string> cookies)
        {
            var composite = new ApplicationDataCompositeValue();
            if (cookies != null)
            {
                foreach (var kv in cookies)
                {
                    composite[kv.Key] = kv.Value;
                }
            }
            Storage["EngineCookies"] = composite;
        }

        /// <summary>
        /// Тип капчи.
        /// </summary>
        public CaptchaType CaptchaType
        {
            get { return (CaptchaType) GetValue("CaptchaType", (int) CaptchaType.Yandex); }
            set
            {
                SetValue("CaptchaType", (int)value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Агент браузера.
        /// </summary>
        public string BrowserUserAgent
        {
            get { return GetValue("BrowserUserAgent", ""); }
            set
            {
                SetValue("BrowserUserAgent", value ?? "");
                OnPropertyChanged();
            }
        }
    }
}