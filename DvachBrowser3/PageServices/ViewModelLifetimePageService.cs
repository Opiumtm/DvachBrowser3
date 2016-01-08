using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.ViewModels;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Сервис управления временем жизни модели представления.
    /// </summary>
    public sealed class ViewModelLifetimePageService : PageLifetimeServiceBase
    {
        /// <summary>
        /// Произошёл заход на страницу.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected override async void OnNavigatedTo(Page sender, NavigationEventArgs e)
        {
            var vmSource = sender as IPageViewModelSource;
            var vmodel = vmSource?.GetViewModel() as IStartableViewModel;
            if (vmodel != null)
            {
                await vmodel.Start();
            }
        }

        /// <summary>
        /// Произошёл уход со страницы.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected override async void OnNavigatedFrom(Page sender, NavigationEventArgs e)
        {
            var vmSource = sender as IPageViewModelSource;
            var vmodel = vmSource?.GetViewModel() as IStartableViewModel;
            if (vmodel != null)
            {
                await vmodel.Stop();
            }
        }

        /// <summary>
        /// Возобновление.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="o">Объект.</param>
        protected override async void OnResume(Page sender, object o)
        {
            var vmSource = sender as IPageViewModelSource;
            var vmodel = vmSource?.GetViewModel() as IStartableViewModel;
            var vmodelR = vmodel as IStartableViewModelWithResume;
            if (vmodelR != null)
            {
                await vmodelR.Resume();
            }
            else if (vmodel != null)
            {
                await vmodel.Start();
            }
        }
    }
}