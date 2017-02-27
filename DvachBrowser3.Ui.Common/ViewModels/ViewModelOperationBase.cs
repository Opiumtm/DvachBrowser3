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
    public abstract class ViewModelOperationBase : IViewModelOperation
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="dispatcher">Диспетчер.</param>
        protected ViewModelOperationBase(int id, CoreDispatcher dispatcher)
        {
            Id = id;
            Dispatcher = dispatcher;
            _progress = new ViewModelOperationProgress(id, ViewModelOperationState.Uninitialized, null, null, null, null);
        }

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Идентификатор операции.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Диспетчер.
        /// </summary>
        protected CoreDispatcher Dispatcher { get; private set; }

        /// <summary>
        /// Вызвать действие на UI-потоке, если это возможно.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <returns>Таск.</returns>
        protected Task DispatchAction(Action action)
        {
            if (Dispatcher != null && !Dispatcher.HasThreadAccess)
            {
                return Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke()).AsTask();
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
        public bool CanCancel => CancelAction != null;

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
            return DispatchAction(() =>
            {
                OnPropertyChanged(nameof(CanCancel));
            });
        }

        /// <summary>
        /// Отменить операцию.
        /// </summary>
        /// <returns>Таск, сигнализирующий о завершении.</returns>
        public async Task Cancel()
        {
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
        public ViewModelOperationProgress Progress => Interlocked.CompareExchange(ref _progress, null, null);

        /// <summary>
        /// Прогресс изменился.
        /// </summary>
        public event EventHandler<ViewModelOperationProgressEventArgs> ProgressChanged;

        /// <summary>
        /// Обновить прогресс операции.
        /// </summary>
        /// <param name="state">Состояние.</param>
        /// <param name="progress">Прогресс.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="error">Ошибка.</param>
        /// <param name="otherData">Другие данные.</param>
        /// <returns>Таск.</returns>
        protected Task UpdateProgress(ViewModelOperationState state, double? progress, string message, Exception error,
            object otherData)
        {
            var newProgress = new ViewModelOperationProgress(Id, state, progress, message, error, otherData);
            var oldProgress = Interlocked.Exchange(ref _progress, newProgress);
            var oldState = oldProgress?.State ?? ViewModelOperationState.Uninitialized;
            return DispatchAction(() =>
            {
                StateChanged?.Invoke(this, new ViewModelOperationStateChangedEventArgs(oldState, state));
                ProgressChanged?.Invoke(this, new ViewModelOperationProgressEventArgs(newProgress));
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(CurrentState));
                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(IsCancelled));
                OnPropertyChanged(nameof(IsFailed));
                OnPropertyChanged(nameof(IsFinished));
                OnPropertyChanged(nameof(IsInitialized));
            });
        }

        /// <summary>
        /// Обновить прогресс.
        /// </summary>
        /// <param name="state">Состояние.</param>
        /// <returns>Таск.</returns>
        protected Task UpdateProgress(ViewModelOperationState state)
        {
            return UpdateProgress(state, null, null, null, null);
        }

        /// <summary>
        /// Обновить прогресс.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <returns>Таск.</returns>
        protected Task UpdateProgress(Exception error)
        {
            return UpdateProgress(ViewModelOperationState.Failed, null, null, error, null);
        }

        /// <summary>
        /// Состояние свойства изменилось.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>Таск.</returns>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Начать отслеживание прогресса операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий завершение операции.</returns>
        public abstract Task StartTracking();
    }
}