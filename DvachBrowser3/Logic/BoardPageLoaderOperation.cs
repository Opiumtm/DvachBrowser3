using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Logic.NetworkLogic;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Операция загрузки данных по борде.
    /// </summary>
    public sealed class BoardPageLoaderOperation : NetworkLogicOperation<IBoardPageLoaderResult, BoardPageLoaderArgument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public BoardPageLoaderOperation(IServiceProvider services, BoardPageLoaderArgument parameter) : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<IBoardPageLoaderResult> Complete(CancellationToken token)
        {
            var engines = Services.GetServiceOrThrow<INetworkEngines>();
            var engine = engines.FindEngine(Parameter.PageLink?.Engine);
            var networkLogic = Services.GetServiceOrThrow<INetworkLogic>();
            var storage = Services.GetServiceOrThrow<IStorageService>();
            if (Parameter.UpdateMode == BoardPageLoaderUpdateMode.GetFromCache)
            {
                var tree = await storage.ThreadData.LoadBoardPage(Parameter.PageLink);
                if (tree != null)
                {
                    try
                    {
                        if (engine == null)
                        {
                            return new OperaiontResult() { IsUpdated = false, Data = tree };
                        }
                        if ((engine.Capability & EngineCapability.LastModifiedRequest) == 0)
                        {
                            return new OperaiontResult() { IsUpdated = false, Data = tree };
                        }
                        var lastId = await storage.ThreadData.LoadStamp(Parameter.PageLink);
                        var updateOperation = engine.GetResourceLastModified(Parameter.PageLink);
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
            else if (Parameter.UpdateMode == BoardPageLoaderUpdateMode.CheckForUpdates)
            {
                if (engine == null)
                {
                    return new OperaiontResult() {IsUpdated = false};
                }
                if ((engine.Capability & EngineCapability.LastModifiedRequest) == 0)
                {
                    return new OperaiontResult() {IsUpdated = false};
                }
                var lastId = await storage.ThreadData.LoadStamp(Parameter.PageLink);
                var updateOperation = engine.GetResourceLastModified(Parameter.PageLink);
                updateOperation.Progress += (sender, e) => OnProgress(e);
                var newId = await updateOperation.Complete(token);
                return new OperaiontResult() {IsUpdated = lastId != newId?.LastModified};
            }
            var request = networkLogic.LoadBoardPage(Parameter.PageLink);
            request.Progress += (sender, e) => OnProgress(e);
            var data = await request.Complete(token);
            return new OperaiontResult() {Data = data, IsUpdated = false};
        }

        /// <summary>
        /// Результат операции.
        /// </summary>
        private sealed class OperaiontResult : IBoardPageLoaderResult
        {
            /// <summary>
            /// Данные.
            /// </summary>
            public BoardPageTree Data { get; set; }

            /// <summary>
            /// Обновлено (для режимов с проверкой обновления).
            /// </summary>
            public bool IsUpdated { get; set; }
        }
    }
}