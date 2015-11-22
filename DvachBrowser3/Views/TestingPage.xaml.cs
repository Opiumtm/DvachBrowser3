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
using DvachBrowser3.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestingPage : Page
    {
        private readonly IBoardPageLoaderViewModel boardLoader;

        public TestingPage()
        {
            boardLoader = new BoardPageLoaderViewModel(new BoardPageLink()
            {
                Engine = CoreConstants.Engine.Makaba,
                Page = 0,
                Board = "b"
            });
            boardLoader.PageLoaded += BoardLoaderOnPageLoaded;
            this.InitializeComponent();
            BannerView.Visibility = Visibility.Collapsed;
            SampleImage.Visibility = Visibility.Collapsed;
        }

        private void BoardLoaderOnPageLoaded(object sender, EventArgs eventArgs)
        {
            BannerView.ViewModel = boardLoader.Page?.Banner;
            BannerView.Visibility = Visibility.Visible;
            boardLoader.Page?.Banner?.TryLoadBanner();
            SampleImage.ViewModel =
                boardLoader?.Page?.Threads?.FirstOrDefault()?
                    .Posts?.FirstOrDefault()?
                    .Media?.Files?.FirstOrDefault()?
                    .ThumbnailImage;
            SampleImage.Visibility = Visibility.Visible;
        }

        public IBoardPageLoaderViewModel BoardLoader => boardLoader;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await boardLoader.Start();
            DvachView.Settings.IsJavaScriptEnabled = true;
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

        private void ReloadCookies_OnClick(object sender, RoutedEventArgs e)
        {
            DvachView.Navigate(new Uri("https://2ch.hk/"));
        }

        private async void SetCookies_OnClick(object sender, RoutedEventArgs e)
        {
            var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            var makaba = engines.GetEngineById(CoreConstants.Engine.Makaba);
            var config = (IMakabaEngineConfig)makaba.Configuration;
            await config.SetDefaultBrowserAgent();
            await config.Save();
        }
    }
}
