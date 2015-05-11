using System;
using DvachBrowser3.Navigation;
using DvachBrowser3.SystemInformation;

namespace DvachBrowser3
{
    /// <summary>
    /// Базовый класс модели представления.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        protected ViewModelBase()
        {
            platform = new Lazy<AppPlatform>(() => Services.GetServiceOrThrow<ISystemInfo>().Platform);
        }

        /// <summary>
        /// Сервисы.
        /// </summary>
        protected IServiceProvider Services
        {
            get { return ServiceLocator.Current; }
        }

        /// <summary>
        /// Навигация.
        /// </summary>
        protected INavigationService Navigation
        {
            get { return Services.GetServiceOrThrow<INavigationService>(); }
        }

        private readonly Lazy<AppPlatform> platform;

        /// <summary>
        /// Платформа.
        /// </summary>
        public AppPlatform Platform
        {
            get { return platform.Value; }
        }
    }
}