using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;
using Template10.Common;

namespace DvachBrowser3.Views
{
    public sealed partial class MainPage : Page, IPageLifetimeCallback, IShellAppBarProvider, INavigationRolePage, IWeakEventCallback
    {
        public MainPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            InitializeComponent();
            AppEvents.AppResume.AddCallback(this);
        }

        /// <summary>
        /// �������� �������.
        /// </summary>
        /// <param name="sender">�����������.</param>
        /// <param name="e">�������� �������.</param>
        /// <param name="channel">�����.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == AppEvents.AppResumeId)
            {
                AppResume?.Invoke(this, e);
            }
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
        /// �������� ���� ���������.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.Main;
    }
}
