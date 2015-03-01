using System.Collections.Generic;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Информация о движках.
    /// </summary>
    public interface INetworkEngines
    {
        /// <summary>
        /// Перечислить движки.
        /// </summary>
        /// <returns>Движки.</returns>
        ICollection<string> ListEngines();

        /// <summary>
        /// Получить движок по его идентификатору.
        /// </summary>
        /// <param name="engine">Движок.</param>
        /// <returns>Идентификатор.</returns>
        INetworkEngine GetEngineById(string engine);
    }
}