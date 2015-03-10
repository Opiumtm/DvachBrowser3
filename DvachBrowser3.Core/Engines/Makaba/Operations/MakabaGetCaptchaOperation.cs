using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Windows.Web.Http;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Получить капчу.
    /// </summary>
    public sealed class MakabaGetCaptchaOperation : HttpGetEngineOperationBase<ICaptchaResult, CaptchaType>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaGetCaptchaOperation(CaptchaType parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected override Uri GetRequestUri()
        {
            if (Parameter == CaptchaType.Yandex || Parameter == CaptchaType.Recaptcha)
            {
                return Services.GetServiceOrThrow<IMakabaUriService>().GetCaptchaUri(Parameter);                
            }
            throw new WebException("Неправильный тип капчи");
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Операция.</returns>
        protected override async Task<ICaptchaResult> DoComplete(HttpResponseMessage message)
        {
            message.EnsureSuccessStatusCode();
            var str = await message.Content.ReadAsStringAsync();
            using (var rd = new StringReader(str))
            {
                var lines = (rd.ReadToEnd()).Split('\n');
                if (lines.Length > 0)
                {
                    if (lines[0] == "OK" || lines[0] == "VIP")
                    {
                        return new OperationResult()
                        {
                            Keys = null,
                            NeedCaptcha = false
                        };
                    }
                    if (lines[0] == "CHECK" && lines.Length > 1)
                    {
                        if (Parameter == CaptchaType.Yandex)
                        {
                            return new OperationResult()
                            {
                                NeedCaptcha = true,
                                Keys = new YandexCaptchaKeys()
                                {
                                    Key = lines[1]
                                }
                            };
                        }
                        if (Parameter == CaptchaType.Recaptcha)
                        {
                            return new OperationResult()
                            {
                                NeedCaptcha = true,
                                Keys = new RecaptchaCaptchaKeys()
                                {
                                    Key = lines[1]
                                }
                            };                            
                        }
                        throw new WebException("Неправильный тип капчи");
                    }
                }
                throw new WebException("Неправильный ответ с сервера на запрос о капче");                
            }
        }

        /// <summary>
        /// Установить хидеры.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <returns>Хидеры.</returns>
        protected override async Task SetHeaders(HttpClient client)
        {
            await base.SetHeaders(client);
            await MakabaHeadersHelper.SetClientHeaders(Services, client);
        }

        /// <summary>
        /// Опция определения.
        /// </summary>
        protected override HttpCompletionOption CompletionOption
        {
            get
            {
                return HttpCompletionOption.ResponseContentRead;
            }
        }

        private class OperationResult : ICaptchaResult
        {
            public bool NeedCaptcha { get; set; }
            public CaptchaKeys Keys { get; set; }
        }
    }
}