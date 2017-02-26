using System;
using System.ComponentModel;
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
        /// Ошибка (если есть).
        /// </summary>
        Exception Error { get; }

        /// <summary>
        /// Текущий прогресс операции.
        /// </summary>
        ViewModelOperationProgress Progress { get; }

        /// <summary>
        /// Прогресс изменился.
        /// </summary>
        event EventHandler<ViewModelOperationProgressEventArgs> ProgressChanged;
    }
}