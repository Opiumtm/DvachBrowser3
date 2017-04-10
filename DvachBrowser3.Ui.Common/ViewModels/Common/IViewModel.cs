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
    public interface IViewModel<TVisualState> : INotifyPropertyChanged where TVisualState : VisualState
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
        /// Операции модели представления.
        /// </summary>
        IViewModelOperationsCollection Operations { get; }

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

        /// <summary>
        /// Можно начинать работу с моделью.
        /// </summary>
        bool CanStart { get; }

        /// <summary>
        /// Начать работу модели.
        /// </summary>
        /// <returns>Таск, сигнализирующий о начале работы.</returns>
        Task Start();

        /// <summary>
        /// Зарегистрировать обратный вызов.
        /// </summary>
        /// <param name="callback">Обратный вызов.</param>
        /// <returns>ID регистрации.</returns>
        Guid RegisterLifetimeCallback(IViewModelLifetimeCallback<TVisualState> callback);

        /// <summary>
        /// Разрегистрировать обратный вызов.
        /// </summary>
        /// <param name="callbackId">ID регистрации.</param>
        void UnregisterLifetimeCallback(Guid callbackId);
    }
}