using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Базовый класс модели представления.
    /// </summary>
    /// <typeparam name="TVisualState">Визуальное состояние.</typeparam>
    public abstract class ViewModelBase<TVisualState> : IViewModel<TVisualState> where TVisualState : VisualState
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        protected ViewModelBase(CoreDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            _currentState = ViewModelState.Uninitialized;
            ActiveOperations.CollectionChanged += ActiveOperationsOnCollectionChanged;
        }

        private async void ActiveOperationsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            try
            {
                await DispatchAccess(() =>
                {
                    OnPropertyChanged(nameof(CanCancel));
                    OnPropertyChanged(nameof(AnyActiveWork));
                });
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Диспетчер.
        /// </summary>
        protected CoreDispatcher Dispatcher { get; }

        /// <summary>
        /// Проверить, производится ли доступ из UI-потока.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CheckThreadAccess()
        {
            if (Dispatcher != null)
            {
                if (!Dispatcher.HasThreadAccess)
                {
                    throw new InvalidOperationException("Доступ к члену класса не из UI-потока");
                }
            }
        }

        /// <summary>
        /// Получить значение, предварительно проверив доступ из UI-потока.
        /// </summary>
        /// <typeparam name="T">Тип значения.</typeparam>
        /// <param name="getValue">Функция получения значения.</param>
        /// <returns>Результат.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T CheckThreadAccess<T>(Func<T> getValue)
        {
            if (getValue == null) throw new ArgumentNullException(nameof(getValue));
            CheckThreadAccess();
            return getValue();
        }

        /// <summary>
        /// Выполнить действие на UI-потоке.
        /// </summary>
        /// <param name="action">Действие.</param>
        protected Task DispatchAccess(Action action)
        {
            if (Dispatcher != null && !Dispatcher.HasThreadAccess)
            {
                return Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    action?.Invoke();
                }).AsTask();
            }
            try
            {
                action?.Invoke();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        /// <summary>
        /// Активные операции.
        /// </summary>
        public ObservableCollection<IViewModelOperation> ActiveOperations { get; } = new ObservableCollection<IViewModelOperation>();

        /// <summary>
        /// Есть какая-то активная работа (основная или фоновая).
        /// </summary>
        public bool AnyActiveWork => CheckThreadAccess(() => ActiveOperations.Any());

        /// <summary>
        /// Можно отменять.
        /// </summary>
        public virtual bool CanCancel => CheckThreadAccess(() => ActiveOperations.Any() || (CurrentOperation?.CanCancel ?? false));

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

        private IViewModelOperation _currentOperation;

        /// <summary>
        /// Текущая операция.
        /// </summary>
        public IViewModelOperation CurrentOperation
        {
            get { return CheckThreadAccess(() => _currentOperation); }
            protected set
            {
                CheckThreadAccess();
                _currentOperation = value;
                OnPropertyChanged(nameof(CurrentOperation));
                OnPropertyChanged(nameof(CanCancel));
                OnPropertyChanged(nameof(AnyActiveWork));
            }
        }

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
        /// Инициализирована.
        /// </summary>
        public bool IsInitialized => CurrentState != ViewModelState.Uninitialized;

        /// <summary>
        /// Работа модели приостановлена.
        /// </summary>
        public bool IsSuspended => CurrentState == ViewModelState.Suspended;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ViewModelStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Отменить все операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий о завершении.</returns>
        public virtual async Task CancelAll()
        {
            CheckThreadAccess();
            var operations = ActiveOperations.ToArray().Concat(new[] {CurrentOperation}).Where(o => o?.CanCancel ?? false).Distinct();
            await Task.WhenAll(operations.Select(o => o.Cancel()));
        }

        /// <summary>
        /// Завершить работу.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск, сигнализирующий о завершении работы.</returns>
        public virtual async Task Close(TVisualState visualState)
        {
            CheckThreadAccess();
            CurrentState = ViewModelState.Closed;
            await CancelAll();
        }

        /// <summary>
        /// Можно начинать работу с моделью.
        /// </summary>
        public virtual bool CanStart => CurrentState == ViewModelState.Uninitialized;

        /// <summary>
        /// Начать работу модели.
        /// </summary>
        /// <returns>Таск, сигнализирующий о начале работы.</returns>
        public virtual Task Start()
        {
            CheckThreadAccess();
            if (CanStart)
            {
                CurrentState = ViewModelState.Active;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Найти операцию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Операция (null, если не найдено).</returns>
        public virtual IViewModelOperation FindOperationById(int id)
        {
            CheckThreadAccess();
            if (CurrentOperation?.Id == id)
            {
                return CurrentOperation;
            }
            return ActiveOperations.ToArray().FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// Возобновить.
        /// </summary>
        /// <returns>Таск, сигнализирующий о возобновлении работы.</returns>
        public virtual Task Resume()
        {
            CheckThreadAccess();
            if (CanResume)
            {
                CurrentState = ViewModelState.Active;
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Приостановить.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск, сигнализирующий о приостановке.</returns>
        public async Task Suspend(TVisualState visualState)
        {
            CheckThreadAccess();
            if (CanSuspend)
            {
                CurrentState = ViewModelState.Suspended;
            }
            else
            {
                return;
            }
            await CancelAll();
        }

        /// <summary>
        /// Свойство изменилось.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Запустить основную операцию (может быть только одна одновременно).
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="operation">Операция.</param>
        /// <returns>Таск выполнения операции.</returns>
        protected async Task<T> QueueForegroundOperation<T>(ViewModelOperationBase<T> operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            CheckThreadAccess();
            if (CurrentState != ViewModelState.Active)
            {
                throw new InvalidOperationException("Попытка запустить основную операцию в неправильном состоянии модели");
            }
            CurrentState = ViewModelState.Busy;
            try
            {
                operation.StateChanged += OperationOnStateChanged;
                try
                {
                    CurrentOperation = operation;
                    if (!ActiveOperations.Contains(operation))
                    {
                        ActiveOperations.Add(operation);
                    }
                    return await operation.StartTracking2();
                }
                finally
                {
                    operation.StateChanged -= OperationOnStateChanged;
                    await RemoveOperation(operation);
                }
            }
            finally
            {
                await DispatchAccess(() =>
                {
                    if (CurrentState == ViewModelState.Busy)
                    {
                        CurrentState = ViewModelState.Active;
                    }
                });
            }
        }

        /// <summary>
        /// Запустить фоновую операцию.
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="operation">Операция.</param>
        /// <param name="bypassThrottle">Не учитывать ограничение на максимальное количество операций.</param>
        /// <returns>Таск выполнения операции.</returns>
        protected async Task<T> QueueBackgroundOperation<T>(ViewModelOperationBase<T> operation, bool bypassThrottle = false)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            CheckThreadAccess();
            if (CurrentState != ViewModelState.Active)
            {
                throw new InvalidOperationException("Попытка запустить основную операцию в неправильном состоянии модели");
            }
            operation.StateChanged += OperationOnStateChanged;
            try
            {
                if (!ActiveOperations.Contains(operation))
                {
                    ActiveOperations.Add(operation);
                }
                if (!bypassThrottle)
                {
                    await ViewModelBaseSemaphoreHolder.BackgroundOperationsSemaphore.WaitAsync();
                }
                try
                {
                    if (!operation.IsCancelled)
                    {
                        return await operation.StartTracking2();
                    }
                    else
                    {
                        throw new OperationCanceledException();
                    }
                }
                finally
                {
                    if (!bypassThrottle)
                    {
                        ViewModelBaseSemaphoreHolder.BackgroundOperationsSemaphore.Release();
                    }
                }
            }
            finally
            {
                operation.StateChanged -= OperationOnStateChanged;
                await RemoveOperation(operation);
            }
        }

        private async Task RemoveOperation(IViewModelOperation obj)
        {
            await DispatchAccess(() =>
            {
                ActiveOperations.Remove(obj);
                if (CurrentOperation == obj)
                {
                    CurrentOperation = null;
                }
            });
        }

        private async void OperationOnStateChanged(object sender, ViewModelOperationStateChangedEventArgs e)
        {
            try
            {
                var obj = sender as IViewModelOperation;
                if (obj != null)
                {
                    if (e.NewState == ViewModelOperationState.Finished || e.NewState == ViewModelOperationState.Failed || e.NewState == ViewModelOperationState.Cancelled)
                    {
                        await RemoveOperation(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }
    }

    internal static class ViewModelBaseSemaphoreHolder
    {
        public static readonly SemaphoreSlim BackgroundOperationsSemaphore = new SemaphoreSlim(CommonUiConsts.MaxBackgroundTasks);
    }
}