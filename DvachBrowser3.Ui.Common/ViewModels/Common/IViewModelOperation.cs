using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Операция модели представления.
    /// </summary>
    public interface IViewModelOperation : INotifyPropertyChanged
    {
        /// <summary>
        /// Идентификатор операции.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        ViewModelOperationState CurrentState { get; }

        /// <summary>
        /// Состояние изменилось.
        /// </summary>
        event EventHandler<ViewModelOperationStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Можно отменить.
        /// </summary>
        bool CanCancel { get; }

        /// <summary>
        /// Отменять на приостановке модели.
        /// </summary>
        bool CancelOnSuspend { get; }

        /// <summary>
        /// Отменить операцию.
        /// </summary>
        /// <returns>Таск, сигнализирующий о завершении.</returns>
        Task Cancel();

        /// <summary>
        /// Инициализирована.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Операция активна.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Операция завершена с ошибкой.
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// Завершена успешно.
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Отменено.
        /// </summary>
        bool IsCancelled { get; }

        /// <summary>
        /// Ошибка (если есть).
        /// </summary>
        Exception Error { get; }

        /// <summary>
        /// Произошла ошибка.
        /// </summary>
        event EventHandler<Exception> Failed;

        /// <summary>
        /// Текущий прогресс операции.
        /// </summary>
        ViewModelOperationProgress Progress { get; }

        /// <summary>
        /// Прогресс изменился.
        /// </summary>
        event EventHandler<ViewModelOperationProgressEventArgs> ProgressChanged;
    }

    /// <summary>
    /// Операция модели представления.
    /// </summary>
    public interface IViewModelOperation<T> : IViewModelOperation
    {
        /// <summary>
        /// Результат операции.
        /// </summary>
        T Result { get; }

        /// <summary>
        /// Прогресс с данными о результате.
        /// </summary>
        ViewModelOperationProgress<T> Progress2 { get; }

        /// <summary>
        /// Прогресс изменился.
        /// </summary>
        event EventHandler<ViewModelOperationProgressEventArgs<T>> ProgressChanged2;
    }
}