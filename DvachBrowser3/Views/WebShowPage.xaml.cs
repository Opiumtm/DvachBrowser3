using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Engines;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebShowPage : Page, IPageLifetimeCallback, IShellAppBarProvider, INavigationRolePage, IWeakEventCallback
    {
        private object lifetimeToken;

        public WebShowPage()
        {
            this.InitializeComponent();
            lifetimeToken = this.BindAppLifetimeEvents();
            AppHelpers.DispatchAction(() =>
            {
                Refresh();
                return Task.CompletedTask;
            });
        }

        public event EventHandler<NavigationEventArgs> NavigatedTo;

        public event EventHandler<NavigationEventArgs> NavigatedFrom;

        public event EventHandler<object> AppResume;

        /// <summary>
        /// Получить нижнюю строку команд.
        /// </summary>
        /// <returns>Строка команд.</returns>
        public AppBar GetBottomAppBar()
        {
            var appBar = new CommandBar();

            var syncButton = new AppBarButton()
            {
                Label = "Обновить",
                Icon = new SymbolIcon(Symbol.Sync)
            };

            syncButton.Click += SyncButtonOnClick;

            appBar.PrimaryCommands.Add(syncButton);

            return appBar;
        }

        private void SyncButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Refresh();
        }

        private void Refresh()
        {
            var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            var engine = engines.GetEngineById(CoreConstants.Engine.Makaba);
            ShowWeb.Navigate(engine.EngineUriService.BaseUri);
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.WebShow;

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == AppEvents.AppResumeId)
            {
                AppResume?.Invoke(this, e);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigatedTo?.Invoke(this, e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom?.Invoke(this, e);
        }
    }
}
