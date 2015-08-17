﻿using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Documents;
using DvachBrowser3;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Storage;
using TestingApp.Common;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace TestingApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TestPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public TestPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ClearLogButton_OnClick(object sender, RoutedEventArgs e)
        {
            LogBlock.Blocks.Clear();
        }

        private void ShowProgress(EngineProgress e)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TestProgress.Visibility = Visibility.Visible;
                TestMessage.Visibility = Visibility.Visible;
                TestMessage.Text = e.Message ?? "";
                if (e.Percent != null)
                {
                    TestProgress.IsIndeterminate = false;
                    TestProgress.Value = e.Percent ?? 0.0;
                }
                else
                {
                    TestProgress.IsIndeterminate = true;
                }
            });
        }

        private void HideProgress()
        {
            TestProgress.Visibility = Visibility.Collapsed;
            TestMessage.Visibility = Visibility.Collapsed;
        }

        private void Log(string message)
        {
            var p = new Paragraph();
            p.Inlines.Add(new Run() { Text = message });
            LogBlock.Blocks.Add(p);
        }

        private async void TestButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var storage = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                await storage.ThreadData.ClearCache();
                var logic = ServiceLocator.Current.GetServiceOrThrow<INetworkLogic>();
                var boardOperation = logic.LoadThread(new ThreadLink() { Engine = CoreConstants.Engine.Makaba, Board = "mobi", Thread = 510352 });
                boardOperation.Progress += (obj, msg) => ShowProgress(msg);
                try
                {
                    var boardResult = await boardOperation.Complete();
                    Log("Получены данные борды");
                    Log("OP date: " + boardResult.Posts.Count);
                }
                finally
                {
                    HideProgress();
                }
                Log((await storage.ThreadData.GetCacheSize()).ToString());
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        private async void CancellationSourceTestButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Task testTask;
                var source = new CancellationTokenSource();
                try
                {
                    var token = source.Token;
                    testTask = Task.Factory.StartNew(async t =>
                    {
                        var taskToken = (CancellationToken) t;
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Log("Task before wait"));
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Log("Task before cancel"));
                        taskToken.ThrowIfCancellationRequested();
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Log("Task after cancel"));
                    }, token, token);
                    testTask = testTask.ContinueWith(async (task, obj) =>
                    {
                        await task;
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Log("Task after cancel2"));
                    }, null, token);
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Log("Task created"));
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Log("Before cancel"));
                    source.Cancel();
                }
                finally
                {
                    source.Dispose();
                }
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Log("Before await"));
                await testTask;
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Log("Task awaited"));
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Log(string.Format("Task cancel = {0}", testTask.IsCanceled)));
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        private void NavigateToText_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (TextPage));
        }
    }
}
