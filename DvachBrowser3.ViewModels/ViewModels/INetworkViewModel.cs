using System;
using System.Threading;
using DvachBrowser3.Engines;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель, поддерживающая сетевую загрузку.
    /// </summary>
    public interface INetworkViewModel
    {
        /// <summary>
        /// Выполнить операцию.
        /// </summary>
        /// <param name="token">Токен отмены.</param>
        void ExecuteOperation(CancellationToken token);

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
        bool CanExecute { get; }

        /// <summary>
        /// Прогресс операции.
        /// </summary>
        event EventHandler<EngineProgress> Progress;

        /// <summary>
        /// Текст ошибки.
        /// </summary>
        string ErrorText { get; }
    }
}