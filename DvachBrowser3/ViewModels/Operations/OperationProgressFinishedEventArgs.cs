using System;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Аргументы завершения операции.
    /// </summary>
    public class OperationProgressFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <param name="isCancelled">Отменено.</param>
        public OperationProgressFinishedEventArgs(Exception error = null, bool isCancelled = false)
        {
            Error = error;
            IsCancelled = isCancelled;
        }

        /// <summary>
        /// Завершено удачно.
        /// </summary>
        public bool IsSuccessful => Error == null && !IsCancelled;

        /// <summary>
        /// Ошибка.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Отменено.
        /// </summary>
        public bool IsCancelled { get; private set; }
    }
}