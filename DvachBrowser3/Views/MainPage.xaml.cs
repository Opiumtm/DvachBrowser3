using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;
using Template10.Common;

namespace DvachBrowser3.Views
{
    public sealed partial class MainPage : Page, IPageLifetimeCallback, IShellAppBarProvider, INavigationRolePage
    {
        public MainPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            InitializeComponent();
            BootStrapper.Current.Resuming += (sender, o) => this.AppResume?.Invoke(this, o);
        }

        public event EventHandler<NavigationEventArgs> NavigatedTo;

        public event EventHandler<NavigationEventArgs> NavigatedFrom;

        public event EventHandler<object> AppResume;

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

        public AppBar GetBottomAppBar()
        {
            return null;
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.Main;
    }
}
