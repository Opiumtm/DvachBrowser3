﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.PageServices;
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;
using DvachBrowser3.Views.Partial;
using Template10.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PostingPage : Page, IPageLifetimeCallback, IPageViewModelSource, IDynamicShellAppBarProvider, INavigationRolePage, INotifyPropertyChanged, IWeakEventCallback, IStyleManagerFactory
    {
        private object lifetimeToken;

        public PostingPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
            this.DataContext = this;
            this.Loaded += OnLoaded;
            lifetimeToken = this.BindAppLifetimeEvents();
            Shell.IsNarrowViewChanged.AddCallback(this);
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Loaded -= OnLoaded;
            this.Unloaded -= OnUnloaded;
            Bindings.StopTracking();
            DataContext = null;
            ViewModel = null;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SetNarrowStyle();
        }

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == AppEvents.AppResumeId)
            {
                AppResume?.Invoke(this, e);
            }
            if (channel?.Id == AppEvents.AppSuspendId)
            {
                OnSuspending(e as SuspendingEventArgs);
            }
            if (channel?.Id == Shell.IsNarrowViewChangedId)
            {
                SetNarrowStyle();
            }
        }

        private async void OnSuspending(SuspendingEventArgs e)
        {
            if (ViewModel != null)
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    await ViewModel.Fields.Flush(true);
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
                finally
                {
                    deferral.Complete();
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom?.Invoke(this, e);
        }

        private BoardLinkBase link;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {            
            base.OnNavigatedTo(e);
            var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
            var extParam = NavigationHelper.GetExtLinkFromParameter(e.Parameter);
            link = extParam.Link;
            if (link == null)
            {
                await AppHelpers.ShowError(new InvalidOperationException("Неправильный тип параметра навигации"));
                BootStrapper.Current.NavigationService.GoBack();
                return;
            }
            var board = linkTransform.GetBoardShortName(link);
            if (board == null)
            {
                await AppHelpers.ShowError(new InvalidOperationException("В ссылке навигации не указана доска"));
                BootStrapper.Current.NavigationService.GoBack();
                return;
            }
            var vm = new PostingViewModel(extParam);
            vm.PostingSuccess += LiteWeakEventHelper.CreatePostingSuccessHandler(new WeakReference<PostingPage>(this), (root, esender, ee) => root.OnPostingSuccess(esender, ee));
            vm.NeedSetCaptcha += LiteWeakEventHelper.CreateNeedSetCaptchaHandler(new WeakReference<PostingPage>(this), (root, esender, ee) => root.OnNeedSetCaptcha(esender, ee));
            vm.PropertyChanged += LiteWeakEventHelper.CreatePropertyHandler(new WeakReference<PostingPage>(this), (root, esender, ee) => root.VmOnPropertyChanged(esender, ee));
            if (vm.IsNewThread)
            {
                HeaderText = "НОВЫЙ ТРЕД";
            }
            else
            {
                HeaderText = "ОТПРАВИТЬ ПОСТ";
            }
            ViewModel = vm;
            OnPropertyChanged(nameof(ShowQuote));
            NavigatedTo?.Invoke(this, e);
        }

        private void VmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IPostingViewModel.HasQuote))
            {
                AppBarChange?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnNeedSetCaptcha(object sender, NeedSetCaptchaEventArgs e)
        {
            CaptchaQueryView.QueryParam = new CaptchaQueryViewParam()
            {
                CaptchaType = e.CaptchaType,
                Engine = e.Engine
            };
            CaptchaQueryView.Load(ViewModel?.PostingLink);
            CaptchaPopup.IsContentVisible = true;
        }

        private async void OnPostingSuccess(object sender, PostingSuccessEventArgs e)
        {
            var dialog = new MessageDialog(ViewModel.IsNewThread ? "Новый тред успешно создан" : "Пост успешно отправлен", "Внимание!")
            {
                Commands = { new UICommand("Ок")}
            };
            await dialog.ShowAsync();
            if (BootStrapper.Current.NavigationService.CanGoBack)
            {
                BootStrapper.Current.NavigationService.GoBack();
            }
            if (e.RedirectLink != null && ViewModel.IsNewThread)
            {
                ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new ThreadNavigationTarget(e.RedirectLink));
            }
        }

        private string headerText = "";

        public string HeaderText
        {
            get { return headerText; }
            private set
            {
                headerText = value;
                OnPropertyChanged(nameof(HeaderText));
            }
        }

        /// <summary>
        /// Заход на страницу.
        /// </summary>
        public event EventHandler<NavigationEventArgs> NavigatedTo;

        /// <summary>
        /// Уход со страницы.
        /// </summary>
        public event EventHandler<NavigationEventArgs> NavigatedFrom;

        /// <summary>
        /// Восстановление приложения.
        /// </summary>
        public event EventHandler<object> AppResume;

        /// <summary>
        /// Получить модель представления.
        /// </summary>
        /// <returns>Модель представления.</returns>
        public object GetViewModel()
        {
            return ViewModel;
        }

        /// <summary>
        /// Получить нижнюю строку команд.
        /// </summary>
        /// <returns>Строка команд.</returns>
        public AppBar GetBottomAppBar()
        {
            oldToolbar = toolbar;
            var appBar = new CommandBar();

            if (EntryView.IsNarrowEditFocused)
            {
                appBar.PrimaryCommands.Add(EntryView.CreateAppBarMarkupForNarrowView());
                SetQuoteCommand(appBar);
            }
            else
            {
                if (toolbar == ToolbarType.Default)
                {
                    var postCommand = new AppBarButton()
                    {
                        Label = "Оправить",
                        Icon = new SymbolIcon(Symbol.Send),
                    };
                    postCommand.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = ViewModel, Path = new PropertyPath("ViewModel.Posting.CanStart"), Mode = BindingMode.OneWay });
                    postCommand.Click += PostCommandOnClick;

                    var clearCommand = new AppBarButton()
                    {
                        Label = "Очистить",
                        Icon = new SymbolIcon(Symbol.Clear)
                    };
                    clearCommand.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = ViewModel, Path = new PropertyPath("ViewModel.Posting.CanStart"), Mode = BindingMode.OneWay });
                    clearCommand.Click += ClearCommandOnClick;

                    SetQuoteCommand(appBar);
                    appBar.PrimaryCommands.Add(clearCommand);
                    appBar.PrimaryCommands.Add(postCommand);
                }

                if (toolbar == ToolbarType.Captcha)
                {
                    var refreshCommand = new AppBarButton()
                    {
                        Label = "Обновить",
                        Icon = new SymbolIcon(Symbol.Refresh)
                    };
                    refreshCommand.Click += (sender, e) => CaptchaQueryView.Refresh();
                    refreshCommand.SetBinding(AppBarButton.IsEnabledProperty, new Binding()
                    {
                        Source = CaptchaQueryView,
                        Mode = BindingMode.OneWay,
                        Path = new PropertyPath("CanLoad")
                    });

                    var acceptCommand = new AppBarButton()
                    {
                        Label = "Принять",
                        Icon = new SymbolIcon(Symbol.Accept)
                    };
                    acceptCommand.Click += (sender, e) => CaptchaQueryView.Accept();
                    appBar.PrimaryCommands.Add(acceptCommand);
                    appBar.PrimaryCommands.Add(refreshCommand);
                }
            }

            return appBar;
        }

        private void SetQuoteCommand(CommandBar appBar)
        {
            char quoteChar;

            unchecked
            {
                var symbol = (short) 0xE134;
                quoteChar = (char) symbol;
            }

            AppBarToggleButton quoteCommand = null;

            if (ViewModel.HasQuote)
            {
                quoteCommand = new AppBarToggleButton()
                {
                    Label = "Цитата",
                    Icon = new FontIcon() {FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = new string(quoteChar, 1)},
                    IsChecked = ShowQuote
                };
                quoteCommand.SetBinding(AppBarButton.IsEnabledProperty, new Binding() {Source = ViewModel, Path = new PropertyPath("ViewModel.HasQuote"), Mode = BindingMode.OneWay});
                showQuoteFunc = (v) =>
                {
                    if (v != null && v != quoteCommand.IsChecked)
                    {
                        quoteCommand.IsChecked = v.Value;
                    }
                    return quoteCommand.IsChecked ?? false;
                };
                quoteCommand.Checked += (sender, e) => { OnPropertyChanged(nameof(ShowQuote)); };
                quoteCommand.Unchecked += (sender, e) => { OnPropertyChanged(nameof(ShowQuote)); };
            }
            else
            {
                showQuoteFunc = null;
            }

            if (quoteCommand != null)
            {
                appBar.PrimaryCommands.Add(quoteCommand);
            }
        }

        private Func<bool?, bool> showQuoteFunc = null;

        public bool ShowQuote
        {
            get { return showQuoteFunc?.Invoke(null) ?? false; }
            set { showQuoteFunc?.Invoke(value); }
        }

        private void ClearCommandOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            AppHelpers.DispatchAction(async () =>
            {
                bool isClear = false;
                var dialog = new MessageDialog("Очистить содержимое постинга? Данные будут потеряны!", "Внимание!")
                {
                    Commands =
                    {
                        new UICommand("Да", command =>
                        {
                            isClear = true;
                        }),
                        new UICommand("Нет")
                    },
                    CancelCommandIndex = 1,
                    DefaultCommandIndex = 1                    
                };
                await dialog.ShowAsync();
                if (isClear)
                {
                    await ViewModel.Fields.Clear();
                }
            }, true, 0);
        }

        private void PostCommandOnClick(object sender, RoutedEventArgs e)
        {
            AppHelpers.DispatchAction(async () =>
            {
                bool isPost = false;
                var dialog = new MessageDialog((ViewModel?.IsNewThread ?? false) ? "Создать новый тред?" : "Отправить пост в тред?", "Внимание!")
                {
                    Commands =
                    {
                        new UICommand("Да", command =>
                        {
                            isPost = true;
                        }),
                        new UICommand("Нет")
                    },
                    CancelCommandIndex = 1,
                    DefaultCommandIndex = 0
                };
                await dialog.ShowAsync();
                if (isPost)
                {
                    if (ViewModel != null) ViewModel.Captcha = null;
                    ViewModel?.Post();
                }
            }, true, 0);
        }

        /// <summary>
        /// Изменить строку команд.
        /// </summary>
        public event EventHandler AppBarChange;

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.Posting;

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostingViewModel ViewModel
        {
            get { return (IPostingViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IPostingViewModel), typeof (PostingPage), new PropertyMetadata(null));

        /// <summary>
        /// Возникает при смене значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager { get; } = StyleManagerFactory.Current.GetManager();

        private bool isNarrowView;

        public bool IsNarrowView
        {
            get { return isNarrowView; }
            private set
            {
                isNarrowView = value;
                OnPropertyChanged(nameof(IsNarrowView));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Popup_OnIsContentVisibleChanged(object sender, EventArgs e)
        {
            ToolbarType? newToolbar = null;
            var isVisible = (sender as ContentPopup)?.IsContentVisible ?? false;
            if (sender == QuotePopup)
            {
                ShowQuote = isVisible;
            }
            if (isVisible)
            {
                if (sender == QuotePopup)
                {
                    CaptchaPopup.IsContentVisible = false;
                }
                if (sender == CaptchaPopup)
                {
                    ShowQuote = false;
                    newToolbar = ToolbarType.Captcha;
                }
            }
            toolbar = newToolbar ?? ToolbarType.Default;
            if (toolbar != oldToolbar)
            {
                AppBarChange?.Invoke(this, EventArgs.Empty);
            }
        }

        private ToolbarType oldToolbar = ToolbarType.None;
        private ToolbarType toolbar = ToolbarType.Default;

        private enum ToolbarType
        {
            None,
            Default,
            Captcha
        }

        private void SetNarrowStyle()
        {
            IsNarrowView = Shell.Instance?.IsNarrowView ?? false;
        }

        private void CaptchaQueryView_OnCaptchaQueryResult(object sender, CaptchaQueryResultEventArgs e)
        {
            CaptchaPopup.IsContentVisible = false;
            if (ViewModel != null)
            {
                ViewModel.Captcha = e.Data;
                ViewModel.Post();
            }
        }

        private void EntryView_OnIsNarrowEditFocusedChanged(object sender, EventArgs e)
        {
            AppHelpers.DispatchAction(() =>
            {
                AppBarChange?.Invoke(this, EventArgs.Empty);
                return Task.CompletedTask;
            }, false, !EntryView.IsNarrowEditFocused ? 2000 : 250);
        }

        private Lazy<IStyleManager> _styleManager = new Lazy<IStyleManager>(() => new StyleManager());
        IStyleManager IStyleManagerFactory.GetManager()
        {
            return _styleManager.Value;
        }
    }
}
