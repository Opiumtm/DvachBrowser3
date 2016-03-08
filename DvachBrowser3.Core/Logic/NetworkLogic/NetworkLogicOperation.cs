using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Storage;

namespace DvachBrowser3.Logic.NetworkLogic
{
    /// <summary>
    /// Операция сетевой логики.
    /// </summary>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class NetworkLogicOperation<TResult, TParam> : IEngineOperationsWithProgress<TResult, EngineProgress>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="parameter">Параметр.</param>
        protected NetworkLogicOperation(IServiceProvider services, TParam parameter)
        {
            Services = services;
            Parameter = parameter;
        }

        /// <summary>
        /// Сервисы.
        /// </summary>
        protected IServiceProvider Services { get; private set; }

        /// <summary>
        /// Параметр.
        /// </summary>
        protected TParam Parameter { get; private set; }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <returns>Таск.</returns>
        public async Task<TResult> Complete()
        {
            return await Complete(new CancellationToken());
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public abstract Task<TResult> Complete(CancellationToken token);

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        public event EventHandler<EngineProgress> Progress;

        /// <summary>
        /// Вызов события прогресса.
        /// </summary>
        /// <param name="e">Параметр события.</param>
        protected virtual void OnProgress(EngineProgress e)
        {
            EventHandler<EngineProgress> handler = Progress;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Обработка данных.
        /// </summary>
        /// <param name="msg">Сообщение.</param>
        /// <param name="kind">Тип операции.</param>
        protected virtual void SignalProcessing(string msg, string kind)
        {
            var otherData = new EngineProgressOtherData();
            otherData.Kind = kind;
            OnProgress(new EngineProgress(msg, null, otherData));
        }

        /// <summary>
        /// Получить движок.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        protected INetworkEngine GetEngine(BoardLinkBase link)
        {
            return Services.GetServiceOrThrow<INetworkEngines>().GetEngineById(link.Engine);
        }

        /// <summary>
        /// Получить хранилище.
        /// </summary>
        /// <returns>Хранилище.</returns>
        protected IStorageService GetStorage()
        {
            return Services.GetServiceOrThrow<IStorageService>();
        }
    }
}