using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Базовый класс сервиса.
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        protected ServiceBase(IServiceProvider services)
        {
            Services = services;
        }

        /// <summary>
        /// Сервисы.
        /// </summary>
        protected IServiceProvider Services { get; private set; }
    }
}