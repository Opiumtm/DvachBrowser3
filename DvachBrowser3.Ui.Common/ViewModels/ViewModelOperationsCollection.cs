using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Коллекция операций модели представления.
    /// </summary>
    public class ViewModelOperationsCollection : DispatchedObjectBase, IViewModelOperationsCollection, IViewModelOperationsScheduler
    {
        /// <summary>
        /// Семафор операций моделей предсталвения.
        /// </summary>
        private static readonly SemaphoreSlim BackgroundOperationsSemaphore = new SemaphoreSlim(CommonUiConsts.MaxBackgroundTasks);

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        /// <param name="foregroundState">Состояние основной операции.</param>
        public ViewModelOperationsCollection(CoreDispatcher dispatcher, IViewModelForegroundState foregroundState = null) : base(dispatcher)
        {
            _foregroundState = foregroundState;
            ActiveOperations.CollectionChanged += ActiveOperationsOnCollectionChanged;
        }

        private async void ActiveOperationsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            try
            {
                await Dispatcher.DispatchAction(() =>
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

        private readonly IViewModelForegroundState _foregroundState;

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
        /// Отменить все операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий о завершении.</returns>
        public virtual async Task CancelAll()
        {
            CheckThreadAccess();
            var operations = ActiveOperations.ToArray().Concat(new[] { CurrentOperation }).Where(o => o?.CanCancel ?? false).Distinct();
            await Task.WhenAll(operations.Select(o => o.Cancel()));
        }

        /// <summary>
        /// Отменить все операции в случае приостановки модели представления.
        /// </summary>
        /// <returns>Таск, сигнализирующий о завершении.</returns>
        public virtual async Task CancelAllOnSuspend()
        {
            CheckThreadAccess();
            var operations = ActiveOperations.ToArray().Concat(new[] { CurrentOperation }).Where(o => (o?.CanCancel ?? false) && (o?.CancelOnSuspend ?? true)).Distinct();
            await Task.WhenAll(operations.Select(o => o.Cancel()));
        }

        /// <summary>
        /// Запустить основную операцию (может быть только одна одновременно).
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="operation">Операция.</param>
        /// <returns>Таск выполнения операции.</returns>
        public async Task<T> QueueForegroundOperation<T>(ViewModelOperationBase<T> operation)
        {
            if (_foregroundState == null)
            {
                throw new InvalidOperationException("Не поддерживается запуск основных операций, так как не настроен индикатор состояния модели представления");
            }
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            CheckThreadAccess();
            if (!_foregroundState.GetActiveState())
            {
                throw new InvalidOperationException("Попытка запустить основную операцию в неправильном состоянии модели");
            }
            _foregroundState.SetBusyState(true);
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
                await Dispatcher.DispatchAction(() =>
                {
                    _foregroundState.SetBusyState(false);
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
        public async Task<T> QueueBackgroundOperation<T>(ViewModelOperationBase<T> operation, bool bypassThrottle = false)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            CheckThreadAccess();
            operation.StateChanged += OperationOnStateChanged;
            try
            {
                if (!ActiveOperations.Contains(operation))
                {
                    ActiveOperations.Add(operation);
                }
                if (!bypassThrottle)
                {
                    await BackgroundOperationsSemaphore.WaitAsync();
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
                        BackgroundOperationsSemaphore.Release();
                    }
                }
            }
            finally
            {
                operation.StateChanged -= OperationOnStateChanged;
                await RemoveOperation(operation);
            }
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

        private async Task RemoveOperation(IViewModelOperation obj)
        {
            await Dispatcher.DispatchAction(() =>
            {
                ActiveOperations.Remove(obj);
                if (CurrentOperation == obj)
                {
                    CurrentOperation = null;
                }
            });
        }
    }
}