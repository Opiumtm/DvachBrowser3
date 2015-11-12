using System.ComponentModel;
using System.Threading.Tasks;

namespace DvachBrowser3
{
    /// <summary>
    /// Конфигурация.
    /// </summary>
    public interface IConfiguration : INotifyPropertyChanged
    {
        /// <summary>
        /// Сохранить конфигурацию.
        /// </summary>
        /// <returns>Таск.</returns>
        Task Save();
    }
}