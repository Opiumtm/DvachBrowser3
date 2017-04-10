namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Состояние операции модели представления.
    /// </summary>
    public enum ViewModelOperationState
    {
        /// <summary>
        /// Не инициализировано.
        /// </summary>
        Uninitialized,

        /// <summary>
        /// Производится операция.
        /// </summary>
        Active,

        /// <summary>
        /// Операция завершена.
        /// </summary>
        Finished,

        /// <summary>
        /// Произошла ошибка.
        /// </summary>
        Failed,

        /// <summary>
        /// Операция отменена.
        /// </summary>
        Cancelled
    }
}