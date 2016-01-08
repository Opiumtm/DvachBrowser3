using System.Threading.Tasks;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления, поддерживающая запуск, останов и возобновление.
    /// </summary>
    public interface IStartableViewModelWithResume : IStartableViewModel
    {
        /// <summary>
        /// Возобновить.
        /// </summary>
        /// <returns>Таск.</returns>
        Task Resume();
    }
}