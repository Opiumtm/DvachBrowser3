using System.Threading.Tasks;

namespace DvachBrowser3.Views.Configuration
{
    /// <summary>
    /// Часть конфигурации UI.
    /// </summary>
    public interface IUiConfigPart
    {
        /// <summary>
        /// Сохранить.
        /// </summary>
        /// <returns>Таск.</returns>
        Task Save();
    }
}