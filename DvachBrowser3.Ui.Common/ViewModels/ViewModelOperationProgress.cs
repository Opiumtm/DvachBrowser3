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
        /// <param name="otherData">Дополнительные данные.</param>
        public ViewModelOperationProgress(int operationId, ViewModelOperationState state, double? progress, string message, object otherData)
        {
            OperationId = operationId;
            Progress = progress;
            Message = message;
            State = state;
            OtherData = otherData;
        }

        /// <summary>
        /// Идентификатор операции.
        /// </summary>
        public int OperationId { get; private set; }

        /// <summary>
        /// Прогресс операции (от 0 до 1).
        /// </summary>
        public double? Progress { get; private set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Прогресс активен.
        /// </summary>
        public bool IsActive => State == ViewModelOperationState.Active;

        /// <summary>
        /// Дополнительные данные.
        /// </summary>
        public object OtherData { get; private set; }

        /// <summary>
        /// Состояние операции.
        /// </summary>
        public ViewModelOperationState State { get; private set; }
    }
}