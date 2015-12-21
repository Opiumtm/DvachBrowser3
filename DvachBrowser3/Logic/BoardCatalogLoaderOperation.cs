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
    public sealed class BoardCatalogLoaderOperation : NetworkLogicOperation<CatalogTree, BoardCatalogLoaderArgument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public BoardCatalogLoaderOperation(IServiceProvider services, BoardCatalogLoaderArgument parameter) : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<CatalogTree> Complete(CancellationToken token)
        {
            //var engines = Services.GetServiceOrThrow<INetworkEngines>();
            //var engine = engines.FindEngine(Parameter.Link?.Engine);
            var networkLogic = Services.GetServiceOrThrow<INetworkLogic>();
            var storage = Services.GetServiceOrThrow<IStorageService>();
            if (Parameter.UpdateMode == BoardCatalogUpdateMode.GetFromCache)
            {
                var tree = await storage.ThreadData.LoadCatalog(Parameter.Link);
                if (tree != null)
                {
                    return tree;
                }
            }
            var request = networkLogic.GetCatalog(Parameter.Link);
            request.Progress += (sender, e) => OnProgress(e);
            var data = await request.Complete(token);
            return data;
        }
    }
}