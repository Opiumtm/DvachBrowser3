using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Враппер сетевой операции.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TResult">Тип промежуточного результата.</typeparam>
    public sealed class NetworkOperationWrapper<T, TResult> : NetworkOperationWrapperBase where T : EventArgs
    {
        private readonly Func<IEngineOperationsWithProgress<TResult, EngineProgress>> operationFactory;

        private readonly Func<TResult, Task<T>> eventFactory;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="operationFactory">Фабрика операций.</param>
        /// <param name="eventFactory">Фабрика событий.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public NetworkOperationWrapper(Func<IEngineOperationsWithProgress<TResult, EngineProgress>> operationFactory, Func<TResult, Task<T>> eventFactory, Func<CancellationToken> cancellationToken = null)
            : base(cancellationToken)
        {
            if (operationFactory == null) throw new ArgumentNullException("operationFactory");
            if (eventFactory == null) throw new ArgumentNullException("eventFactory");
            this.operationFactory = operationFactory;
            this.eventFactory = eventFactory;
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        public override async void ExecuteOperation()
        {
            try
            {
                if (!IsCanExecute)
                {
                    return;
                }
                IsCanExecute = false;
                IsExecuting = true;
                try
                {
                    IsOk = false;
                    IsError = false;
                    OnStarted();
                    var operation = operationFactory();
                    operation.Progress += OperationOnProgress;
                    try
                    {
                        var r = await operation.Complete(CancellationToken != null ? CancellationToken() : new CancellationToken());
                        IsOk = true;
                        IsError = false;
                        ErrorText = null;
                        var e = await eventFactory(r);
                        OnSetResult(e);
                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        IsOk = false;
                        ErrorText = ex.Message;
                        OnError();
                    }
                }
                finally
                {
                    IsCanExecute = true;
                    IsExecuting = false;
                    OnCompleted();
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private void OperationOnProgress(object sender, EngineProgress engineProgress)
        {
            OnProgress(engineProgress);
        }

        /// <summary>
        /// Установить результат.
        /// </summary>
        public event EventHandler<T> SetResult;

        /// <summary>
        /// Установить результат.
        /// </summary>
        /// <param name="e">Событие.</param>
        private void OnSetResult(T e)
        {
            EventHandler<T> handler = SetResult;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public override void Execute(object parameter)
        {
            ExecuteOperation();
        }
    }
}