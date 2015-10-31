using System;
using System.Threading;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
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
        protected EngineOperationWrapper(Func<IEngineOperationsWithProgress<TResult, TProgress>> operationFactory)
        {
            if (operationFactory == null) throw new ArgumentNullException(nameof(operationFactory));
            OperationFactory = operationFactory;
            CanStart = true;
            Progress = 0;
            IsIndeterminate = true;
            IsError = false;
            IsCancelled = false;
            Error = null;
            Message = null;
            IsActive = false;
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
        protected Func<IEngineOperationsWithProgress<TResult, TProgress>> OperationFactory { get; private set; }

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

        /// <summary>
        /// Начать.
        /// </summary>
        public async void Start()
        {
            if (!CanStart)
            {
                return;
            }
            var operation = OperationFactory();
            if (operation == null)
            {
                return;
            }
            IsActive = true;
            CanStart = false;
            IsError = false;
            Error = null;
            Progress = 0;
            IsIndeterminate = true;
            Message = null;
            IsCancelled = false;
            operation.Progress += OperationOnProgress;
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
                            Finished?.Invoke(this, new OperationProgressFinishedEventArgs());
                            ResultGot?.Invoke(this, EventArgs.Empty);
                        }
                        catch (Exception ex)
                        {
                            if (!tokenSource.IsCancellationRequested)
                            {
                                IsError = true;
                                Error = ex.Message;
                                Finished?.Invoke(this, new OperationProgressFinishedEventArgs(ex));
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
                        Finished?.Invoke(this, new OperationProgressFinishedEventArgs(null, true));
                    }
                }
            }
            finally
            {
                operation.Progress -= OperationOnProgress;
                IsActive = false;
                CanStart = true;
            }
        }

        private void OperationOnProgress(object sender, TProgress p)
        {
            Dispatcher.Dispatch(() =>
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
        /// Получен результат.
        /// </summary>
        public event EventHandler ResultGot;
    }
}