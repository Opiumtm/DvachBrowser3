using System;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.PageServices;
using DvachBrowser3.Views;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Помощник времени жизни моделей представления.
    /// </summary>
    public static class PageLifetimeHelper
    {
        /// <summary>
        /// Привязать отмену к времени жизни страницы.
        /// </summary>
        /// <param name="viewModel">Модель представления.</param>
        public static void BindCancelToPageLifeTime(this ICancellableViewModel viewModel)
        {
            if (viewModel == null)
            {
                return;
            }
            var page = Shell.HamburgerMenu?.NavigationService?.FrameFacade?.Content as IPageLifetimeCallback;
            if (page == null)
            {
                return;
            }
            EventHandler<NavigationEventArgs> callback = null;
            callback = (sender, e) =>
            {
                viewModel.Cancel();
                if (callback != null) page.NavigatedFrom -= callback;
            };
            page.NavigatedFrom += callback;
        }
    }
}