using System.Threading.Tasks;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления, поддерживающая запуск и останов.
    /// </summary>
    public interface IStartableViewModel
    {
        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        Task Start();

        /// <summary>
        /// Останов.
        /// </summary>
        /// <returns>Задача.</returns>
        Task Stop();
    }
}