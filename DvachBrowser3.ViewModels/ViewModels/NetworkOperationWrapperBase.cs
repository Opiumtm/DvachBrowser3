using System;
using System.Threading;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// ������� ����� ������� ��������.
    /// </summary>
    public abstract class NetworkOperationWrapperBase : ViewModelBase, INetworkViewModel
    {
        /// <summary>
        /// ����� ������.
        /// </summary>
        protected readonly Func<CancellationToken> CancellationToken;

        /// <summary>
        /// �����������.
        /// </summary>
        /// <param name="cancellationToken">����� ������.</param>
        protected NetworkOperationWrapperBase(Func<CancellationToken> cancellationToken)
        {
            CancellationToken = cancellationToken;
        }

        /// <summary>
        /// ����� ��������� ��������.
        /// </summary>
        /// <param name="parameter">��������.</param>
        /// <returns>���������.</returns>
        public bool CanExecute(object parameter)
        {
            return IsCanExecute;
        }

        /// <summary>
        /// ��������� ��������.
        /// </summary>
        /// <param name="parameter">��������.</param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// ���������� ����������� ����������.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// ���������� ����������� ����������.
        /// </summary>
        protected void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// ��������� ��������.
        /// </summary>
        public abstract void ExecuteOperation();

        private bool isExecuting;

        /// <summary>
        /// �������� �����������.
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
        /// ���� ������.
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
        /// �������� ��������� �������.
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
        /// ����� ���������.
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
        /// �������� ��������.
        /// </summary>
        public event EventHandler<EngineProgress> Progress;

        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="e">��������.</param>
        protected void OnProgress(EngineProgress e)
        {
            EventHandler<EngineProgress> handler = Progress;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// ������.
        /// </summary>
        public event EventHandler Error;

        /// <summary>
        /// ������.
        /// </summary>
        protected virtual void OnError()
        {
            EventHandler handler = Error;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// ���������.
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// ���������.
        /// </summary>
        protected virtual void OnCompleted()
        {
            EventHandler handler = Completed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// ������.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// ������.
        /// </summary>
        protected virtual void OnStarted()
        {
            EventHandler handler = Started;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private string errorText;

        /// <summary>
        /// ����� ������.
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