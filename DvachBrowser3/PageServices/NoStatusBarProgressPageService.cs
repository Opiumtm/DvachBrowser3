using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Заглушка для страниц без прогресса в строке состояния.
    /// </summary>
    public class NoStatusBarProgressPageService : PageLifetimeServiceBase
    {
        /// <summary>
        /// Произошёл заход на страницу.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected override async void OnNavigatedTo(Page sender, NavigationEventArgs e)
        {
            if (StatusBarHelper.IsStatusBarPresent)
            {
                await StatusBarHelper.StatusBar.ProgressIndicator.HideAsync();
            }
        }

        /// <summary>
        /// Возобновление.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="o">Объект.</param>
        protected async override void OnResume(Page sender, object o)
        {
            if (StatusBarHelper.IsStatusBarPresent)
            {
                await StatusBarHelper.StatusBar.ProgressIndicator.HideAsync();
            }
        }
    }
}