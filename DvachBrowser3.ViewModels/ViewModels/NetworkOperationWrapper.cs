using System;
using System.Threading;
using Windows.System.UserProfile;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Враппер сетевой операции.
    /// </summary>
    /// <typeparam name="T">Тип результата.</typeparam>
    /// <typeparam name="TResult">Тип промежуточного результата.</typeparam>
    public sealed class NetworkOperationWrapper<T, TResult> : ViewModelBase, INetworkViewModel where T : EventArgs
    {
        private readonly Func<IEngineOperationsWithProgress<TResult, EngineProgress>> operationFactory;

        private readonly Func<TResult, T> eventFactory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="operationFactory">Фабрика операций.</param>
        /// <param name="eventFactory">Фабрика событий.</param>
        public NetworkOperationWrapper(Func<IEngineOperationsWithProgress<TResult, EngineProgress>> operationFactory, Func<TResult, T> eventFactory)
        {
            if (operationFactory == null) throw new ArgumentNullException("operationFactory");
            if (eventFactory == null) throw new ArgumentNullException("eventFactory");
            this.operationFactory = operationFactory;
            this.eventFactory = eventFactory;
            CanExecute = true;
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены.</param>
        public async void ExecuteOperation(CancellationToken token)
        {
            try
            {
                if (!CanExecute)
                {
                    return;
                }
                CanExecute = false;
                IsExecuting = true;
                try
                {
                    IsOk = false;
                    IsError = false;
                    var operation = operationFactory();
                    operation.Progress += OperationOnProgress;
                    try
                    {
                        var r = await operation.Complete(token);
                        IsOk = true;
                        IsError = false;
                        ErrorText = null;
                        var e = eventFactory(r);
                        OnSetResult(e);
                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        IsOk = false;
                        ErrorText = ex.Message;
                    }
                }
                finally
                {
                    CanExecute = true;
                    IsExecuting = false;
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

        private bool isExecuting;

        /// <summary>
        /// Операция выполняется.
        /// </summary>
        public bool IsExecuting
        {
            get { return isExecuting; }
            set
            {
                isExecuting = value;
                OnPropertyChanged();
            }
        }

        private bool isError;

        /// <summary>
        /// Есть ошибка.
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            set
            {
                isError = value;
                OnPropertyChanged();
            }
        }

        private bool isOk;

        /// <summary>
        /// Операция завершена успешно.
        /// </summary>
        public bool IsOk
        {
            get { return isOk; }
            set
            {
                isOk = value;
                OnPropertyChanged();
            }
        }

        private bool canExecute;

        /// <summary>
        /// Можно выполнять.
        /// </summary>
        public bool CanExecute
        {
            get { return canExecute; }
            set
            {
                canExecute = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        public event EventHandler<EngineProgress> Progress;

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        /// <param name="e">Событие.</param>
        private void OnProgress(EngineProgress e)
        {
            EventHandler<EngineProgress> handler = Progress;
            if (handler != null) handler(this, e);
        }

        private string errorText;

        /// <summary>
        /// Текст ошибки.
        /// </summary>
        public string ErrorText
        {
            get { return errorText; }
            set
            {
                errorText = value;
                OnPropertyChanged();
            }
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
    }
}