using System;
using System.Threading;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Базовый класс сетевой операции.
    /// </summary>
    public abstract class NetworkOperationWrapperBase : ViewModelBase, INetworkViewModel
    {
        /// <summary>
        /// Токен отмены.
        /// </summary>
        protected readonly Func<CancellationToken> CancellationToken;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        protected NetworkOperationWrapperBase(Func<CancellationToken> cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        /// <summary>
        /// Можно выполнить операцию.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>Результат.</returns>
        public bool CanExecute(object parameter)
        {
            return IsCanExecute;
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// Изменилась возможность исполнения.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Изменилась возможность исполнения.
        /// </summary>
        protected void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        public abstract void ExecuteOperation();

        private bool isExecuting;

        /// <summary>
        /// Операция выполняется.
        /// </summary>
        public bool IsExecuting
        {
            get { return isExecuting; }
            protected set
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
            get
            {
                return isError;
            }
            protected set
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
            get
            {
                return isOk;
            }
            protected set
            {
                isOk = value;
                OnPropertyChanged();
            }
        }

        private bool isCanExecute = true;

        /// <summary>
        /// Можно выполнять.
        /// </summary>
        public bool IsCanExecute
        {
            get { return isCanExecute; }
            protected set
            {
                isCanExecute = value;
                OnPropertyChanged();
                OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        public event EventHandler<EngineProgress> Progress;

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        /// <param name="e">Прогресс.</param>
        protected void OnProgress(EngineProgress e)
        {
            EventHandler<EngineProgress> handler = Progress;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Ошибка.
        /// </summary>
        public event EventHandler Error;

        /// <summary>
        /// Ошибка.
        /// </summary>
        protected virtual void OnError()
        {
            EventHandler handler = Error;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Завершено.
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// Завершено.
        /// </summary>
        protected virtual void OnCompleted()
        {
            EventHandler handler = Completed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Начато.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Начато.
        /// </summary>
        protected virtual void OnStarted()
        {
            EventHandler handler = Started;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private string errorText;

        /// <summary>
        /// Текст ошибки.
        /// </summary>
        public string ErrorText
        {
            get { return errorText; }
            protected set
            {
                errorText = value;
                OnPropertyChanged();
            }
        }
    }
}