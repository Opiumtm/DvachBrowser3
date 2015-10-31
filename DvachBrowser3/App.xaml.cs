using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
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
        }

        private bool isInitialized;

        private void TryInitialize()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                var shell = new Views.Shell(nav);
                Window.Current.Content = shell;
                NavigationService.Navigate(typeof(Views.MainPage));
            }
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            TryInitialize();
            await base.OnInitializeAsync(args);
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await Task.Delay(50);
            TryInitialize();
        }
    }
}

