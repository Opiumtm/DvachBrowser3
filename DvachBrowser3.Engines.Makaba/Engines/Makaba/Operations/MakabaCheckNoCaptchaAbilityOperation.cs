using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using DvachBrowser3.ApiKeys;
using DvachBrowser3.Links;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Операция проверки возможности постинга без капчи.
    /// </summary>
    public sealed class MakabaCheckNoCaptchaAbilityOperation : HttpGetEngineOperationBase<INoCaptchaCheckResult, BoardLinkBase>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaCheckNoCaptchaAbilityOperation(BoardLinkBase parameter, IServiceProvider services) : base(parameter, services)
        {
            appId = new Lazy<string>(GetAppId);
        }

        /// <summary>
        /// Получить ID приложения.
        /// </summary>
        /// <returns>ID приложения.</returns>
        private string GetAppId()
        {
            var apiKeys = Services.GetServiceOrThrow<IApiKeyService>();
            var container = apiKeys.Find(CoreConstants.ApiKeys.Containers.MakabaPosting);
            var keys = container?.GetKeys();
            if (keys == null)
            {
                return null;
            }
            if (!keys.ContainsKey(CoreConstants.ApiKeys.ApplicationId))
            {
                return null;
            }
            return keys[CoreConstants.ApiKeys.ApplicationId].Get();
        }

        /// <summary>
        /// Установить хидеры.
        /// </summary>
        /// <param name="client">Клиент.</param>
        /// <param name="filter"></param>
        /// <returns>Хидеры.</returns>
        protected override async Task SetHeaders(HttpClient client, IHttpFilter filter)
        {
            await base.SetHeaders(client, filter);
            await MakabaHeadersHelper.SetClientHeaders(Services, client, filter);
        }

        /// <summary>
        /// Короткий результат.
        /// </summary>
        /// <param name="result">Результат.</param>
        /// <returns>true, если запрос делать не нужно.</returns>
        protected override bool ShortComplete(out INoCaptchaCheckResult result)
        {
            if (appId.Value == null)
            {
                result = new Result(NoCaptchaAbility.NoApiKeys);
                return true;
            }
            if (!(Parameter is RootLink || Parameter.LinkKind == BoardLinkKind.Thread || Parameter.LinkKind == BoardLinkKind.PartialThread || Parameter.LinkKind == BoardLinkKind.Post))
            {
                result = new Result(NoCaptchaAbility.ForbiddenForLink);
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected override Uri GetRequestUri()
        {
            return Services.GetServiceOrThrow<IMakabaUriService>().GetNocaptchaUri(true, appId.Value ?? "xxx");
        }

        /// <summary>
        /// Установить результат для ошибки.
        /// </summary>
        protected override bool SetResultOnError => true;

        /// <summary>
        /// Результат по ошибке.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns>Результат.</returns>
        protected override INoCaptchaCheckResult OnError(Exception error)
        {
            return new Result(NoCaptchaAbility.TryAgain);
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены.</param>
        /// <param name="message">Сообщение.</param>
        /// <returns>Операция.</returns>
        protected override async Task<INoCaptchaCheckResult> DoComplete(HttpResponseMessage message, CancellationToken token)
        {
            message.EnsureSuccessStatusCode();
            var str = await message.Content.ReadAsStringAsync().AsTask(token);
            var l = new List<string>();
            using (var rd = new StringReader(str))
            {
                string s = null;
                do
                {
                    s = rd.ReadLine();
                    if (s != null)
                    {
                        l.Add(s);
                    }
                } while (s != null);
            }
            if (l.Count == 0)
            {
                return new Result(NoCaptchaAbility.TryAgain);
            }
            if ("APP VALID".Equals(l[0].Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return new Result(NoCaptchaAbility.Ok);
            }
            return new Result(NoCaptchaAbility.InvalidKeys);
        }

        /// <summary>
        /// Опция определения.
        /// </summary>
        protected override HttpCompletionOption CompletionOption => HttpCompletionOption.ResponseContentRead;

        private readonly Lazy<string> appId;

        /// <summary>
        /// Результат выполнения операции.
        /// </summary>
        private class Result : INoCaptchaCheckResult
        {
            public Result(NoCaptchaAbility ability)
            {
                Ability = ability;
            }

            /// <summary>
            /// Можно постить без капчи.
            /// </summary>
            public bool CanPost => Ability == NoCaptchaAbility.Ok;

            /// <summary>
            /// Возможность.
            /// </summary>
            public NoCaptchaAbility Ability { get; private set; }
        }
    }
}