using System;
using Windows.UI.Xaml.Navigation;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Обратный вызов цикла жизни страницы.
    /// </summary>
    public interface IPageLifetimeCallback
    {
        /// <summary>
        /// Заход на страницу.
        /// </summary>
        event EventHandler<NavigationEventArgs> NavigatedTo;

        /// <summary>
        /// Уход со страницы.
        /// </summary>
        event EventHandler<NavigationEventArgs> NavigatedFrom;
    }
}