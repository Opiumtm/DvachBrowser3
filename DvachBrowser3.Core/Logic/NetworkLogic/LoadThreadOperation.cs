using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Other;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция загрузки треда.
    /// </summary>
    public class LoadThreadOperation : NetworkLogicOperation<ThreadTree, LoadThreadOperationParameter>
    {
        public LoadThreadOperation(IServiceProvider services, LoadThreadOperationParameter parameter) : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<ThreadTree> Complete(CancellationToken token)
        {
            var engine = GetEngine(Parameter.Link);
            var storage = GetStorage();
            var linkTransformService = Services.GetServiceOrThrow<ILinkTransformService>();
            var threadProcessService = Services.GetServiceOrThrow<IThreadTreeProcessService>();
            var linkHashService = Services.GetServiceOrThrow<ILinkHashService>();
            var checkEtag = ((Parameter.Mode & UpdateThreadMode.CheckETag) != 0) && ((engine.Capability & EngineCapability.LastModifiedRequest) != 0);
            var partial = ((Parameter.Mode & UpdateThreadMode.Partial) != 0) && ((engine.Capability & EngineCapability.PartialThreadRequest) != 0);
            var updateVisit = (Parameter.Mode & UpdateThreadMode.UpdateVisitData) != 0;
            var saveToCache = (Parameter.Mode & UpdateThreadMode.SaveToCache) != 0;
            var threadLink = linkTransformService.ThreadLinkFromThreadPartLink(Parameter.Link);
            if (threadLink == null)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            var oldData = await storage.ThreadData.LoadThread(threadLink);
            string newEtag = null;
            if (checkEtag && oldData != null)
            {
                token.ThrowIfCancellationRequested();
                var etag = await storage.ThreadData.LoadStamp(threadLink);
                if (etag != null)
                {
                    var etagOperation = engine.GetResourceLastModified(threadLink);
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
                var etagOperation = engine.GetResourceLastModified(threadLink);
                etagOperation.Progress += (sender, e) => OnProgress(e);
                var newEtagObj = await etagOperation.Complete(token);
                if (newEtagObj != null)
                {
                    newEtag = newEtagObj.LastModified;
                }
            }
            if ((engine.Capability & EngineCapability.LastModifiedRequest) != 0 && saveToCache)
            {
                await storage.ThreadData.SaveStamp(threadLink, newEtag);                
            }
            ThreadTree result = null;
            int? gotPostCount = null;
            DateTime? gotUpdateDate = null;
            BoardLinkBase partialLink = null;
            if (oldData != null && oldData.Posts != null && partial)
            {
                var lastPost = oldData.Posts.LastOrDefault();
                if (lastPost != null && lastPost.Link != null)
                {
                    partialLink = linkTransformService.ThreadPartLinkFromThreadLink(threadLink, lastPost.Link);
                }
            }
            if (partial && partialLink != null)
            {
                if ((engine.Capability & EngineCapability.ThreadStatusRequest) != 0)
                {
                    token.ThrowIfCancellationRequested();
                    var statusOperation = engine.GetThreadStatus(threadLink);
                    SignalProcessing("Получение количества постов...", "THREAD STATUS");
                    var status = await statusOperation.Complete(token);
                    if (status != null)
                    {
                        if (status.IsFound)
                        {
                            gotPostCount = status.TotalPosts;
                            gotUpdateDate = status.LastUpdate;
                        }
                    }                    
                }
                token.ThrowIfCancellationRequested();
                var partialOperation = engine.GetThread(partialLink);
                partialOperation.Progress += (sender, e) => OnProgress(e);
                var partialThread = await partialOperation.Complete(token);
                if (partialThread != null && partialThread.CollectionResult != null)
                {
                    if (partialThread.CollectionResult is ThreadTreePartial)
                    {
                        result = oldData;
                        threadProcessService.MergeTree(result, partialThread.CollectionResult as ThreadTreePartial);
                    }
                    if (partialThread.CollectionResult is ThreadTree)
                    {
                        result = partialThread.CollectionResult as ThreadTree;
                        gotPostCount = null;
                        gotUpdateDate = null;
                    }
                }
            }
            if (result == null)
            {
                token.ThrowIfCancellationRequested();
                var threadOperation = engine.GetThread(threadLink);
                threadOperation.Progress += (sender, e) => OnProgress(e);
                var thread = await threadOperation.Complete(token);
                if (thread == null || thread.CollectionResult == null)
                {
                    throw new InvalidOperationException("Не удалось получить тред");
                }
                if (!(thread.CollectionResult is ThreadTree))
                {
                    throw new InvalidOperationException("Неправильный ответ сервиса сетевого движка");
                }
                result = thread.CollectionResult as ThreadTree;
            }
            SignalProcessing("Обработка данных...", "PARSE");
            threadProcessService.SetBackLinks(result);
            threadProcessService.SortThreadTree(result);

            if (saveToCache)
            {
                await storage.ThreadData.SaveThread(result);
                if (updateVisit)
                {
                    await UpdateVisitedDb(result, storage, threadLink, gotUpdateDate, gotPostCount, linkHashService, threadProcessService);
                }
            }

            return result;
        }

        private async Task UpdateVisitedDb(ThreadTree result, IStorageService storage, BoardLinkBase threadLink,
            DateTime? gotUpdateDate, int? gotPostCount, ILinkHashService linkHashService,
            IThreadTreeProcessService threadProcessService)
        {
            var lastPost1 = result.Posts.LastOrDefault();
            var postCount = await storage.ThreadData.LoadPostCountInfo(threadLink);
            if (postCount == null)
            {
                postCount = new PostCountInfo()
                {
                    Link = threadLink,
                    LastView = DateTime.MinValue
                };
            }
            postCount.LastChange = gotUpdateDate ?? (lastPost1 != null ? lastPost1.Date : DateTime.MinValue);
            postCount.LoadedPostCount = gotPostCount ?? (lastPost1 != null ? lastPost1.Counter : 0);
            postCount.PostCount = postCount.LoadedPostCount;

            var threadHash = linkHashService.GetLinkHash(threadLink);
            await storage.ThreadData.SavePostCountInfo(postCount);
            var favorites = await storage.ThreadData.FavoriteThreads.LoadLinkCollection();
            if (favorites != null && favorites.Links != null && favorites is ThreadLinkCollection)
            {
                var fav2 = favorites as ThreadLinkCollection;
                if (fav2.ThreadInfo.ContainsKey(threadHash))
                {
                    var favData = fav2.ThreadInfo[threadHash];
                    if (favData is FavoriteThreadInfo)
                    {
                        var favData2 = favData as FavoriteThreadInfo;
                        favData2.CountInfo = postCount;
                        favData2.UpdatedDate = postCount.LastChange;
                        await storage.ThreadData.FavoriteThreads.SaveLinkCollection(favorites);
                    }
                }
                await Services.GetServiceOrThrow<ILiveTileService>().UpdateFavoritesTile(favorites);
            }

            var visited = await storage.ThreadData.VisitedThreads.LoadLinkCollection();
            if (visited == null)
            {
                visited = new ThreadLinkCollection()
                {
                    Links = new List<BoardLinkBase>(),
                    ThreadInfo = new Dictionary<string, ShortThreadInfo>(),
                };
            }
            var threadInfo = threadProcessService.GetShortInfo(result);
            if (visited is ThreadLinkCollection && threadInfo != null)
            {
                var visited2 = visited as ThreadLinkCollection;
                if (visited2.ThreadInfo == null)
                {
                    visited2.ThreadInfo = new Dictionary<string, ShortThreadInfo>();
                }
                if (visited2.Links == null)
                {
                    visited2.Links = new List<BoardLinkBase>();
                }
                threadInfo.UpdatedDate = postCount.LastChange;
                threadInfo.ViewDate = DateTime.Now;
                visited2.Links.Add(threadLink);
                if (visited2.ThreadInfo.ContainsKey(threadHash))
                {
                    var oldInfo = visited2.ThreadInfo[threadHash];
                    threadInfo.AddedDate = oldInfo.AddedDate;
                }
                else
                {
                    threadInfo.AddedDate = DateTime.Now;
                }
                visited2.ThreadInfo[threadHash] = threadInfo;
                var newLinks = visited2.Links
                    .WithKeys(linkHashService.GetLinkHash)
                    .Where(a => visited2.ThreadInfo.ContainsKey(a.Key))
                    .Select(a => new { link = a.Value, info = visited2.ThreadInfo[a.Key], hash = a.Key })
                    .Where(a => a.info != null)
                    .OrderByDescending(a => a.info.ViewDate)
                    .Take(CoreConstants.MaxVisitedThreads).ToList();
                visited2.Links = newLinks.Select(a => a.link).ToList();
                var newLinksHash = new HashSet<string>(newLinks.Select(a => a.hash));
                foreach (var h in visited2.ThreadInfo.Keys.ToArray())
                {
                    if (!newLinksHash.Contains(h))
                    {
                        visited2.ThreadInfo.Remove(h);
                    }
                }
                await storage.ThreadData.VisitedThreads.SaveLinkCollection(visited2);
            }
        }
    }
}