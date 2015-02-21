using System;
using System.Collections.Generic;

namespace DvachBrowser3
{
    /// <summary>
    /// Контейнер сервисов.
    /// </summary>
    public sealed class ServiceContainer : IServiceProvider
    {
        /// <summary>
        /// Сервисы.
        /// </summary>
        private readonly Dictionary<Type, Func<object>> services = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// Получить сервис.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        /// <returns>Сервис.</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            lock (services)
            {
                var func = services.ContainsKey(serviceType) ? services[serviceType] : null;
                return func != null ? func() : null;
            }
        }

        /// <summary>
        /// Зарегистрировать сервис.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <param name="service">Сервис.</param>
        public void RegisterService<T>(Func<object> service)
        {
            lock (services)
            {
                services[typeof(T)] = service;
            }
        }

        /// <summary>
        /// Зарегистрировать сервис.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <param name="service">Сервис.</param>
        public void RegisterService<T>(T service)
        {
            RegisterService<T>(() => service);
        }
    }
}