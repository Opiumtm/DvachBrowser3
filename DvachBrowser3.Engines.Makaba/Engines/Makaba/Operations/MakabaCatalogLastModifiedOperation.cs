using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// �������� ��������� ���������� ��������� ��������.
    /// </summary>
    public sealed class MakabaCatalogLastModifiedOperation : HttpEngineHeadJsonEngineOperationBase<ILastModifiedCheckResult, MakabaCatalogArgument>
    {
        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="parameter">��������.</param>
        /// <param name="services">�������.</param>
        public MakabaCatalogLastModifiedOperation(MakabaCatalogArgument parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        /// <summary>
        /// ������ URI �������.
        /// </summary>
        /// <returns>URI �������.</returns>
        protected override Uri GetRequestUri()
        {
            var uri = Services.GetServiceOrThrow<IMakabaUriService>().GetCatalogUri(Parameter.Link, Parameter.Sort);
            if (uri == null)
            {
                throw new ArgumentException("������������ ������ ������ (get catalog)");
            }
            return uri;
        }

        /// <summary>
        /// ��������� ��������.
        /// </summary>
        /// <param name="message">���������.</param>
        /// <param name="token">����� ������.</param>
        /// <returns>��������.</returns>
        protected async override Task<ILastModifiedCheckResult> DoComplete(HttpResponseMessage message, CancellationToken token)
        {
            if (message.Headers.ContainsKey("ETag"))
            {
                return new OperationResult() { LastModified = message.Headers["ETag"] };
            }
            return new OperationResult() { LastModified = null };
        }

        /// <summary>
        /// ���������� ������.
        /// </summary>
        /// <param name="client">������.</param>
        /// <param name="filter">������.</param>
        /// <returns>������.</returns>
        protected override async Task SetHeaders(HttpClient client, IHttpFilter filter)
        {
            await base.SetHeaders(client, filter);
            await MakabaHeadersHelper.SetClientHeaders(Services, client, filter);
        }

        private class OperationResult : ILastModifiedCheckResult
        {
            public string LastModified { get; set; }
        }
    }
}