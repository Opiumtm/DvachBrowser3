using System;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель прогресса операции.
    /// </summary>
    public interface IOperationProgressViewModel
    {
        /// <summary>
        /// Прогресс.
        /// </summary>
        double Progress { get; }

        /// <summary>
        /// Неявное положение.
        /// </summary>
        bool IsIndeterminate { get; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Активен.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Ошибка.
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Ошибка.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Отменено.
        /// </summary>
        bool IsCancelled { get; }

        /// <summary>
        /// Запущено.
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// Прогресс изменился.
        /// </summary>
        event EventHandler ProgressChanged;

        /// <summary>
        /// Завершено.
        /// </summary>
        event OperationProgressFinishedEventHandler Finished;
    }
}