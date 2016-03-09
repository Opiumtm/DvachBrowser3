using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Вызов сборки мусора при навигации.
    /// </summary>
    public sealed class GcInvokePageService : PageLifetimeServiceBase
    {
        /// <summary>
        /// Произошёл заход на страницу.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected override void OnNavigatedTo(Page sender, NavigationEventArgs e)
        {
            AppHelpers.DispatchAction(() =>
            {
                GC.Collect();
                return Task.CompletedTask;
            });
        }
    }
}