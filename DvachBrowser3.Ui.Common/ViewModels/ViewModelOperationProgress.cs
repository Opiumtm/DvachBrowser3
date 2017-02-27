using System;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Прогресс операции модели представления.
    /// </summary>
    public class ViewModelOperationProgress
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="operationId">Идентификатор операции.</param>
        /// <param name="state">Состояние операции.</param>
        /// <param name="progress">Прогресс операции (от 0 до 1).</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="error">Ошибка.</param>
        /// <param name="otherData">Дополнительные данные.</param>
        public ViewModelOperationProgress(int operationId, ViewModelOperationState state, double? progress, string message, Exception error, object otherData)
        {
            OperationId = operationId;
            State = state;
            Progress = progress;
            Message = message;
            Error = error;
            OtherData = otherData;
        }

        /// <summary>
        /// Идентификатор операции.
        /// </summary>
        public int OperationId { get; }

        /// <summary>
        /// Прогресс операции (от 0 до 1).
        /// </summary>
        public double? Progress { get; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Прогресс активен.
        /// </summary>
        public bool IsActive => State == ViewModelOperationState.Active;

        /// <summary>
        /// Дополнительные данные.
        /// </summary>
        public object OtherData { get; }

        /// <summary>
        /// Состояние операции.
        /// </summary>
        public ViewModelOperationState State { get; }

        /// <summary>
        /// Ошибка.
        /// </summary>
        public Exception Error { get; }
    }
}