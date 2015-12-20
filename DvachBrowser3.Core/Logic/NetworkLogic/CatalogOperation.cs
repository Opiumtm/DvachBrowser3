using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;

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
            var checkEtag = ((Parameter.UpdateMode & UpdateCatalogMode.CheckETag) != 0) && ((engine.Capability & EngineCapability.LastModifiedRequest) != 0);
            var saveToCache = (Parameter.UpdateMode & UpdateCatalogMode.SaveToCache) != 0;
            var storage = Services.GetServiceOrThrow<IStorageService>();
            var oldData = await storage.ThreadData.LoadCatalog(Parameter.Link);
            string newEtag = null;
            if (checkEtag && oldData != null)
            {
                token.ThrowIfCancellationRequested();
                var etag = await storage.ThreadData.LoadStamp(Parameter.Link);
                if (etag != null)
                {
                    var etagOperation = engine.GetResourceLastModified(Parameter.Link);
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
            CatalogTree result;
            token.ThrowIfCancellationRequested();
            var threadOperation = engine.GetCatalog(Parameter.Link);
            threadOperation.Progress += (sender, e) => OnProgress(e);
            var page = await threadOperation.Complete(token);
            result = page?.CollectionResult as CatalogTree;
            if (result == null)
            {
                throw new InvalidOperationException("Не удалось получить каталог");
            }
            newEtag = result.ETag ?? Guid.NewGuid().ToString();
            if (saveToCache)
            {
                await storage.ThreadData.SaveCatalog(result);
                if ((engine.Capability & EngineCapability.LastModifiedRequest) != 0)
                {
                    await storage.ThreadData.SaveStamp(Parameter.Link, newEtag);
                }
            }
            return result;
        }
    }
}