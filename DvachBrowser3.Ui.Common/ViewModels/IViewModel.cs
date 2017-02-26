using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Модель представления.
    /// </summary>
    /// <typeparam name="TVisualState">Тип визуального состояния.</typeparam>
    public interface IViewModel<in TVisualState> : INotifyPropertyChanged where TVisualState : VisualState
    {
        /// <summary>
        /// Текущее состояние.
        /// </summary>
        ViewModelState CurrentState { get; }

        /// <summary>
        /// Состояние изменилось.
        /// </summary>
        event EventHandler<ViewModelStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Инициализирована.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Активна.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Производится работа.
        /// </summary>
        bool IsBusy { get; }

        /// <summary>
        /// Работа модели приостановлена.
        /// </summary>
        bool IsSuspended { get; }

        /// <summary>
        /// Работа модели завершена.
        /// </summary>
        bool IsClosed { get; }

        /// <summary>
        /// Активные операции.
        /// </summary>
        ObservableCollection<IViewModelOperation> ActiveOperations { get; }

        /// <summary>
        /// Текущая операция.
        /// </summary>
        IViewModelOperation CurrentOperation { get; }

        /// <summary>
        /// Найти операцию по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Операция (null, если не найдено).</returns>
        IViewModelOperation FindOperationById(int id);

        /// <summary>
        /// Можно отменять.
        /// </summary>
        bool CanCancel { get; }

        /// <summary>
        /// Отменить все операции.
        /// </summary>
        /// <returns>Таск, сигнализирующий о завершении.</returns>
        Task CancelAll();

        /// <summary>
        /// Можно приостановить.
        /// </summary>
        bool CanSuspend { get; }

        /// <summary>
        /// Приостановить.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск, сигнализирующий о приостановке.</returns>
        Task Suspend(TVisualState visualState);

        /// <summary>
        /// Можно возобновить.
        /// </summary>
        bool CanResume { get; }

        /// <summary>
        /// Возобновить.
        /// </summary>
        /// <returns>Таск, сигнализирующий о возобновлении работы.</returns>
        Task Resume();

        /// <summary>
        /// Можно завершить работу.
        /// </summary>
        bool CanClose { get; }

        /// <summary>
        /// Завершить работу.
        /// </summary>
        /// <param name="visualState">Визуальное состояние.</param>
        /// <returns>Таск, сигнализирующий о завершении работы.</returns>
        Task Close(TVisualState visualState);
    }
}