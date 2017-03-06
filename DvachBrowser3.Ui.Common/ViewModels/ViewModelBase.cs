using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Базовый класс модели представления.
    /// </summary>
    /// <typeparam name="TVisualState">Визуальное состояние.</typeparam>
    public abstract class ViewModelBase<TVisualState> : DispatchedObjectBase, IViewModel<TVisualState>, IViewModelForegroundState where TVisualState : VisualState
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        protected ViewModelBase(CoreDispatcher dispatcher)
            :base(dispatcher)
        {
            _currentState = ViewModelState.Uninitialized;
            var o = new ViewModelOperationsCollection(dispatcher, this);
            Operations = o;
            OperationsScheduler = o;
        }

        private readonly Dictionary<Guid, IViewModelLifetimeCallback<TVisualState>> _lifetimeCallbacks = new Dictionary<Guid, IViewModelLifetimeCallback<TVisualState>>();

        /// <summary>
        /// Можно завершить работу.
        /// </summary>
        public virtual bool CanClose => true;

        /// <summary>
        /// Можно возобновить.
        /// </summary>
        public virtual bool CanResume => CurrentState == ViewModelState.Suspended;

        /// <summary>
        /// Можно приостановить.
        /// </summary>
        public virtual bool CanSuspend => CurrentState != ViewModelState.Suspended;

        private ViewModelState _currentState;

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        public ViewModelState CurrentState
        {
            get { return CheckThreadAccess(() => _currentState); }
            protected set
            {
                var oldState = _currentState;
                CheckThreadAccess();
                _currentState = value;
                StateChanged?.Invoke(this, new ViewModelStateChangedEventArgs(oldState, value));
                OnPropertyChanged(nameof(CanStart));
                OnPropertyChanged(nameof(CanResume));
                OnPropertyChanged(nameof(CanSuspend));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(IsClosed));
                OnPropertyChanged(nameof(IsInitialized));
            }
        }

        /// <summary>
        /// Активна.
        /// </summary>
        public bool IsActive => CurrentState == ViewModelState.Active;

        /// <summary>
        /// Производится работа.
        /// </summary>
        public bool IsBusy => CurrentState == ViewModelState.Busy;

        /// <summary>
        /// Работа модели завершена.
        /// </summary>
        public bool IsClosed => CurrentState == ViewModelState.Closed;

        /// <summary>
        /// Операции модели представления.
        /// </summary>
        public IViewModelOperationsCollection Operations { get; }

        /// <summary>
        /// Средство запуска операций.
        /// </summary>
        protected IViewModelOperationsScheduler OperationsScheduler { get; }

        /// <summary>
        /// Инициализирована.
        /// </summary>
        public bool IsInitialized => CurrentState != ViewModelState.Uninitialized;

        /// <summary>
        /// Работа модели приостановлена.
        /// </summary>
        public bool IsSuspended => CurrentState == ViewModelState.Suspended;

        public event EventHandler<ViewModelStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Завершить работу.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск, сигнализирующий о завершении работы.</returns>
        public virtual async Task Close(TVisualState visualState)
        {
            CheckThreadAccess();
            var callbacks = _lifetimeCallbacks.Values.Where(v => v != null).OrderByDescending(v => v.ToInactivePriority).ToArray();
            foreach (var c in callbacks)
            {
                await c.OnClose(this, visualState);
            }
            CurrentState = ViewModelState.Closed;
            await Operations.CancelAll();
        }

        /// <summary>
        /// Можно начинать работу с моделью.
        /// </summary>
        public virtual bool CanStart => CurrentState == ViewModelState.Uninitialized;

        /// <summary>
        /// Начать работу модели.
        /// </summary>
        /// <returns>Таск, сигнализирующий о начале работы.</returns>
        public virtual async Task Start()
        {
            CheckThreadAccess();

            if (CanStart)
            {
                var callbacks = _lifetimeCallbacks.Values.Where(v => v != null).OrderByDescending(v => v.ToActivePriority).ToArray();
                foreach (var c in callbacks)
                {
                    await c.OnStart(this);
                }

                CurrentState = ViewModelState.Active;
            }
        }

        /// <summary>
        /// Зарегистрировать обратный вызов.
        /// </summary>
        /// <param name="callback">Обратный вызов.</param>
        /// <returns>ID регистрации.</returns>
        public Guid RegisterLifetimeCallback(IViewModelLifetimeCallback<TVisualState> callback)
        {
            CheckThreadAccess();
            return DoRegisterLifetimeCallback(callback);
        }

        /// <summary>
        /// Зарегистрировать обратный вызов (без проверки доступа с UI-потока).
        /// </summary>
        /// <param name="callback">Обратный вызов.</param>
        /// <returns>ID регистрации.</returns>
        protected Guid DoRegisterLifetimeCallback(IViewModelLifetimeCallback<TVisualState> callback)
        {
            var id = Guid.NewGuid();
            _lifetimeCallbacks[id] = callback;
            return id;
        }

        /// <summary>
        /// Разрегистрировать обратный вызов.
        /// </summary>
        /// <param name="callbackId">ID регистрации.</param>
        public void UnregisterLifetimeCallback(Guid callbackId)
        {
            CheckThreadAccess();
            DoUnregisterLifetimeCallback(callbackId);
        }

        /// <summary>
        /// Разрегистрировать обратный вызов (без проверки доступа с UI-потока).
        /// </summary>
        /// <param name="callbackId">ID регистрации.</param>
        protected void DoUnregisterLifetimeCallback(Guid callbackId)
        {
            _lifetimeCallbacks.Remove(callbackId);
        }

        /// <summary>
        /// Возобновить.
        /// </summary>
        /// <returns>Таск, сигнализирующий о возобновлении работы.</returns>
        public virtual async Task Resume()
        {
            CheckThreadAccess();

            if (CanResume)
            {
                var callbacks = _lifetimeCallbacks.Values.Where(v => v != null).OrderByDescending(v => v.ToActivePriority).ToArray();
                foreach (var c in callbacks)
                {
                    await c.OnResume(this);
                }

                CurrentState = ViewModelState.Active;
            }
        }

        /// <summary>
        /// Приостановить.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск, сигнализирующий о приостановке.</returns>
        public virtual async Task Suspend(TVisualState visualState)
        {
            CheckThreadAccess();
            if (CanSuspend)
            {
                var callbacks = _lifetimeCallbacks.Values.Where(v => v != null).OrderByDescending(v => v.ToInactivePriority).ToArray();
                foreach (var c in callbacks)
                {
                    await c.OnSuspend(this, visualState);
                }
                CurrentState = ViewModelState.Suspended;
                await Operations.CancelAllOnSuspend();
            }
        }

        /// <summary>
        /// Получить сосотояние активности (готовоности к выполнению основной операции).
        /// </summary>
        /// <returns>Состояние.</returns>
        bool IViewModelForegroundState.GetActiveState()
        {
            return CurrentState == ViewModelState.Active;
        }

        /// <summary>
        /// Установить, что занято.
        /// </summary>
        /// <param name="isBusy">Флаг занятости.</param>
        void IViewModelForegroundState.SetBusyState(bool isBusy)
        {
            if (isBusy)
            {
                if (CurrentState == ViewModelState.Active)
                {
                    CurrentState = ViewModelState.Busy;
                }
            }
            else
            {
                if (CurrentState == ViewModelState.Busy)
                {
                    CurrentState = ViewModelState.Active;
                }
            }
        }
    }
}