namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Операция.
    /// </summary>
    public interface IOperationViewModel
    {
        /// <summary>
        /// Можно начинать.
        /// </summary>
        bool CanStart { get; }

        /// <summary>
        /// Начать.
        /// </summary>
        void Start();

        /// <summary>
        /// Отменить.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Прогресс.
        /// </summary>
        IOperationProgressViewModel Progress { get; }
    }
}