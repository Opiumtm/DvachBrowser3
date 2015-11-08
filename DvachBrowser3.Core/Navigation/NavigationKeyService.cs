using System;
using System.Collections.Generic;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Сервис навигации.
    /// </summary>
    public sealed class NavigationKeyService : ServiceBase, INavigationKeyService
    {
        private readonly Dictionary<string, INavigationKeyFactory> factoryByType;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public NavigationKeyService(IServiceProvider services) : base(services)
        {
            var factories = new INavigationKeyFactory[]
            {
                new BoardLinkNavigationFactory(), 
            };

            factoryByType = new Dictionary<string, INavigationKeyFactory>(StringComparer.OrdinalIgnoreCase);

            foreach (var f in factories)
            {
                foreach (var k in f.TypeNames)
                {
                    factoryByType[k] = f;
                }
            }
        }

        /// <summary>
        /// Сериализовать в строку.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Строка.</returns>
        public string Serialize(INavigationKey key)
        {
            return string.Format("{0}|{1}", key.TypeName, key.Serialize());
        }

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Строка.</param>
        /// <returns>Ключ.</returns>
        public INavigationKey Deserialize(string data)
        {
            var idx = data.IndexOf('|');
            var typeName = data.Substring(0, idx - 1);
            var keyData = data.Remove(0, idx);
            if (factoryByType.ContainsKey(typeName))
            {
                return factoryByType[typeName].Deserialize(typeName, keyData);
            }
            throw new ArgumentException(string.Format("Тип ключа навигации не поддерживается {0}", typeName));
        }

        /// <summary>
        /// Фабрика ключей навигации.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        public void AddNavigationKeyFactory(INavigationKeyFactory factory)
        {
            if (factory == null)
            {
                return;
            }
            foreach (var k in factory.TypeNames)
            {
                factoryByType[k] = factory;
            }
        }
    }
}