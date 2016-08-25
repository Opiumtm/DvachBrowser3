using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using DvachBrowser3.ApiKeys;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// �������� �������� ����������� �������� ��� �����.
    /// </summary>
    public sealed class MakabaCheckNoCaptchaAbilityV2Operation : HttpGetJsonEngineOperationBase<INoCaptchaCheckResult, BoardLinkBase, CaptchaV2IdResult>
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="parameter">��������.</param>
        /// <param name="services">�������.</param>
        public MakabaCheckNoCaptchaAbilityV2Operation(BoardLinkBase parameter, IServiceProvider services) : base(parameter, services)
        {
            appId = new Lazy<string>(GetAppId);
        }

        /// <summary>
        /// �������� ID ����������.
        /// </summary>
        /// <returns>ID ����������.</returns>
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
        /// ���������� ������.
        /// </summary>
        /// <param name="client">������.</param>
        /// <param name="filter"></param>
        /// <returns>������.</returns>
        protected override async Task SetHeaders(HttpClient client, IHttpFilter filter)
        {
            await base.SetHeaders(client, filter);
            await MakabaHeadersHelper.SetClientHeaders(Services, client, filter);
        }

        /// <summary>
        /// �������� ���������.
        /// </summary>
        /// <param name="result">���������.</param>
        /// <returns>true, ���� ������ ������ �� �����.</returns>
        protected override bool ShortComplete(out INoCaptchaCheckResult result)
        {
            if (appId.Value == null)
            {
                result = new Result(NoCaptchaAbility.NoApiKeys);
                return true;
            }
            if (!(Parameter is RootLink || (Parameter.LinkKind & BoardLinkKind.Thread) != 0 || (Parameter.LinkKind & BoardLinkKind.PartialThread) != 0 || (Parameter.LinkKind & BoardLinkKind.Post) != 0))
            {
                result = new Result(NoCaptchaAbility.ForbiddenForLink);
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>
        /// ������ URI �������.
        /// </summary>
        /// <returns>URI �������.</returns>
        protected override Uri GetRequestUri()
        {
            return Services.GetServiceOrThrow<IMakabaUriService>().GetNocaptchaUri(true, appId.Value ?? "xxx");
        }

        /// <summary>
        /// ���������� ��������� ��� ������.
        /// </summary>
        protected override bool SetResultOnError => true;

        /// <summary>
        /// ��������� �� ������.
        /// </summary>
        /// <param name="error">������.</param>
        /// <returns>���������.</returns>
        protected override INoCaptchaCheckResult OnError(Exception error)
        {
            return new Result(NoCaptchaAbility.TryAgain);
        }

        protected override async Task<INoCaptchaCheckResult> ProcessJson(CaptchaV2IdResult message, string etag, CancellationToken token)
        {
            switch (message.Result)
            {
                // 1 - ������ ����������� �������.
                case 1:
                    return new Result(NoCaptchaAbility.Ok);
                // 0 - ��� ���������� ������� �������� ������. ��� ������ ��������� � ���������� error, �������� ������ � ���������� description.
                case 0:
                    return new Result(NoCaptchaAbility.InvalidKeys);
                default:
                    return new Result(NoCaptchaAbility.TryAgain);
            }

        }

        private readonly Lazy<string> appId;

        /// <summary>
        /// ��������� ���������� ��������.
        /// </summary>
        private class Result : INoCaptchaCheckResult
        {
            public Result(NoCaptchaAbility ability)
            {
                Ability = ability;
            }

            /// <summary>
            /// ����� ������� ��� �����.
            /// </summary>
            public bool CanPost => Ability == NoCaptchaAbility.Ok;

            /// <summary>
            /// �����������.
            /// </summary>
            public NoCaptchaAbility Ability { get; private set; }
        }
    }
}