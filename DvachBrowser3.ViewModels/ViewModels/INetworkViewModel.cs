using System;
using System.Threading;
using System.Windows.Input;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель, поддерживающая сетевую загрузку.
    /// </summary>
    public interface INetworkViewModel : ICommand
    {
        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        void ExecuteOperation();

        /// <summary>
        /// Операция выполняется.
        /// </summary>
        bool IsExecuting { get; }

        /// <summary>
        /// Есть ошибка.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Операция завершена успешно.
        /// </summary>
        bool IsOk { get; }

        /// <summary>
        /// Можно выполнять.
        /// </summary>
        bool IsCanExecute { get; }

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        event EventHandler<EngineProgress> Progress;

        /// <summary>
        /// Ошибка.
        /// </summary>
        event EventHandler Error;

        /// <summary>
        /// Завершено.
        /// </summary>
        event EventHandler Completed;

        /// <summary>
        /// Начато.
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// Текст ошибки.
        /// </summary>
        string ErrorText { get; }
    }

    /// <summary>
    /// Модель, поддерживающая сетевую загрузку.
    /// </summary>
    /// <typeparam name="TArg">Тип аргумента.</typeparam>
    public interface INetworkViewModel<in TArg> : INetworkViewModel
    {
        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="arg">Аргумент.</param>
        void ExecuteOperation(TArg arg);
    }
}