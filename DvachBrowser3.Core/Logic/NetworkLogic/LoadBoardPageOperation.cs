using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция загрузки страницы борды.
    /// </summary>
    public class LoadBoardPageOperation : NetworkLogicOperation<BoardPageTree, LoadBoardPageOperationParameter>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public LoadBoardPageOperation(IServiceProvider services, LoadBoardPageOperationParameter parameter)
            : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<BoardPageTree> Complete(CancellationToken token)
        {
            var engine = GetEngine(Parameter.Link);
            var storage = GetStorage();
            var linkTransformService = Services.GetServiceOrThrow<ILinkTransformService>();
            var threadProcessService = Services.GetServiceOrThrow<IThreadTreeProcessService>();
            var checkEtag = ((Parameter.Mode & UpdateBoardPageMode.CheckETag) != 0) && ((engine.Capability & EngineCapability.LastModifiedRequest) != 0);
            var saveToCache = (Parameter.Mode & UpdateBoardPageMode.SaveToCache) != 0;
            var boardLink = linkTransformService.BoardPageLinkFromBoardLink(Parameter.Link);
            if (boardLink == null)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            var oldData = await storage.ThreadData.LoadBoardPage(boardLink);
            string newEtag = null;
            if (checkEtag && oldData != null)
            {
                token.ThrowIfCancellationRequested();
                var etag = await storage.ThreadData.LoadStamp(boardLink);
                if (etag != null)
                {
                    var etagOperation = engine.GetResourceLastModified(boardLink);
                    SignalProcessing("Проверка обновлений...", "ETAG");
                    var newEtagObj = await etagOperation.Complete(token);
                    if (newEtagObj.LastModified != null)
                    {
                        newEtag = newEtagObj.LastModified;
                        if (etag == newEtag)
                        {
                            return oldData;
                        }
                    }
                }
            }
            if ((engine.Capability & EngineCapability.LastModifiedRequest) != 0 && newEtag == null)
            {
                var etagOperation = engine.GetResourceLastModified(boardLink);
                SignalProcessing("Проверка обновлений...", "ETAG"); 
                var newEtagObj = await etagOperation.Complete(token);
                if (newEtagObj != null)
                {
                    newEtag = newEtagObj.LastModified;
                }
            }
            BoardPageTree result;
            token.ThrowIfCancellationRequested();
            var threadOperation = engine.GetBoardPage(boardLink);
            threadOperation.Progress += (sender, e) => OnProgress(e);
            var page = await threadOperation.Complete(token);
            if (page == null || page.Result == null)
            {
                throw new InvalidOperationException("Не удалось получить тред");
            }
            result = page.Result;
            SignalProcessing("Обработка данных...", "PARSE");
            if (result.Threads == null)
            {
                result.Threads = new List<ThreadPreviewTree>();
            }
            foreach (var t in result.Threads)
            {
                threadProcessService.SetBackLinks(t);
                threadProcessService.SortThreadTree(t);                
            }
            if (saveToCache)
            {
                await storage.ThreadData.SaveBoardPage(result);
                if ((engine.Capability & EngineCapability.LastModifiedRequest) != 0)
                {
                    await storage.ThreadData.SaveStamp(boardLink, newEtag);
                }
            }
            return result;
        }
    }
}