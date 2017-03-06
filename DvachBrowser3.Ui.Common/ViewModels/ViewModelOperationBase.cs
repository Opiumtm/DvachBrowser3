using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Базовый класс операции модели представления.
    /// </summary>
    public abstract class ViewModelOperationBase : DispatchedObjectBase, IViewModelOperation
    {
        private static int _idCounter = 0;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        protected ViewModelOperationBase(CoreDispatcher dispatcher)
            :base(dispatcher)
        {
            Id = Interlocked.Increment(ref _idCounter);
        }

        /// <summary>
        /// Идентификатор операции.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        public ViewModelOperationState CurrentState => Progress.State;

        /// <summary>
        /// Состояние изменилось.
        /// </summary>
        public event EventHandler<ViewModelOperationStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Можно отменить.
        /// </summary>
        public bool CanCancel => CurrentState == ViewModelOperationState.Uninitialized || CancelAction != null;

        private int _cancelOnSuspend = 1;

        /// <summary>
        /// Отменять на приостановке модели.
        /// </summary>
        public bool CancelOnSuspend
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Interlocked.CompareExchange(ref _cancelOnSuspend, 0, 0) != 0; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { _cancelOnSuspend = Interlocked.Exchange(ref _cancelOnSuspend, value ? 1 : 0); }
        }

        /// <summary>
        /// Действие по остановке операции.
        /// </summary>
        private Func<Task> _cancelAction;

        /// <summary>
        /// Действие по остановке операции.
        /// </summary>
        protected Func<Task> CancelAction => Interlocked.CompareExchange(ref _cancelAction, null, null);

        /// <summary>
        /// Установить действие по остановке операции.
        /// </summary>
        /// <param name="value">Дейтсвие.</param>
        /// <returns>Таск.</returns>
        protected Task SetCancelAction(Func<Task> value)
        {
            Interlocked.Exchange(ref _cancelAction, value);
            return Dispatcher.DispatchAction(() =>
            {
                OnPropertyChanged(nameof(CanCancel));
            });
        }

        /// <summary>
        /// Отменить операцию.
        /// </summary>
        /// <returns>Таск, сигнализирующий о завершении.</returns>
        public virtual async Task Cancel()
        {
            if (CurrentState == ViewModelOperationState.Uninitialized)
            {
                await UpdateProgress(ViewModelOperationState.Cancelled);
                return;
            }
            var action = CancelAction;
            if (action == null)
            {
                return;
            }
            await action();
        }

        /// <summary>
        /// Инициализирована.
        /// </summary>
        public bool IsInitialized => CurrentState != ViewModelOperationState.Uninitialized;

        /// <summary>
        /// Операция активна.
        /// </summary>
        public bool IsActive => CurrentState == ViewModelOperationState.Active;

        /// <summary>
        /// Операция завершена с ошибкой.
        /// </summary>
        public bool IsFailed => CurrentState == ViewModelOperationState.Failed;

        /// <summary>
        /// Завершена успешно.
        /// </summary>
        public bool IsFinished => CurrentState == ViewModelOperationState.Finished;

        /// <summary>
        /// Отменено.
        /// </summary>
        public bool IsCancelled => CurrentState == ViewModelOperationState.Cancelled;

        /// <summary>
        /// Ошибка (если есть).
        /// </summary>
        public Exception Error => Progress.Error;

        /// <summary>
        /// Произошла ошибка.
        /// </summary>
        public event EventHandler<Exception> Failed;

        private ViewModelOperationProgress _progress;

        /// <summary>
        /// Текущий прогресс операции.
        /// </summary>
        public ViewModelOperationProgress Progress => Interlocked.CompareExchange(ref _progress, null, null) ?? CreateProgress(Id, ViewModelOperationState.Uninitialized, null, null, null, null, null);

        /// <summary>
        /// Прогресс изменился.
        /// </summary>
        public event EventHandler<ViewModelOperationProgressEventArgs> ProgressChanged;

        /// <summary>
        /// Создать объект прогресса операции.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="state">Состояние.</param>
        /// <param name="progress">Прогресс.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="error">Ошибка.</param>
        /// <param name="otherData">Прочие данные.</param>
        /// <param name="result">Результат.</param>
        /// <returns>Объект прогресса операции.</returns>
        protected virtual ViewModelOperationProgress CreateProgress(int id, ViewModelOperationState state, double? progress, string message, Exception error,
            object otherData, object result)
        {
            return new ViewModelOperationProgress(id, state, progress, message, error, otherData);
        }

        /// <summary>
        /// Обновить прогресс операции.
        /// </summary>
        /// <param name="state">Состояние.</param>
        /// <param name="progress">Прогресс.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="error">Ошибка.</param>
        /// <param name="otherData">Другие данные.</param>
        /// <param name="result">Результат.</param>
        /// <returns>Таск.</returns>
        protected Task UpdateProgress(ViewModelOperationState state, double? progress, string message, Exception error,
            object otherData, object result)
        {
            var newProgress = CreateProgress(Id, state, progress, message, error, otherData, result);
            var oldProgress = Interlocked.Exchange(ref _progress, newProgress);
            return Dispatcher.DispatchAction(() =>
            {
                TriggerProgressEvents(oldProgress, newProgress);
            });
        }

        protected virtual void TriggerProgressEvents(ViewModelOperationProgress oldProgress, ViewModelOperationProgress newProgress)
        {
            var oldState = oldProgress?.State ?? ViewModelOperationState.Uninitialized;
            var state = newProgress?.State ?? ViewModelOperationState.Uninitialized;
            StateChanged?.Invoke(this, new ViewModelOperationStateChangedEventArgs(oldState, state));
            ProgressChanged?.Invoke(this, new ViewModelOperationProgressEventArgs(newProgress));
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(CurrentState));
            OnPropertyChanged(nameof(IsActive));
            OnPropertyChanged(nameof(IsCancelled));
            OnPropertyChanged(nameof(IsFailed));
            OnPropertyChanged(nameof(IsFinished));
            OnPropertyChanged(nameof(IsInitialized));
        }

        /// <summary>
        /// Обновить прогресс.
        /// </summary>
        /// <param name="state">Состояние.</param>
        /// <returns>Таск.</returns>
        protected Task UpdateProgress(ViewModelOperationState state)
        {
            return UpdateProgress(state, null, null, null, null, null);
        }

        /// <summary>
        /// Обновить прогресс.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns>Таск.</returns>
        protected Task UpdateProgress(Exception error)
        {
            return UpdateProgress(ViewModelOperationState.Failed, null, null, error, null, null);
        }

        /// <summary>
        /// Начать отслеживание прогресса операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий завершение операции.</returns>
        public abstract Task StartTracking();
    }
    
    /// <summary>
    /// Базовый класс операции модели представления.
    /// </summary>
    /// <typeparam name="T">Результат операции.</typeparam>
    public abstract class ViewModelOperationBase<T> : ViewModelOperationBase, IViewModelOperation<T>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dispatcher">Диспетчер.</param>
        protected ViewModelOperationBase(CoreDispatcher dispatcher)
            :base(dispatcher)
        {
        }

        /// <summary>
        /// Результат операции.
        /// </summary>
        public T Result
        {
            get
            {
                var p = Progress2;
                return p != null ? p.Result : default(T);
            }
        }

        protected override void TriggerProgressEvents(ViewModelOperationProgress oldProgress, ViewModelOperationProgress newProgress)
        {
            base.TriggerProgressEvents(oldProgress, newProgress);
            ProgressChanged2?.Invoke(this, new ViewModelOperationProgressEventArgs<T>(newProgress as ViewModelOperationProgress<T>));
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(Progress2));
        }

        /// <summary>
        /// Прогресс с данными о результате.
        /// </summary>
        public ViewModelOperationProgress<T> Progress2 => Progress as ViewModelOperationProgress<T>;

        /// <summary>
        /// Прогресс изменился.
        /// </summary>
        public event EventHandler<ViewModelOperationProgressEventArgs<T>> ProgressChanged2;

        protected override ViewModelOperationProgress CreateProgress(int id, ViewModelOperationState state, double? progress, string message, Exception error, object otherData, object result)
        {
            T r;
            if (result == null)
            {
                r = default(T);
            }
            else if (result is T)
            {
                r = (T) result;
            }
            else
            {
                r = default(T);
            }
            return new ViewModelOperationProgress<T>(id, state, progress, message, error, otherData, r);
        }

        /// <summary>
        /// Начать отслеживание прогресса операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий завершение операции.</returns>
        public abstract Task<T> StartTracking2();

        /// <summary>
        /// Начать отслеживание прогресса операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий завершение операции.</returns>
        public sealed override Task StartTracking()
        {
            return StartTracking2();
        }

        /// <summary>
        /// Обновить прогресс.
        /// </summary>
        /// <param name="result">Результат.</param>
        /// <returns>Таск.</returns>
        protected Task UpdateProgress(T result)
        {
            return UpdateProgress(ViewModelOperationState.Finished, null, null, null, null, result);
        }
    }
}