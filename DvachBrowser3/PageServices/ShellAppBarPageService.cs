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
            Views.Shell.Instance?.SetBottomAppBar(provider?.GetBottomAppBar());
        }

        /// <summary>
        /// Произошёл уход со страницы.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected override void OnNavigatedFrom(Page sender, NavigationEventArgs e)
        {
            Views.Shell.Instance?.SetBottomAppBar(null);
        }
    }
}