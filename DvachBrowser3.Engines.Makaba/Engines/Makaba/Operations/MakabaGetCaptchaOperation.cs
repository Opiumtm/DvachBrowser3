using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Получить капчу.
    /// </summary>
    public sealed class MakabaGetCaptchaOperation : HttpGetEngineOperationBase<ICaptchaResult, MakabaGetCaptchaArgument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaGetCaptchaOperation(MakabaGetCaptchaArgument parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected override Uri GetRequestUri()
        {
            var isThread = (Parameter.Link.LinkKind & BoardLinkKind.Thread) != 0;
            var uri = Services.GetServiceOrThrow<IMakabaUriService>().GetCaptchaUri(Parameter.CaptchaType, isThread);
            if (uri == null)
            {
                throw new WebException("Неправильный тип капчи");
            }
            return uri;
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="token">Токен отмены.</param>
        /// <returns>Операция.</returns>
        protected override async Task<ICaptchaResult> DoComplete(HttpResponseMessage message, CancellationToken token)
        {
            message.EnsureSuccessStatusCode();
            var str = await message.Content.ReadAsStringAsync().AsTask(token);
            using (var rd = new StringReader(str))
            {
                var lines = (rd.ReadToEnd()).Split('\n');
                if (lines.Length > 0)
                {
                    if ("OK".Equals(lines[0], StringComparison.OrdinalIgnoreCase) || "VIP".Equals(lines[0], StringComparison.OrdinalIgnoreCase) || "Disabled".Equals(lines[0], StringComparison.OrdinalIgnoreCase))
                    {
                        return new OperationResult()
                        {
                            Keys = null,
                            NeedCaptcha = false
                        };
                    }
                    if (lines[0] == "CHECK" && lines.Length > 1)
                    {
                        /* Остальные типы капчи уже не поддерживаются сервером */
                        if (Parameter.CaptchaType == CaptchaType.DvachCaptcha)
                        {
                            return new OperationResult()
                            {
                                NeedCaptcha = true,
                                Keys = new DvachCaptchaKeys()
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
        /// <param name="filter">Фильтр.</param>
        /// <returns>Хидеры.</returns>
        protected override async Task SetHeaders(HttpClient client, IHttpFilter filter)
        {
            await base.SetHeaders(client, filter);
            await MakabaHeadersHelper.SetClientHeaders(Services, client, filter);
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