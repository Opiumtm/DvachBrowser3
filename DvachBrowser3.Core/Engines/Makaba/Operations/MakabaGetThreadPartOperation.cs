using System;
using System.Net;
using System.Threading.Tasks;
using Windows.Web.Http;
using DvachBrowser3.Engines.Makaba.Html;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;
using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Operations
{
    /// <summary>
    /// Получить часть треда.
    /// </summary>
    public sealed class MakabaGetThreadPartOperation : HttpGetEngineOperationBase<IThreadResult, BoardLinkBase>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        public MakabaGetThreadPartOperation(BoardLinkBase parameter, IServiceProvider services) : base(parameter, services)
        {
        }

        private ThreadPartLink GetThreadLink()
        {
            if (Parameter is ThreadPartLink)
            {
                var l = Parameter as ThreadPartLink;
                return new ThreadPartLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = l.Board,
                    Thread = l.Thread,
                    FromPost = l.FromPost
                };
            }
            throw new ArgumentException("Неправильный формат ссылки (get thread part)");
        }

        /// <summary>
        /// Получи URI запроса.
        /// </summary>
        /// <returns>URI запроса.</returns>
        protected override Uri GetRequestUri()
        {
            return Services.GetServiceOrThrow<IMakabaUriService>().GetJsonLink(GetThreadLink());
        }

        protected override async Task<IThreadResult> DoComplete(HttpResponseMessage message)
        {
            message.EnsureSuccessStatusCode();
            var str = await message.Content.ReadAsStringAsync();
            Operation = null;
            SignalProcessing();
            if (str.StartsWith("["))
            {
                var result = JsonConvert.DeserializeObject<BoardPost2[]>(str);
                var task = Task<IThreadResult>.Factory.StartNew(() =>
                {
                    var data = new OperationResult()
                    {
                        CollectionResult = Services.GetServiceOrThrow<IMakabaJsonResponseParseService>().ParseThreadPartial(result, GetThreadLink())
                    };
                    return data;
                });
                return await task;                
            }
            else
            {
                var errorObj = JsonConvert.DeserializeObject<ThreadPartialError>(str);
                throw new WebException(string.Format("{0}: {1}", errorObj.Code, errorObj.Error));
            }
        }

        private class OperationResult : IThreadResult
        {
            public PostTreeCollection CollectionResult { get; set; }
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

        protected override HttpCompletionOption CompletionOption
        {
            get
            {
                return HttpCompletionOption.ResponseContentRead;
            }
        }
    }
}