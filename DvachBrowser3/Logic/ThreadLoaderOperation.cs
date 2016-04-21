using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic.NetworkLogic;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Операция загрузки треда.
    /// </summary>
    public sealed class ThreadLoaderOperation : NetworkLogicOperation<IThreadLoaderResult, ThreadLoaderArgument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public ThreadLoaderOperation(IServiceProvider services, ThreadLoaderArgument parameter) : base(services, parameter)
        {
            linkComparer = Services.GetServiceOrThrow<ILinkHashService>().GetComparer();
        }

        private static ThreadTree cachedTree;

        private static BoardLinkBase cacheLink;

        private readonly IEqualityComparer<BoardLinkBase> linkComparer;

        private static readonly object CacheLock = new object();

        private ThreadTree TryGetTree(BoardLinkBase link)
        {
            lock (CacheLock)
            {
                if (link == null || cacheLink == null)
                {
                    return null;
                }
                if (linkComparer.Equals(cacheLink, link))
                {
                    return cachedTree;
                }
                return null;
            }
        }

        private void PutTree(BoardLinkBase link, ThreadTree tree)
        {
            lock (CacheLock)
            {
                if (link == null || tree == null)
                {
                    cacheLink = null;
                    cachedTree = null;
                }
                else
                {
                    cacheLink = link;
                    cachedTree = tree;
                }
            }
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<IThreadLoaderResult> Complete(CancellationToken token)
        {
            var engines = Services.GetServiceOrThrow<INetworkEngines>();
            var engine = engines.FindEngine(Parameter.ThreadLink?.Engine);
            var networkLogic = Services.GetServiceOrThrow<INetworkLogic>();
            var storage = Services.GetServiceOrThrow<IStorageService>();
            if (Parameter.UpdateMode == ThreadLoaderUpdateMode.GetFromCache || Parameter.UpdateMode == ThreadLoaderUpdateMode.GetFromCacheOffline)
            {
                var tree = TryGetTree(Parameter.ThreadLink);
                if (tree == null)
                {
                    tree = await storage.ThreadData.LoadThread(Parameter.ThreadLink);
                    PutTree(Parameter.ThreadLink, tree);
                }
                if (tree != null)
                {
                    try
                    {
                        if (engine == null)
                        {
                            return new OperaiontResult() { IsUpdated = false, Data = tree };
                        }
                        if ((engine.Capability & EngineCapability.LastModifiedRequest) == 0 || Parameter.UpdateMode == ThreadLoaderUpdateMode.GetFromCacheOffline)
                        {
                            return new OperaiontResult() { IsUpdated = false, Data = tree };
                        }
                        var lastId = await storage.ThreadData.LoadStamp(Parameter.ThreadLink);
                        var updateOperation = engine.GetResourceLastModified(Parameter.ThreadLink);
                        updateOperation.Progress += (sender, e) => OnProgress(e);
                        var newId = await updateOperation.Complete(token);
                        return new OperaiontResult() { IsUpdated = lastId != newId?.LastModified, Data = tree };
                    }
                    catch
                    {
                        return new OperaiontResult() { IsUpdated = false, Data = tree };
                    }
                }
            }
            else if (Parameter.UpdateMode == ThreadLoaderUpdateMode.CheckForUpdates)
            {
                if (engine == null)
                {
                    return new OperaiontResult() { IsUpdated = false };
                }
                if ((engine.Capability & EngineCapability.LastModifiedRequest) == 0)
                {
                    return new OperaiontResult() { IsUpdated = false };
                }
                var lastId = await storage.ThreadData.LoadStamp(Parameter.ThreadLink);
                var updateOperation = engine.GetResourceLastModified(Parameter.ThreadLink);
                updateOperation.Progress += (sender, e) => OnProgress(e);
                var newId = await updateOperation.Complete(token);
                return new OperaiontResult() { IsUpdated = lastId != newId?.LastModified };
            }

            IEngineOperationsWithProgress<ThreadTree, EngineProgress> request;

            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (Parameter.UpdateMode == ThreadLoaderUpdateMode.LoadFull)
            {
                request = networkLogic.LoadThread(Parameter.ThreadLink, UpdateThreadMode.DefaultFull);
            }
            else if (Parameter.UpdateMode == ThreadLoaderUpdateMode.ResyncFull)
            {
                request = networkLogic.LoadThread(Parameter.ThreadLink, UpdateThreadMode.DefaultReload);
            }
            else
            {
                request = networkLogic.LoadThread(Parameter.ThreadLink);
            }

            request.Progress += (sender, e) => OnProgress(e);
            var data = await request.Complete(token);
            PutTree(Parameter.ThreadLink, data);

            ViewModelEvents.VisitedListRefreshed.RaiseEvent(this, null);
            ViewModelEvents.FavoritesListRefreshed.RaiseEvent(this, null);

            return new OperaiontResult() { Data = data, IsUpdated = false };
        }

        /// <summary>
        /// Результат операции.
        /// </summary>
        private sealed class OperaiontResult : IThreadLoaderResult
        {
            /// <summary>
            /// Данные.
            /// </summary>
            public ThreadTree Data { get; set; }

            /// <summary>
            /// Обновлено (для режимов с проверкой обновления).
            /// </summary>
            public bool IsUpdated { get; set; }

            /// <summary>
            /// Получить посты.
            /// </summary>
            /// <returns>Посты.</returns>
            public Task<IList<PostTree>> GetPosts()
            {
                if (Data != null)
                {
                    return ((IPostTreeListSource)Data).GetPosts();
                }
                return Task.FromResult((IList<PostTree>) null);
            }
        }
    }
}