using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.Xaml;
using DvachBrowser3.Configuration;
using DvachBrowser3.Engines;
using DvachBrowser3.Engines.Makaba;
using DvachBrowser3.Navigation;
using DvachBrowser3.Services;
using DvachBrowser3.Storage.Files;
using DvachBrowser3.SystemInformation;
using DvachBrowser3.Views;

namespace DvachBrowser3
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            InitializeComponent();
            var container = new ServiceContainer();
            ServiceLocator.Current = container;
            CoreServicesInitializer.InitializeServices(container, new SystemInfoParam() { Platform = AppPlatform.Windows10Universal });
            MakabaEngineServicesInitializer.InitializeServices(container, new SystemInfoParam() { Platform = AppPlatform.Windows10Universal });
            EsentServicesInitializer.InitializeServices(container, new SystemInfoParam() { Platform = AppPlatform.Windows10Universal });
            container.RegisterService<INetworkProfileService>(new NetworkProfileService(container));
            container.RegisterService<IUiConfigurationService>(new UiConfigurationService(container));
            container.RegisterService<IPageNavigationService>(new PageNavigationService(container));
            Resuming += (sender, o) =>
            {
                AppEvents.AppResume.RaiseEvent(this, o);
            };
        }

        private bool isInitialized;

        private bool isCoreInitialized;

        private async Task TryCoreInitialize()
        {
            if (!isCoreInitialized)
            {
                isCoreInitialized = true;
                await ServiceLocator.Current.GetServiceOrThrow<IStorageSizeCacheFactory>().InitializeGlobal();
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                var shell = new Views.Shell(nav);
                Window.Current.Content = shell;
                await ServiceLocator.Current.GetServiceOrThrow<INetworkProfileService>().Initialize();
#pragma warning disable 4014
                AppHelpers.Dispatcher.DispatchAsync(async () =>
#pragma warning restore 4014
                {
                    try
                    {
                        var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
                        var makaba = engines.GetEngineById(CoreConstants.Engine.Makaba);
                        var config = (IMakabaEngineConfig)makaba.Configuration;
                        await config.SetDefaultBrowserAgent();
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.BreakOnError(ex);
                    }
                });
                StyleTitleBars();
            }
        }

        private async Task TryInitialize()
        {
            await TryCoreInitialize();
            if (!isInitialized)
            {
                isInitialized = true;
                NavigationService.Navigate(typeof(Views.MainPage));
            }
        }

        private void StyleTitleBars()
        {
            var mobileBar = StatusBarHelper.StatusBar;
            if (mobileBar != null)
            {
                mobileBar.BackgroundColor = (Color) Resources["SystemAccentColor"];
                mobileBar.BackgroundOpacity = 1;
                mobileBar.ForegroundColor = Colors.White;
            }
            
            var appView = StatusBarHelper.ApplicationView;
            if (appView != null)
            {
                if (appView.TitleBar != null)
                {
                    appView.TitleBar.BackgroundColor = (Color) Resources["SystemAccentColor"];
                    appView.TitleBar.ForegroundColor = Colors.White;
                    appView.TitleBar.ButtonBackgroundColor = (Color) Resources["SystemAccentColor"];
                    appView.TitleBar.ButtonForegroundColor = Colors.White;
                    appView.TitleBar.ButtonHoverBackgroundColor = Colors.White;
                    appView.TitleBar.ButtonHoverForegroundColor = Colors.Black;
                }
            }
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await TryInitialize();
            await base.OnInitializeAsync(args);
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await Task.Delay(50);
            await TryInitialize();
        }
    }
}

