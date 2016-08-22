using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Получить капчу.
    /// </summary>
    public sealed class MakabaGetCaptchaOperationV2 : HttpGetJsonEngineOperationBase<ICaptchaResult, MakabaGetCaptchaArgument, CaptchaV2IdResult>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaGetCaptchaOperationV2(MakabaGetCaptchaArgument parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected override Uri GetRequestUri()
        {
            string board;
            int? thread;
            if (Parameter.Link is ThreadLink)
            {
                board = ((ThreadLink) Parameter.Link).Board;
                thread = ((ThreadLink) Parameter.Link).Thread;
            } else if (Parameter.Link is PostLink)
            {
                board = ((PostLink)Parameter.Link).Board;
                thread = ((PostLink)Parameter.Link).Thread;
            } else if (Parameter.Link is BoardLink)
            {
                board = ((BoardLink)Parameter.Link).Board;
                thread = null;
            } else if (Parameter.Link is BoardPageLink)
            {
                board = ((BoardPageLink)Parameter.Link).Board;
                thread = null;
            }
            else
            {
                throw new InvalidOperationException("Неправильный тип ссылки");
            }
            var uri = Services.GetServiceOrThrow<IMakabaUriService>().GetCaptchaUriV2(Parameter.CaptchaType, board, thread);
            if (uri == null)
            {
                throw new WebException("Неправильный тип капчи");
            }
            return uri;
        }

        protected override async Task<ICaptchaResult> ProcessJson(CaptchaV2IdResult message, string etag, CancellationToken token)
        {
            switch (message.Result)
            {
                // 3 - Капча не требуется. Например, в случае если на доске она отключена.
                case 3:
                    return new OperationResult() {NeedCaptcha = false, Keys = null};
                // 2 - Капча не требуется, поскольку активен VIP аккаунт.
                case 2:
                    return new OperationResult() {NeedCaptcha = false, Keys = null};
                // 1 - Запрос удовлетворён успешно.
                case 1:
                    if (message.Id == null)
                    {
                        throw new WebException("Неправильный ответ с сервера (captcha.id = null)");
                    }
                    return new OperationResult()
                    {
                        NeedCaptcha = true,
                        Keys = new DvachCaptchaKeys()
                        {
                            Key = message.Id
                        }
                    };
                // 0 - При выполнении запроса возникла ошибка. Код ошибки находится в переменной error, описание ошибки в переменной description.
                case 0:
                    throw new WebException($"Ошибка получения капчи {message.Error}: {message.ErrorDescription}");
                default:
                    throw new WebException("Неправильный ответ с сервера (captcha.result invalid)");
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

        private class OperationResult : ICaptchaResult
        {
            public bool NeedCaptcha { get; set; }
            public CaptchaKeys Keys { get; set; }
        }
    }
}