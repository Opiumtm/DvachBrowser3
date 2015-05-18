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
    public sealed class NetworkOperationWrapper<T, TResult> : ViewModelBase, INetworkViewModel where T : EventArgs
    {
        private readonly Func<IEngineOperationsWithProgress<TResult, EngineProgress>> operationFactory;

        private readonly Func<TResult, Task<T>> eventFactory;

        private readonly Func<CancellationToken> cancellationToken;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="operationFactory">Фабрика операций.</param>
        /// <param name="eventFactory">Фабрика событий.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        public NetworkOperationWrapper(Func<IEngineOperationsWithProgress<TResult, EngineProgress>> operationFactory, Func<TResult, Task<T>> eventFactory, Func<CancellationToken> cancellationToken = null)
        {
            if (operationFactory == null) throw new ArgumentNullException("operationFactory");
            if (eventFactory == null) throw new ArgumentNullException("eventFactory");
            this.operationFactory = operationFactory;
            this.eventFactory = eventFactory;
            this.cancellationToken = cancellationToken;
            IsCanExecute = true;
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        public async void ExecuteOperation()
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
                        var r = await operation.Complete(cancellationToken != null ? cancellationToken() : new CancellationToken());
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
        public bool IsCanExecute
        {
            get { return canExecute; }
            set
            {
                canExecute = value;
                OnPropertyChanged();
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        public event EventHandler<EngineProgress> Progress;

        /// <summary>
        /// Ошибка.
        /// </summary>
        public event EventHandler Error;

        private void OnError()
        {
            EventHandler handler = Error;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Завершено.
        /// </summary>
        public event EventHandler Completed;

        private void OnCompleted()
        {
            EventHandler handler = Completed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Начато.
        /// </summary>
        public event EventHandler Started;

        private void OnStarted()
        {
            EventHandler handler = Started;
            if (handler != null) handler(this, EventArgs.Empty);
        }

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

        /// <summary>
        /// Можно выполнить.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>Результат.</returns>
        public bool CanExecute(object parameter)
        {
            return IsCanExecute;
        }

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public void Execute(object parameter)
        {
            ExecuteOperation();
        }

        /// <summary>
        /// Можно выполнить.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Можно выполнить.
        /// </summary>
        private void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}