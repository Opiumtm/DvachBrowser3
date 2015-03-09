using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Web.Http;
using DvachBrowser3.Engines;
using DvachBrowser3.Posting;
using HtmlAgilityPack;

namespace DvachBrowser3.Captcha
{
    /// <summary>
    /// Капча ReCaptcha.
    /// </summary>
    public sealed class RecaptchaCaptcha : ICaptcha
    {
        private readonly IServiceProvider services;

        private readonly RecaptchaCaptchaKeys keys;

        private string challenge;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="keys">Ключи.</param>
        public RecaptchaCaptcha(IServiceProvider services, RecaptchaCaptchaKeys keys)
        {
            this.services = services;
            this.keys = keys;
        }

        /// <summary>
        /// Тип.
        /// </summary>
        public CaptchaType Kind
        {
            get
            {
                return CaptchaType.Recaptcha;
            }
        }

        /// <summary>
        /// Подготовка данных капчи (после этого доступен ImageUri).
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task Prepare()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(new Uri("http://www.google.com/recaptcha/api/challenge?k=" + keys.Key, UriKind.Absolute));
                string responseStr = await response.Content.ReadAsStringAsync();

                var regex = services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(@"challenge\s\:\s\'.+\'");
                string captchure = regex.Match(responseStr).Captures[0].Value;
                challenge = captchure.Split(':')[1];
                challenge = challenge.Trim('\'', ' ');

                string imageUrl = "http://www.google.com/recaptcha/api2/payload?c=" + challenge;
                ImageUri = new Uri(imageUrl, UriKind.Absolute);
            }
        }

        /// <summary>
        /// URI капчи.
        /// </summary>
        public Uri ImageUri { get; private set; }

        private const string FallbackUri = "http://www.google.com/recaptcha/api/fallback";

        /// <summary>
        /// Проверить капчу перед постингом.
        /// </summary>
        /// <param name="entry">Введённая строка.</param>
        /// <returns>Таск.</returns>
        public async Task<CaptchaPostingData> Validate(string entry)
        {
            var uri = new Uri(FallbackUri + "?k=" + keys.Key + "&c=" + challenge + "&response=" + WebUtility.UrlEncode(entry), UriKind.Absolute);
            using (var client = new HttpClient())
            {
                var responseStr = await client.GetStringAsync(uri);
                var doc = new HtmlDocument();
                doc.LoadHtml(responseStr);
                var node = doc.DocumentNode.FlatHierarchy(h => h.ChildNodes).FirstOrDefault(h => h.GetAttributeValue("class", "*") == "fbc-verification-token");
                if (node == null)
                {
                    throw new WebException("Ошибка проверки капчи");
                }
                if (!node.HasChildNodes)
                {
                    throw new WebException("Ошибка проверки капчи");
                }
                var node1 = node.ChildNodes.FirstOrDefault(n => n.Name == "textarea");
                if (node1 == null)
                {
                    throw new WebException("Ошибка проверки капчи");
                }
                if (!node.HasChildNodes)
                {
                    throw new WebException("Ошибка проверки капчи");
                }
                var hash = node1.ChildNodes.OfType<HtmlTextNode>().FirstOrDefault();
                if (hash == null)
                {
                    throw new WebException("Ошибка проверки капчи");
                }
                return new RecaptchaCaptchaPostingData()
                {
                    CaptchaEntry = entry,
                    RecaptchaHash = hash.Text
                };
            }
        }
    }
}