using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Styles;
using DvachBrowser3.Views;
using Template10.Common;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Обёртка для прогресса операции.
    /// </summary>
    /// <typeparam name="TResult">Результат.</typeparam>
    /// <typeparam name="TProgress">Тип прогресса.</typeparam>
    public abstract class EngineOperationWrapper<TResult, TProgress> : ViewModelBase, IOperationViewModel, IOperationProgressViewModel where TProgress : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="operationFactory">Фабрика операций.</param>
        protected EngineOperationWrapper(Func<object, IEngineOperationsWithProgress<TResult, TProgress>> operationFactory)
        {
            if (operationFactory == null) throw new ArgumentNullException(nameof(operationFactory));
            OperationFactory = operationFactory;
            Progress = 0;
            IsIndeterminate = true;
            IsError = false;
            IsCancelled = false;
            Error = null;
            Message = null;
            IsActive = false;
            UpdateCanStart();
        }

        private TResult result;

        /// <summary>
        /// Результат.
        /// </summary>
        public TResult Result
        {
            get { return result; }
            set
            {
                result = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Фабрика операций.
        /// </summary>
        protected Func<object, IEngineOperationsWithProgress<TResult, TProgress>> OperationFactory { get; private set; }

        private bool canStart;

        /// <summary>
        /// Можно начинать.
        /// </summary>
        public bool CanStart
        {
            get { return canStart; }
            private set
            {
                canStart = value;
                RaisePropertyChanged();
            }
        }

        private bool isDisabled;

        private void UpdateCanStart()
        {
            CanStart = !IsActive && !isDisabled;
        }

        private async Task DoOperation(IEngineOperationsWithProgress<TResult, TProgress> operation, object arg)
        {
            IsActive = true;
            UpdateCanStart();
            IsError = false;
            Error = null;
            Exception = null;
            Progress = 0;
            IsIndeterminate = true;
            Message = null;
            IsCancelled = false;
            IsWaiting = true;
            Argument = arg;
            operation.Progress += OperationOnProgress;
            OperationProgressFinishedEventArgs finishArgs = null;
            try
            {
                using (var tokenSource = new CancellationTokenSource())
                {
                    cancelAction = () => tokenSource.Cancel();
                    try
                    {
                        try
                        {
                            Started?.Invoke(this, EventArgs.Empty);
                            Result = await operation.Complete(tokenSource.Token);
                            finishArgs = new OperationProgressFinishedEventArgs(arg);
                            ResultGot?.Invoke(this, EventArgs.Empty);
                        }
                        catch (Exception ex)
                        {
                            if (!tokenSource.IsCancellationRequested)
                            {
                                IsError = true;
                                Error = ex.Message;
                                Exception = ex;
                                finishArgs = new OperationProgressFinishedEventArgs(arg, ex);
                            }
                        }
                    }
                    finally
                    {
                        cancelAction = null;
                    }
                    if (tokenSource.IsCancellationRequested)
                    {
                        IsCancelled = true;
                        finishArgs = new OperationProgressFinishedEventArgs(arg, null, true);
                    }
                }
            }
            finally
            {
                operation.Progress -= OperationOnProgress;
                IsActive = false;
                IsWaiting = false;
                UpdateCanStart();
                if (finishArgs != null)
                {
                    Finished?.Invoke(this, finishArgs);
                }
            }
        }

        /// <summary>
        /// Высокий приоритет.
        /// </summary>
        public bool HighPriority { get; set; }

        /// <summary>
        /// Начать.
        /// </summary>
        /// <param name="arg">Аргумент.</param>
        public void Start2(object arg)
        {
            AppHelpers.DispatchAction(async () =>
            {
                if (!CanStart)
                {
                    return;
                }
                var operation = OperationFactory(arg);
                if (operation == null)
                {
                    return;
                }
                var cancelFlag = new InterlockedFlagContainer();
                cancelAction = () =>
                {
                    cancelFlag.Flag = true;
                };
                Func<Task> action = async () =>
                {
                    if (cancelFlag.Flag)
                    {
                        return;
                    }
                    await DoOperation(operation, arg);
                };
                await action();
            });
        }

        public void Start()
        {
            Start2(null);
        }

        private void OperationOnProgress(object sender, TProgress p)
        {
            BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
            {
                var progressValue = GetProgress(p);
                var messageValue = GetMessage(p);
                if (progressValue == null)
                {
                    Progress = 0;
                    IsIndeterminate = true;
                }
                else
                {
                    Progress = progressValue.Value;
                }
                Message = messageValue;
                IsWaiting = GetIsWaiting(p);
            });
        }

        /// <summary>
        /// Получить прогресс.
        /// </summary>
        /// <param name="progress">Объект прогресса.</param>
        /// <returns>Прогресс.</returns>
        protected abstract double? GetProgress(TProgress progress);

        /// <summary>
        /// Получить сообщение.
        /// </summary>
        /// <param name="progress">Объект прогресса.</param>
        /// <returns>Сообщение.</returns>
        protected abstract string GetMessage(TProgress progress);

        /// <summary>
        /// Получить статус ожидания.
        /// </summary>
        /// <param name="progress">Прогресс.</param>
        /// <returns>Статус ожидания.</returns>
        protected abstract bool GetIsWaiting(TProgress progress);

        private Action cancelAction;

        /// <summary>
        /// Отменить.
        /// </summary>
        public void Cancel()
        {
            cancelAction?.Invoke();
        }

        /// <summary>
        /// Прогресс.
        /// </summary>
        IOperationProgressViewModel IOperationViewModel.Progress => this;

        /// <summary>
        /// Запретить.
        /// </summary>
        public void Disable()
        {
            isDisabled = true;
            UpdateCanStart();
        }

        /// <summary>
        /// Разрешить.
        /// </summary>
        public void Enable()
        {
            isDisabled = false;
            UpdateCanStart();
        }

        /// <summary>
        /// Нужна диспетчеризация.
        /// </summary>
        public bool NeedDispatch { get; set; }

        private double progress;

        /// <summary>
        /// Прогресс.
        /// </summary>
        public double Progress
        {
            get { return progress; }
            private set
            {
                progress = value;
                RaisePropertyChanged();
            }
        }

        private bool isIndeterminate;

        /// <summary>
        /// Неявное положение.
        /// </summary>
        public bool IsIndeterminate
        {
            get { return isIndeterminate; }
            private set
            {
                isIndeterminate = value;
                RaisePropertyChanged();
            }
        }

        private string message;

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message
        {
            get { return message; }
            private set
            {
                message = value;
                RaisePropertyChanged();
            }
        }

        private bool isActive;

        /// <summary>
        /// Активен.
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            private set
            {
                isActive = value;
                RaisePropertyChanged();
            }
        }

        private string error;

        /// <summary>
        /// Ошибка.
        /// </summary>
        public string Error
        {
            get { return error; }
            private set
            {
                error = value;
                RaisePropertyChanged();
            }
        }

        private Exception exception;

        /// <summary>
        /// Полная ошибка.
        /// </summary>
        public Exception Exception
        {
            get { return exception; }
            private set
            {
                exception = value;
                RaisePropertyChanged();
            }
        }

        private bool isError;

        /// <summary>
        /// Ошибка.
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            private set
            {
                isError = value;
                RaisePropertyChanged();
            }
        }

        private bool isCancelled;

        /// <summary>
        /// Отменено.
        /// </summary>
        public bool IsCancelled
        {
            get { return isCancelled; }
            private set
            {
                isCancelled = value;
                RaisePropertyChanged();
            }
        }

        private bool isWaiting;

        /// <summary>
        /// Ожидается.
        /// </summary>
        public bool IsWaiting
        {
            get { return isWaiting; }
            private set
            {
                isWaiting = value;
                RaisePropertyChanged();
            }
        }

        private object argument;

        /// <summary>
        /// Аргумент.
        /// </summary>
        public object Argument
        {
            get { return argument; }
            private set
            {
                argument = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Запущено.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Прогресс изменился.
        /// </summary>
        public event EventHandler ProgressChanged;

        /// <summary>
        /// Завершено.
        /// </summary>
        public event OperationProgressFinishedEventHandler Finished;

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager => Shell.StyleManager;

        /// <summary>
        /// Получен результат.
        /// </summary>
        public event EventHandler ResultGot;
    }

    /// <summary>
    /// Диспетчер асинхронных операций.
    /// </summary>
    public static class EngineOperationWrapperDispatcher
    {
        /// <summary>
        /// Диспетчер.
        /// </summary>
        public static readonly AsyncOperationDispatcher Dispatcher = new AsyncOperationDispatcher(100);
    }
}