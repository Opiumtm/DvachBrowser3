using System;

namespace DvachBrowser3.ApiKeys
{
    /// <summary>
    /// Ключ API.
    /// </summary>
    public sealed class ApiKey : IApiKey
    {
        /// <summary>
        /// Ключ.
        /// </summary>
        private readonly string key;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="key">Ключ.</param>
        public ApiKey(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// Получить.
        /// </summary>
        /// <returns>Ключ.</returns>
        public string Get()
        {
            return key;
        }
    }
}