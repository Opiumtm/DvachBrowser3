using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Сервис установки строки команд в оболочке.
    /// </summary>
    public sealed class ShellAppBarPageService : PageLifetimeServiceBase
    {
        /// <summary>
        /// Произошёл заход на страницу.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected override void OnNavigatedTo(Page sender, NavigationEventArgs e)
        {
            var provider = sender as IShellAppBarProvider;
            var dynProvider = sender as IDynamicShellAppBarProvider;
            if (dynProvider != null)
            {
                dynProvider.AppBarChange += DynProviderOnAppBarChange;
            }
            Views.Shell.Instance?.SetBottomAppBar(provider?.GetBottomAppBar());
        }

        private void DynProviderOnAppBarChange(object sender, EventArgs eventArgs)
        {
            var provider = sender as IShellAppBarProvider;
            Views.Shell.Instance?.SetBottomAppBar(provider?.GetBottomAppBar());
        }

        /// <summary>
        /// Произошёл уход со страницы.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected override void OnNavigatedFrom(Page sender, NavigationEventArgs e)
        {
            if (e != null)
            {
                if (e.NavigationMode == NavigationMode.Back || e.NavigationMode == NavigationMode.New)
                {
                    Views.Shell.Instance?.SetBottomAppBar(null);
                }
            }
        }

        /// <summary>
        /// Возобновление.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="o">Объект.</param>
        protected override void OnResume(Page sender, object o)
        {
            var provider = sender as IShellAppBarProvider;
            Views.Shell.Instance?.SetBottomAppBar(provider?.GetBottomAppBar());
        }
    }
}