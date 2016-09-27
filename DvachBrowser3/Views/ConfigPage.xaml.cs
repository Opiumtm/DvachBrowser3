using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;
using DvachBrowser3.Styles;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigPage : Page, IPageLifetimeCallback, IShellAppBarProvider, INavigationRolePage, IWeakEventCallback, IStyleManagerFactory
    {
        private object lifetimeToken;

        public ConfigPage()
        {
            this.InitializeComponent();
            lifetimeToken = this.BindAppLifetimeEvents();
        }

        public event EventHandler<NavigationEventArgs> NavigatedTo;

        public event EventHandler<NavigationEventArgs> NavigatedFrom;

        public event EventHandler<object> AppResume;

        public AppBar GetBottomAppBar()
        {
            var appBar = new CommandBar();

            var saveButton = new AppBarButton()
            {
                Label = "Сохранить",
                Icon = new SymbolIcon(Symbol.Accept)
            };

            saveButton.Click += SaveButtonOnClick;

            appBar.PrimaryCommands.Add(saveButton);

            return appBar;
        }

        private void SaveButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            AppHelpers.ActionOnUiThread(async () =>
            {
                await MakabaConfigPart.Save();
                await AppHelpers.ShowMessage("Настройки успешно сохранены");
            });
        }

        public NavigationRole? NavigationRole => Navigation.NavigationRole.Configuration;

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

        private Lazy<IStyleManager> _styleManager = new Lazy<IStyleManager>(() => new StyleManager());
        IStyleManager IStyleManagerFactory.GetManager()
        {
            return _styleManager.Value;
        }
    }
}
