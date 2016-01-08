using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Navigation;
using DvachBrowser3.Storage;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Сервис хранения данных навигации.
    /// </summary>
    public sealed class StoreNavigationDataPageService : PageLifetimeServiceBase
    {
        /// <summary>
        /// Минут хранить.
        /// </summary>
        public double MinutesToStore
        {
            get { return (double) GetValue(MinutesToStoreProperty); }
            set { SetValue(MinutesToStoreProperty, value); }
        }

        /// <summary>
        /// Минут хранить.
        /// </summary>
        public static readonly DependencyProperty MinutesToStoreProperty = DependencyProperty.Register("MinutesToStore", typeof (double), typeof (StoreNavigationDataPageService),
            new PropertyMetadata(60*24*30));

        /// <summary>
        /// Версия.
        /// </summary>
        public string Version
        {
            get { return (string) GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

        /// <summary>
        /// Версия.
        /// </summary>
        public static readonly DependencyProperty VersionProperty = DependencyProperty.Register("Version", typeof (string), typeof (StoreNavigationDataPageService),
            new PropertyMetadata("1.0.0"));


        /// <summary>
        /// Произошёл заход на страницу.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected override async void OnNavigatedTo(Page sender, NavigationEventArgs e)
        {
            await LoadData(sender);
        }

        /// <summary>
        /// Возобновление.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="o">Объект.</param>
        protected override async void OnResume(Page sender, object o)
        {
            await LoadData(sender);
        }

        private async Task LoadData(Page sender)
        {
            try
            {
                var navData = sender as INavigationDataPage;
                if (navData != null)
                {
                    var dataKey = navData.NavigationDataKey;
                    var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                    var data = await store.CustomData.LoadCustomData(dataKey);
                    if (data == null)
                    {
                        return;
                    }
                    if (data.ContainsKey("StoreNavigationDataPageService.Version"))
                    {
                        var v = (string) data["StoreNavigationDataPageService.Version"];
                        if (v != Version)
                        {
                            await DeleteFile(dataKey);
                            return;
                        }
                    }
                    else
                    {
                        await DeleteFile(dataKey);
                        return;
                    }
                    bool isStale = false;
                    if (data.ContainsKey("StoreNavigationDataPageService.DateTime"))
                    {
                        var dt = (DateTime) data["StoreNavigationDataPageService.DateTime"];
                        isStale = (DateTime.Now - dt) > TimeSpan.FromMinutes(MinutesToStore);
                    }
                    if (!isStale)
                    {
                        try
                        {
                            await navData.RestoreNavigationData(data);
                        }
                        catch (Exception ex)
                        {
                            DebugHelper.BreakOnError(ex);
                            await DeleteFile(dataKey);
                        }
                    }
                    else
                    {
                        await DeleteFile(dataKey);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private async Task DeleteFile(string key)
        {
            try
            {
                var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                await store.CustomData.DeleteCustomData(key);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Произошёл уход со страницы.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected override async void OnNavigatedFrom(Page sender, NavigationEventArgs e)
        {
            try
            {
                var navData = sender as INavigationDataPage;
                if (navData != null)
                {
                    var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
                    var data = await navData.GetNavigationData();
                    if (data != null)
                    {
                        data["StoreNavigationDataPageService.DateTime"] = DateTime.Now;
                        data["StoreNavigationDataPageService.Version"] = Version;
                        await store.CustomData.SaveCustomData(navData.NavigationDataKey, data);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }
    }
}