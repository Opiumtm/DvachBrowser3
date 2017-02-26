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
    public interface IViewModel : INotifyPropertyChanged
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
        bool IsDisposed { get; }

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
        /// <returns>Таск, сигнализирующий о приостановке.</returns>
        Task Suspend();

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
        /// <returns>Таск, сигнализирующий о завершении работы.</returns>
        Task Close();
    }
}