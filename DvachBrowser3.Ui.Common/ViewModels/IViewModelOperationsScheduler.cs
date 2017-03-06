using System.Threading.Tasks;

namespace DvachBrowser3.Ui.ViewModels
{
    /// <summary>
    /// Средство запуска операций моделей представления.
    /// </summary>
    public interface IViewModelOperationsScheduler
    {
        /// <summary>
        /// Запустить основную операцию (может быть только одна одновременно).
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="operation">Операция.</param>
        /// <returns>Таск выполнения операции.</returns>
        Task<T> QueueForegroundOperation<T>(ViewModelOperationBase<T> operation);

        /// <summary>
        /// Запустить фоновую операцию.
        /// </summary>
        /// <typeparam name="T">Тип результата.</typeparam>
        /// <param name="operation">Операция.</param>
        /// <param name="bypassThrottle">Не учитывать ограничение на максимальное количество операций.</param>
        /// <returns>Таск выполнения операции.</returns>
        Task<T> QueueBackgroundOperation<T>(ViewModelOperationBase<T> operation, bool bypassThrottle = false);
    }
}