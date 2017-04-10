namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Состояние модели представления.
    /// </summary>
    public enum ViewModelState
    {
        /// <summary>
        /// Не инициализировано.
        /// </summary>
        Uninitialized,

        /// <summary>
        /// Готово к использованию.
        /// </summary>
        Active,

        /// <summary>
        /// Производится работа.
        /// </summary>
        Busy,

        /// <summary>
        /// Модель приостановлена.
        /// </summary>
        Suspended,

        /// <summary>
        /// Завершена.
        /// </summary>
        Closed
    }
}