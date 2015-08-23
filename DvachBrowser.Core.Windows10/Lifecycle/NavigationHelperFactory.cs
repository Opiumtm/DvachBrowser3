using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3;
using DvachBrowser3.Lifecycle;

namespace DvachBrowser.Core.Lifecycle
{
    /// <summary>
    /// Фабрика создания помощников навигации.
    /// </summary>
    public sealed class NavigationHelperFactory : ServiceBase, INavigationHelperFactory
    {
        /// <summary>
        /// Фрейм.
        /// </summary>
        private Frame frame;

        private Lazy<bool> isHardwareButtonsApiPresentValue = new Lazy<bool>(IsHardwareButtonsApiPresentLazy);

        private bool IsHardwareButtonsApiPresent
        {
            get { return isHardwareButtonsApiPresentValue.Value; }
        }

        private static bool IsHardwareButtonsApiPresentLazy()
        {
            return Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        }

        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="page">Страница.</param>
        /// <returns>Помощник навигации.</returns>
        public INavigationHelper Create(Page page)
        {
            return new NavigationHelper(page, this);
        }

        private bool isInitialized = false;

        /// <summary>
        /// Инициализировать.
        /// </summary>
        /// <param name="f">Фрейм.</param>
        public void Initialize(Frame f)
        {
            if (frame != null)
            {
                frame.Navigated -= FrameOnNavigated;
            }
            this.frame = f;
            CheckCanGoBackState();
            if (!isInitialized)
            {
                if (IsHardwareButtonsApiPresent)
                {
                    HardwareButtons.BackPressed += HardwareButtonsOnBackPressed;
                }
                else
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
                }
            }
            if (frame != null)
            {
                frame.Navigated += FrameOnNavigated;
            }
        }

        private void FrameOnNavigated(object sender, NavigationEventArgs navigationEventArgs)
        {
            CheckCanGoBackState();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            GoBack();
        }

        private void HardwareButtonsOnBackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            GoBack();
        }

        /// <summary>
        /// Проверить состояние кнопки назад.
        /// </summary>
        private void CheckCanGoBackState()
        {
            if (!IsHardwareButtonsApiPresent)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = CanGoBack()
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
            }
        }

        public bool CanGoBack()
        {
            return frame != null && frame.CanGoBack;
        }

        public bool CanGoForward()
        {
            return frame != null && frame.CanGoForward;
        }

        public void GoBack()
        {
            if (CanGoBack()) frame.GoBack();
        }

        public void GoForward()
        {
            if (CanGoForward()) frame.GoForward();
        }


        public sealed class NavigationHelper : INavigationHelper
        {
            private Page Page { get; set; }

            private readonly NavigationHelperFactory parent;

            private Frame Frame => parent.frame;

            /// <summary>
            /// Initializes a new instance of the <see cref="NavigationHelper"/> class.
            /// </summary>
            /// <param name="page">A reference to the current page used for navigation.  
            /// This reference allows for frame manipulation and to ensure that keyboard 
            /// navigation requests only occur when the page is occupying the entire window.</param>
            public NavigationHelper(Page page, NavigationHelperFactory parent)
            {
                this.Page = page;
                this.parent = parent;
            }

            private RelayCommand _goBackCommand;
            private RelayCommand _goForwardCommand;

            public ICommand GoBackCommand
            {
                get
                {
                    if (_goBackCommand == null)
                    {
                        _goBackCommand = new RelayCommand(
                            () => this.GoBack(),
                            () => this.CanGoBack());
                    }
                    return _goBackCommand;
                }
            }

            public ICommand GoForwardCommand
            {
                get
                {
                    if (_goForwardCommand == null)
                    {
                        _goForwardCommand = new RelayCommand(
                            () => this.GoForward(),
                            () => this.CanGoForward());
                    }
                    return _goForwardCommand;
                }
            }

            public bool CanGoBack()
            {
                return parent.CanGoBack();
            }

            public bool CanGoForward()
            {
                return parent.CanGoForward();
            }

            public void GoBack()
            {
                parent.GoBack();
            }

            /// <summary>
            /// Навигация вперёд.
            /// </summary>
            public void GoForward()
            {
                parent.GoForward();
            }

            /// <summary>
            /// Загрузка состояния.
            /// </summary>
            public event LoadStateEventHandler LoadState;

            /// <summary>
            /// Сохранение состояния.
            /// </summary>
            public event SaveStateEventHandler SaveState;

            private String _pageKey;


            public void OnNavigatedFrom(NavigationEventArgs e)
            {
                var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
                var pageState = new Dictionary<String, Object>();
                if (this.SaveState != null)
                {
                    this.SaveState(this, new SaveStateEventArgs(pageState));
                }
                frameState[_pageKey] = pageState;
            }

            public void OnNavigatedTo(NavigationEventArgs e)
            {
                var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
                this._pageKey = "Page-" + this.Frame.BackStackDepth;

                if (e.NavigationMode == NavigationMode.New)
                {
                    // Clear existing state for forward navigation when adding a new page to the
                    // navigation stack
                    var nextPageKey = this._pageKey;
                    int nextPageIndex = this.Frame.BackStackDepth;
                    while (frameState.Remove(nextPageKey))
                    {
                        nextPageIndex++;
                        nextPageKey = "Page-" + nextPageIndex;
                    }

                    // Pass the navigation parameter to the new page
                    if (this.LoadState != null)
                    {
                        this.LoadState(this, new LoadStateEventArgs(e.Parameter, null));
                    }
                }
                else
                {
                    // Pass the navigation parameter and preserved page state to the page, using
                    // the same strategy for loading suspended state and recreating pages discarded
                    // from cache
                    if (this.LoadState != null)
                    {
                        this.LoadState(this, new LoadStateEventArgs(e.Parameter, (Dictionary<String, Object>)frameState[this._pageKey]));
                    }
                }
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public NavigationHelperFactory(IServiceProvider services) : base(services)
        {
        }
    }
}