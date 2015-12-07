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
using DvachBrowser3.Engines;
using DvachBrowser3.Engines.Makaba;
using DvachBrowser3.Links;
using DvachBrowser3.Navigation;
using DvachBrowser3.ViewModels;
using DvachBrowser3.Views.Partial;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestingPage : Page, INavigationRolePage
    {
        private readonly IBoardPageLoaderViewModel boardLoader;

        public TestingPage()
        {
            this.InitializeComponent();
            boardLoader = new BoardPageLoaderViewModel(new BoardPageLink()
            {
                Engine = CoreConstants.Engine.Makaba,
                Page = 0,
                Board = "po"
            });
            boardLoader.PageLoaded += BoardLoaderOnPageLoaded;
            ProgressControl.ViewModel = boardLoader.Update.Progress;
            BoardThreadList.SetBinding(BoardThreadRefList.ViewModelProperty, new Binding()
            {
                Source = boardLoader,
                Path = new PropertyPath("Page"),
                Mode = BindingMode.OneWay
            });
        }

        private void BoardLoaderOnPageLoaded(object sender, EventArgs eventArgs)
        {
        }

        public IBoardPageLoaderViewModel BoardLoader => boardLoader;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await boardLoader.Start();
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            await boardLoader.Stop();
        }

        private async void LoadBoardDataButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                boardLoader.Reload();
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.TestPage;
    }
}
