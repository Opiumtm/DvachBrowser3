﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
using DvachBrowser3.PageServices;
using DvachBrowser3.Storage;
using DvachBrowser3.ViewModels;
using DvachBrowser3.Views.Partial;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestingPage : Page, INavigationRolePage, IShellAppBarProvider, IPageLifetimeCallback
    {
        public TestingPage()
        {
            this.InitializeComponent();
            //this.Unloaded += (sender, e) => Bindings.StopTracking();
        }

        public ObservableCollection<string> Files { get; }= new ObservableCollection<string>();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await RefreshList();
            Shell.Instance.BottomAppBar = null;
            NavigatedTo?.Invoke(this, e);
        }

        private async Task RefreshList()
        {
            Files.Clear();
            var dir = await ApplicationData.Current.LocalFolder.CreateFolderAsync("errorlog", CreationCollisionOption.OpenIfExists);
            var files = (await dir.GetFilesAsync()).ToArray().OrderBy(f => f.Name);
            foreach (var f in files)
            {
                Files.Add(f.Name);
            }
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom?.Invoke(this, e);
        }

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.TestPage;

        private void InvokeError_OnClick(object sender, RoutedEventArgs e)
        {
            throw new InvalidOperationException();
        }

        private async void RefreshBase_OnClick(object sender, RoutedEventArgs e)
        {
            await RefreshList();
        }

        private async void DeleteFile_OnClick(object sender, RoutedEventArgs e)
        {
            var fn = (sender as FrameworkElement)?.Tag as string;
            if (fn != null)
            {
                var dir = await ApplicationData.Current.LocalFolder.CreateFolderAsync("errorlog", CreationCollisionOption.OpenIfExists);
                var f = await dir.CreateFileAsync(fn, CreationCollisionOption.OpenIfExists);
                await f.DeleteAsync();
                await RefreshList();
            }
        }

        private async void LogItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var fn = (sender as FrameworkElement)?.Tag as string;
            if (fn != null)
            {
                var dir = await ApplicationData.Current.LocalFolder.CreateFolderAsync("errorlog", CreationCollisionOption.OpenIfExists);
                var f = await dir.CreateFileAsync(fn, CreationCollisionOption.OpenIfExists);
                using (var str = await f.OpenStreamForReadAsync())
                {
                    using (var rd = new StreamReader(str, Encoding.UTF8))
                    {
                        var txt = await rd.ReadToEndAsync();
                        ErrorText.Text = txt;
                        ViewGrid.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Получить нижнюю строку команд.
        /// </summary>
        /// <returns>Строка команд.</returns>
        public AppBar GetBottomAppBar()
        {
            var appBar = new CommandBar();
            var b = new AppBarButton()
            {
                Icon = new SymbolIcon(Symbol.Flag),
                Label = "Тест скорости"
            };
            b.Click += BOnClick;
            appBar.PrimaryCommands.Add(b);
            return appBar;
        }

        private void BOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            AppHelpers.DispatchAction(SpeedTest, true);
        }

        private async Task SpeedTest()
        {
            var sw = new Stopwatch();
            sw.Start();

            var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();

            var link = new ThreadLink()
            {
                Engine = "makaba",
                Board = "mobi",
                Thread = 665542
            };

            for (var i = 0; i < 100; i++)
            {
                await storage.ThreadData.LoadThread(link);
            }

            sw.Stop();
            await AppHelpers.ShowMessage($"{sw.Elapsed}");
        }

        public event EventHandler<NavigationEventArgs> NavigatedTo;
        public event EventHandler<NavigationEventArgs> NavigatedFrom;
        public event EventHandler<object> AppResume;
    }
}
