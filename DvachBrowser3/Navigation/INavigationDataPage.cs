using System.Collections.Generic;
using System.Threading.Tasks;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Страница с хранением данных навигации.
    /// </summary>
    public interface INavigationDataPage
    {
        /// <summary>
        /// Получить данные навигации.
        /// </summary>
        /// <returns>Данные навигации.</returns>
        Task<Dictionary<string, object>> GetNavigationData();

        /// <summary>
        /// Восстановить данные навигации.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Результат.</returns>
        Task RestoreNavigationData(Dictionary<string, object> data);

        /// <summary>
        /// Ключ навигации.
        /// </summary>
        string NavigationDataKey { get; }
    }
}