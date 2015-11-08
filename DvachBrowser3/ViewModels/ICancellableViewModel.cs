namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления с отменяемой операцией.
    /// </summary>
    public interface ICancellableViewModel
    {
        /// <summary>
        /// Отменить операцию.
        /// </summary>
        void Cancel();
    }
}