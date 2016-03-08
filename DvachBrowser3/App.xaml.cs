using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using DvachBrowser3.Configuration;
using DvachBrowser3.Engines;
using DvachBrowser3.Engines.Makaba;
using DvachBrowser3.Navigation;
using DvachBrowser3.Services;
using DvachBrowser3.Storage;
using DvachBrowser3.Storage.Files;
using DvachBrowser3.SystemInformation;

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
            container.RegisterService<ILocalFolderProvider>(new LocalFolderProvider(container));
            var keyInitializer = new ApiKeysInitializer();
            keyInitializer.SetContainer();
            Resuming += (sender, o) =>
            {
                AppEvents.AppResume.RaiseEvent(this, o);
            };
            Suspending += (sender, e) =>
            {
                AppEvents.AppSuspend.RaiseEvent(this, e);
            };
            UnhandledException += OnUnhandledException;
        }

        private async Task CleanTempFiles()
        {
            try
            {
                var files = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
                var tasks = files.Select(DeleteTempFile).ToArray();
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private async Task DeleteTempFile(StorageFile f)
        {
            try
            {
                await f.DeleteAsync();
            }
            catch
            {
                // ignore
            }
        }

        private bool isInitialized;

        private bool isCoreInitialized;

        private async Task PreloadCaches()
        {
            var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            await Task.WhenAll(
                storage.ThreadData.FavoriteBoards.Preload(),
                storage.ThreadData.FavoriteThreads.Preload(),
                storage.ThreadData.VisitedThreads.Preload(),
                storage.ThreadData.PreloadBoardReferences()
            );
        }

        private async Task TryCoreInitialize()
        {
            if (!isCoreInitialized)
            {
                var d = await ApplicationData.Current.LocalFolder.CreateFolderAsync("errorlog", CreationCollisionOption.OpenIfExists);
                await CleanTempFiles();
                await PreloadCaches();
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
                NavigationService.Frame.CacheSize = 0;
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

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var name = DateTime.Now.Ticks.ToString();
            var p = Path.Combine(ApplicationData.Current.LocalFolder.Path, "errorlog", name + ".txt");
            File.WriteAllText(p, e.Exception.ToString());
        }
    }
}

