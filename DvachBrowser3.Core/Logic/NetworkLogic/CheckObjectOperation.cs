using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция проверки объекта.
    /// </summary>
    public sealed class CheckObjectOperation : NetworkLogicOperation<bool?, BoardLinkBase>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        public CheckObjectOperation(IServiceProvider services, BoardLinkBase parameter) : base(services, parameter)
        {
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public override async Task<bool?> Complete(CancellationToken token)
        {
            var engine = GetEngine(Parameter);
            var storage = GetStorage();
            if ((engine.Capability & EngineCapability.LastModifiedRequest) == 0)
            {
                return null;
            }
            var etag = await storage.ThreadData.LoadStamp(Parameter);
            if (etag == null)
            {
                return true;
            }
            token.ThrowIfCancellationRequested();
            var etagOperation = engine.GetResourceLastModified(Parameter);
            SignalProcessing("Проверка обновлений...", "ETAG");
            var newEtag = await etagOperation.Complete(token);
            if (newEtag.LastModified == null)
            {
                return null;
            }
            return etag != newEtag.LastModified;
        }
    }
}