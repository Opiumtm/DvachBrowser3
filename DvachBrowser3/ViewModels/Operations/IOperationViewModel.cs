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
        /// Начать.
        /// </summary>
        /// <param name="arg">Аргумент.</param>
        void Start(object arg);

        /// <summary>
        /// Отменить.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Прогресс.
        /// </summary>
        IOperationProgressViewModel Progress { get; }

        /// <summary>
        /// Запретить.
        /// </summary>
        void Disable();

        /// <summary>
        /// Разрешить.
        /// </summary>
        void Enable();
    }
}