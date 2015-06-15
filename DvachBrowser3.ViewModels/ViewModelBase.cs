using System;
using Windows.UI.Core;
using DvachBrowser3.Configuration;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.SystemInformation;
using DvachBrowser3.ViewModels;

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

        /// <summary>
        /// Конфигурация.
        /// </summary>
        protected IViewConfigurationService Configuration
        {
            get { return Services.GetServiceOrThrow<IViewConfigurationService>(); }
        }

        /// <summary>
        /// Сетевая логика.
        /// </summary>
        protected INetworkLogic NetworkLogic
        {
            get { return Services.GetServiceOrThrow<INetworkLogic>(); }
        }

        /// <summary>
        /// Сервис получения ключей из ссылок.
        /// </summary>
        public IBoardLinkKeyService LinkKeyService
        {
            get { return Services.GetServiceOrThrow<IBoardLinkKeyService>(); }
        }

        /// <summary>
        /// Диспетчер.
        /// </summary>
        protected CoreDispatcher Dispatcher
        {
            get { return GlobalDispatcherHelper.Dispatcher; }
        }
    }
}