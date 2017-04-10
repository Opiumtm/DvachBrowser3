namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Состояние основной операции.
    /// </summary>
    public interface IViewModelForegroundState
    {
        /// <summary>
        /// Получить сосотояние активности (готовоности к выполнению основной операции).
        /// </summary>
        /// <returns>Состояние.</returns>
        bool GetActiveState();

        /// <summary>
        /// Установить, что занято.
        /// </summary>
        /// <param name="isBusy">Флаг занятости.</param>
        void SetBusyState(bool isBusy);
    }
}