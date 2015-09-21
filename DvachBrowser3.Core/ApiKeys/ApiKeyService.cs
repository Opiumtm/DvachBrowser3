using System;
using System.Collections.Generic;

namespace DvachBrowser3.ApiKeys
{
    /// <summary>
    /// Сервис ключей API.
    /// </summary>
    public sealed class ApiKeyService : ServiceBase, IApiKeyService
    {
        private readonly Dictionary<string, IApiKeyContainer> containers = new Dictionary<string, IApiKeyContainer>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public ApiKeyService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Найти контейнер.
        /// </summary>
        /// <param name="name">Имя.</param>
        /// <returns>Контейнер.</returns>
        public IApiKeyContainer Find(string name)
        {
            lock (containers)
            {
                return containers.ContainsKey(name) ? containers[name] : null;
            }
        }

        /// <summary>
        /// Добавить.
        /// </summary>
        /// <param name="name">Имя.</param>
        /// <param name="container">Контейнер.</param>
        public void Add(string name, IApiKeyContainer container)
        {
            lock (containers)
            {
                containers[name] = container;
            }
        }
    }
}