using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.PageServices;
using DvachBrowser3.Views;
using Template10.Services.NavigationService;

namespace DvachBrowser3
{
    /// <summary>
    /// События приложения.
    /// </summary>
    public static class AppEvents
    {
        /// <summary>
        /// Возобновление приложения.
        /// </summary>
        public static readonly Guid AppResumeId = new Guid("{407E852B-5EB1-4C23-880D-F32ED1BA24A1}");

        /// <summary>
        /// Возобновление приложения.
        /// </summary>
        public static readonly IWeakEventChannel AppResume = new WeakEventChannel(AppResumeId);

        /// <summary>
        /// Приостановка приложения.
        /// </summary>
        public static readonly Guid AppSuspendId = new Guid("{58A2586F-E429-47DC-99D3-E2DD06631A71}");

        /// <summary>
        /// Приостановка приложения.
        /// </summary>
        public static readonly IWeakEventChannel AppSuspend = new WeakEventChannel(AppSuspendId);

        /// <summary>
        /// Привязать события цикла жизни приложения к странице.
        /// </summary>
        /// <typeparam name="T">Тип страницы.</typeparam>
        /// <param name="page">Страница.</param>
        /// <param name="flags">Флаги.</param>
        /// <returns>Токен цикла жизни приложения.</returns>
        public static IDisposable BindAppLifetimeEvents<T>(this T page, AppEventsBindFlags flags = AppEventsBindFlags.Suspend | AppEventsBindFlags.Resume) where T : Page, IWeakEventCallback, IPageLifetimeCallback
        {
            if (page == null)
            {
                return null;
            }
            return new PageAppEventsToken(page, page, flags);
        }

        /// <summary>
        /// Привязать события цикла жизни приложения к странице.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="obj">Объект.</param>
        /// <param name="flags">Флаги.</param>
        /// <returns>Токен цикла жизни приложения.</returns>
        public static IDisposable BindAppLifetimeEventsToPage<T>(this T obj, AppEventsBindFlags flags = AppEventsBindFlags.Resume) where T : IWeakEventCallback
        {
            if (obj == null)
            {
                return null;
            }
            return new ObjectAppEventsToken(obj, flags);
        }

        private sealed class ObjectAppEventsToken : AppEventsTokenBase
        {
            public ObjectAppEventsToken(IWeakEventCallback eventCallback, AppEventsBindFlags flags)
                : base(eventCallback, flags)
            {
                var page = Shell.HamburgerMenu?.NavigationService?.FrameFacade?.Content as IPageLifetimeCallback;
                if (page != null)
                {
                    BindNavigatedFrom(page);
                    BindCallbacks();
                }
            }
        }

        private sealed class PageAppEventsToken : AppEventsTokenBase
        {
            public PageAppEventsToken(IWeakEventCallback eventCallback, IPageLifetimeCallback pageLifetime, AppEventsBindFlags flags)
                : base(eventCallback, flags)
            {
                BindNavigatedTo(pageLifetime);
                BindNavigatedFrom(pageLifetime);
            }
        }

        private abstract class AppEventsTokenBase : IDisposable
        {
            private Guid? resumeId;
            private Guid? suspendId;

            private readonly WeakReference<IWeakEventCallback> eventCallback;
            private readonly AppEventsBindFlags flags;

            protected AppEventsTokenBase(IWeakEventCallback eventCallback, AppEventsBindFlags flags)
            {
                this.eventCallback = new WeakReference<IWeakEventCallback>(eventCallback);
                this.flags = flags;
            }

            protected void BindCallbacks()
            {
                IWeakEventCallback callbackObj;
                if (eventCallback.TryGetTarget(out callbackObj))
                {
                    if ((flags & AppEventsBindFlags.Resume) != 0)
                    {
                        resumeId = AppResume.AddCallback(callbackObj);
                    }
                    if ((flags & AppEventsBindFlags.Suspend) != 0)
                    {
                        suspendId = AppSuspend.AddCallback(callbackObj);
                    }
                }
            }

            protected void BindNavigatedTo(IPageLifetimeCallback lifetimeCallback)
            {
                if (lifetimeCallback != null)
                {
                    lifetimeCallback.NavigatedTo += BindNavigatedTo(new WeakReference<AppEventsTokenBase>(this));
                }
            }

            protected void BindNavigatedFrom(IPageLifetimeCallback lifetimeCallback)
            {
                if (lifetimeCallback != null)
                {
                    lifetimeCallback.NavigatedTo += BindNavigatedFrom(new WeakReference<AppEventsTokenBase>(this));
                }
            }

            private static EventHandler<NavigationEventArgs> BindNavigatedTo(WeakReference<AppEventsTokenBase> token)
            {
                return (sender, e) =>
                {
                    AppEventsTokenBase obj;
                    if (token.TryGetTarget(out obj))
                    {
                        obj.PageLifetimeOnNavigatedTo(sender, e);
                    }
                };
            }

            private static EventHandler<NavigationEventArgs> BindNavigatedFrom(WeakReference<AppEventsTokenBase> token)
            {
                return (sender, e) =>
                {
                    AppEventsTokenBase obj;
                    if (token.TryGetTarget(out obj))
                    {
                        obj.PageLifetimeOnNavigatedFrom(sender, e);
                    }
                };
            }

            private void PageLifetimeOnNavigatedTo(object sender, NavigationEventArgs e)
            {
                BindCallbacks();
            }

            private void PageLifetimeOnNavigatedFrom(object sender, NavigationEventArgs e)
            {
                if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
                {
                    Dispose();
                }
            }

            public void Dispose()
            {
                if (resumeId != null)
                {
                    AppResume.RemoveCallback(resumeId.Value);
                    resumeId = null;
                }
                if (suspendId != null)
                {
                    AppSuspend.RemoveCallback(suspendId.Value);
                    suspendId = null;
                }
            }
        }
    }

    /// <summary>
    /// На какие события привязываться.
    /// </summary>
    [Flags]
    public enum AppEventsBindFlags : int
    {
        /// <summary>
        /// Возобновление.
        /// </summary>
        Resume = 0x0001,

        /// <summary>
        /// Приостанов.
        /// </summary>
        Suspend = 0x0002        
    }
}