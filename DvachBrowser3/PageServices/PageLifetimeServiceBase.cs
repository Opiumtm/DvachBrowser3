using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Сервис, связанный с циклом жизни страницы.
    /// </summary>
    public abstract class PageLifetimeServiceBase : DependencyObject, IPageService
    {
        /// <summary>
        /// Страница.
        /// </summary>
        protected Page Page { get; private set; }

        /// <summary>
        /// Обратный вызов.
        /// </summary>
        protected IPageLifetimeCallback LifetimeCallback { get; private set; }

        /// <summary>
        /// Прикрепиться к странице.
        /// </summary>
        /// <param name="page">Страница.</param>
        public virtual void Attach(Page page)
        {
            DetachEvents();
            Page = page;
            LifetimeCallback = page as IPageLifetimeCallback;
            if (LifetimeCallback != null)
            {
                LifetimeCallback.NavigatedTo += OnNavigatedToCall;
                LifetimeCallback.NavigatedFrom += OnNavigatedFromCall;
                LifetimeCallback.AppResume += OnAppResumeCall;
            }
        }

        /// <summary>
        /// Отсоединить события.
        /// </summary>
        protected void DetachEvents()
        {
            if (LifetimeCallback != null)
            {
                LifetimeCallback.NavigatedTo -= OnNavigatedToCall;
                LifetimeCallback.NavigatedFrom -= OnNavigatedFromCall;
                LifetimeCallback.AppResume -= OnAppResumeCall;
            }
        }

        private void OnAppResumeCall(object sender, object o)
        {
            OnResume(sender as Page, o);
        }

        private void OnNavigatedToCall(object sender, NavigationEventArgs e)
        {
            OnNavigatedTo(sender as Page, e);
        }

        private void OnNavigatedFromCall(object sender, NavigationEventArgs e)
        {
            OnNavigatedFrom(sender as Page, e);
            if (e != null)
            {
                if (e.NavigationMode == NavigationMode.Back || e.NavigationMode == NavigationMode.New)
                {
                    OnPageLeave();
                }
            }
        }

        /// <summary>
        /// Произошёл заход на страницу.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected virtual void OnNavigatedTo(Page sender, NavigationEventArgs e)
        {            
        }

        /// <summary>
        /// Произошёл уход со страницы.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected virtual void OnNavigatedFrom(Page sender, NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Возобновление.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="o">Объект.</param>
        protected virtual void OnResume(Page sender, object o)
        {            
        }

        /// <summary>
        /// Событие по выходу со страницы (намигация Back или New).
        /// </summary>
        protected virtual void OnPageLeave()
        {
            DetachEvents();
        }
    }
}