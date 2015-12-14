using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция получения каталога.
    /// </summary>
    public class CatalogOperation : NetworkLogicOperation<CatalogTree, CatalogParameter>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public CatalogOperation(IServiceProvider services, CatalogParameter parameter) : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<CatalogTree> Complete(CancellationToken token)
        {
            var engine = GetEngine(Parameter.Link);
            if ((engine.Capability & EngineCapability.Catalog) == 0)
            {
                return null;
            }
            var operation = engine.GetCatalog(Parameter.Link, Parameter.SortMode);
            operation.Progress += (sender, e) => OnProgress(e);
            var result = await operation.Complete(token);
            var tree = result?.CollectionResult as CatalogTree;
            return tree;
        }
    }
}