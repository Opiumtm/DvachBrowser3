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
        private WeakReference<Page> pageHandle;

        protected PageLifetimeServiceBase()
        {
            OnNavigatedToCall = CreateCallback(new WeakReference<PageLifetimeServiceBase>(this), OnNavigatedToCallStatic);
            OnNavigatedFromCall = CreateCallback(new WeakReference<PageLifetimeServiceBase>(this), OnNavigatedFromCallStatic);
            OnAppResumeCall = CreateCallback(new WeakReference<PageLifetimeServiceBase>(this), OnAppResumeCallStatic);
            OnUnloadedCall = CreateCallback(new WeakReference<PageLifetimeServiceBase>(this), OnUnloadedCallStatic);
        }

        /// <summary>
        /// Страница.
        /// </summary>
        protected Page Page
        {
            get
            {
                if (pageHandle == null)
                {
                    return null;
                }
                Page obj;
                if (pageHandle.TryGetTarget(out obj))
                {
                    return obj;
                }
                return null;
            }
        }

        /// <summary>
        /// Обратный вызов.
        /// </summary>
        protected IPageLifetimeCallback LifetimeCallback => Page as IPageLifetimeCallback;

        /// <summary>
        /// Прикрепиться к странице.
        /// </summary>
        /// <param name="page">Страница.</param>
        public virtual void Attach(Page page)
        {
            DetachEvents();
            pageHandle = page != null ? new WeakReference<Page>(page) : null;
            var lifetimeCallback = LifetimeCallback;
            if (lifetimeCallback != null)
            {
                lifetimeCallback.NavigatedTo += OnNavigatedToCall;
                lifetimeCallback.NavigatedFrom += OnNavigatedFromCall;
                lifetimeCallback.AppResume += OnAppResumeCall;
            }
            if (page != null)
            {
                page.Unloaded += OnUnloadedCall;
            }
        }

        /// <summary>
        /// Отсоединить события.
        /// </summary>
        protected void DetachEvents()
        {
            var lifetimeCallback = LifetimeCallback;
            if (lifetimeCallback != null)
            {
                lifetimeCallback.NavigatedTo -= OnNavigatedToCall;
                lifetimeCallback.NavigatedFrom -= OnNavigatedFromCall;
                lifetimeCallback.AppResume -= OnAppResumeCall;
            }
            var page = Page;
            if (page != null)
            {
                page.Unloaded -= OnUnloadedCall;
            }
        }

        private readonly EventHandler<NavigationEventArgs> OnNavigatedToCall;

        private readonly EventHandler<NavigationEventArgs> OnNavigatedFromCall;

        private readonly EventHandler<object> OnAppResumeCall;

        private readonly RoutedEventHandler OnUnloadedCall;

        private static EventHandler<NavigationEventArgs> CreateCallback(WeakReference<PageLifetimeServiceBase> weakRef, Action<PageLifetimeServiceBase, object, NavigationEventArgs> callback)
        {
            return (sender, e) =>
            {
                PageLifetimeServiceBase obj;
                if (weakRef.TryGetTarget(out obj))
                {
                    callback(obj, sender, e);
                }
            };
        }

        private static EventHandler<object> CreateCallback(WeakReference<PageLifetimeServiceBase> weakRef, Action<PageLifetimeServiceBase, object, object> callback)
        {
            return (sender, e) =>
            {
                PageLifetimeServiceBase obj;
                if (weakRef.TryGetTarget(out obj))
                {
                    callback(obj, sender, e);
                }
            };
        }

        private static RoutedEventHandler CreateCallback(WeakReference<PageLifetimeServiceBase> weakRef, Action<PageLifetimeServiceBase, object, RoutedEventArgs> callback)
        {
            return (sender, e) =>
            {
                PageLifetimeServiceBase obj;
                if (weakRef.TryGetTarget(out obj))
                {
                    callback(obj, sender, e);
                }
            };
        }

        private static void OnAppResumeCallStatic(PageLifetimeServiceBase obj, object sender, object o)
        {
            obj.OnResume(sender as Page, o);
        }

        private static void OnNavigatedToCallStatic(PageLifetimeServiceBase obj, object sender, NavigationEventArgs e)
        {
            obj.OnNavigatedTo(sender as Page, e);
        }

        private static void OnNavigatedFromCallStatic(PageLifetimeServiceBase obj, object sender, NavigationEventArgs e)
        {
            obj.OnNavigatedFrom(sender as Page, e);
            if (e != null)
            {
                if (e.NavigationMode == NavigationMode.Back || e.NavigationMode == NavigationMode.New)
                {
                    obj.OnPageLeave();
                }
            }
        }

        private static void OnUnloadedCallStatic(PageLifetimeServiceBase obj, object sender, RoutedEventArgs e)
        {
            obj.DetachEvents();
            obj.OnUnloaded(obj.Page, e);
        }

        /// <summary>
        /// Страница выгружена.
        /// </summary>
        /// <param name="sender">Страница.</param>
        /// <param name="e">Событие.</param>
        protected virtual void OnUnloaded(Page sender, RoutedEventArgs e)
        {            
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
        }
    }
}