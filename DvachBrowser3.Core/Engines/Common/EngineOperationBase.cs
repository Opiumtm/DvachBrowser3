using System;
using System.Diagnostics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Базовый класс операции движка.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TParam">Тип параметра.</typeparam>
    public abstract class EngineOperationBase<T, TParam> : IEngineOperationsWithProgress<T, EngineProgress>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="services">Сервисы.</param>
        protected EngineOperationBase(TParam parameter, IServiceProvider services)
        {
            Parameter = parameter;
            Services = services;
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <returns>Таск.</returns>
        public abstract Task<T> Complete();

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены операции.</param>
        /// <returns>Таск.</returns>
        public async Task<T> Complete(CancellationToken token)
        {
            token.Register(Cancel);
            // ReSharper disable once MethodSupportsCancellation
            return await Complete();
        }

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        public event EventHandler<EngineProgress> Progress;

        private IAsyncInfo asyncInfo;

        protected IAsyncInfo Operation
        {
            get { return Interlocked.CompareExchange(ref asyncInfo, null, null); }
            set { Interlocked.Exchange(ref asyncInfo, value); }
        }

        /// <summary>
        /// Отменить.
        /// </summary>
        protected virtual void Cancel()
        {
            try
            {
                var operation = Operation;
                if (operation != null)
                {
                    if (operation.Status == AsyncStatus.Started)
                    {
                        operation.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Прогресс.
        /// </summary>
        /// <param name="e">Событие прогресса.</param>
        protected virtual void OnProgress(EngineProgress e)
        {
            EventHandler<EngineProgress> handler = Progress;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Обработка данных.
        /// </summary>
        protected virtual void SignalProcessing()
        {
            dynamic otherData = new ExpandoObject();
            otherData.Kind = "PARSE";
            OnProgress(new EngineProgress("Обработка данных...", null, otherData));            
        }

        /// <summary>
        /// Параметр.
        /// </summary>
        public TParam Parameter { get; private set; }

        /// <summary>
        /// Сервисы.
        /// </summary>
        protected IServiceProvider Services { get; private set; }
    }
}