using System.Collections.Generic;

namespace DvachBrowser3.ApiKeys
{
    /// <summary>
    /// Контейнер ключей API.
    /// </summary>
    public interface IApiKeyContainer
    {
        /// <summary>
        /// Ключи.
        /// </summary>
        /// <returns>Ключи.</returns>
        IReadOnlyDictionary<string, IApiKey> GetKeys();
    }
}