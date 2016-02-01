using System;
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
    public sealed partial class PostingPage : Page, IPageLifetimeCallback, IPageViewModelSource, IDynamicShellAppBarProvider, INavigationRolePage, INotifyPropertyChanged, IWeakEventCallback
    {
        private object lifetimeToken;

        public PostingPage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
            lifetimeToken = this.BindAppLifetimeEvents();
            Shell.IsNarrowViewChanged.AddCallback(this);
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
            vm.PostingSuccess += OnPostingSuccess;
            vm.NeedSetCaptcha += OnNeedSetCaptcha;
            vm.PropertyChanged += VmOnPropertyChanged;
            if (vm.IsNewThread)
            {
                HeaderText = "НОВЫЙ ТРЕД";
            }
            else
            {
                HeaderText = "ОТПРАВИТЬ ПОСТ";
            }
            DataContext = vm;
            OnPropertyChanged(nameof(ViewModel));
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
        }

        private async void OnPostingSuccess(object sender, PostingSuccessEventArgs e)
        {
            ViewModel.Fields.MarkAsSent();
            var dialog = new MessageDialog(ViewModel.IsNewThread ? "Новый тред успешно создан" : "Пост успешно отправлен", "Внимание!")
            {
                Commands = { new UICommand("Ок")}
            };
            await dialog.ShowAsync();
            if (BootStrapper.Current.NavigationService.CanGoBack)
            {
                BootStrapper.Current.NavigationService.GoBack();
            }
            if (e.RedirectLink != null)
            {
                ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new ThreadNavigationTarget(e.RedirectLink));
            }
        }

        private string headerText;

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
            var appBar = new CommandBar();

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

            char quoteChar;

            unchecked
            {
                var symbol = (short)0xE134;
                quoteChar = (char) symbol;
            }

            AppBarToggleButton quoteCommand = null;

            if (ViewModel.HasQuote)
            {
                quoteCommand = new AppBarToggleButton()
                {
                    Label = "Цитата",
                    Icon = new FontIcon() { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = new string(quoteChar, 1) },
                    IsChecked = ShowQuote
                };
                quoteCommand.SetBinding(AppBarButton.IsEnabledProperty, new Binding() { Source = ViewModel, Path = new PropertyPath("ViewModel.HasQuote"), Mode = BindingMode.OneWay });
                showQuoteFunc = (v) =>
                {
                    if (v != null && v != quoteCommand.IsChecked)
                    {
                        quoteCommand.IsChecked = v.Value;
                    }
                    return quoteCommand.IsChecked ?? false;
                };
                quoteCommand.Checked += (sender, e) =>
                {
                    OnPropertyChanged(nameof(ShowQuote));
                };
                quoteCommand.Unchecked += (sender, e) =>
                {
                    OnPropertyChanged(nameof(ShowQuote));
                };
            }
            else
            {
                showQuoteFunc = null;
            }

            if (quoteCommand != null)
            {
                appBar.PrimaryCommands.Add(quoteCommand);
            }
            appBar.PrimaryCommands.Add(clearCommand);
            appBar.PrimaryCommands.Add(postCommand);

            return appBar;
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
            }, true);
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
                    ViewModel?.Post();
                }
            }, true);
        }

        /// <summary>
        /// Изменить строку команд.
        /// </summary>
        public event EventHandler AppBarChange;

        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        public NavigationRole? NavigationRole => Navigation.NavigationRole.Posting;

        public IPostingViewModel ViewModel => DataContext as IPostingViewModel;

        /// <summary>
        /// Возникает при смене значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public IStyleManager StyleManager => Shell.StyleManager;

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
            var isVisible = (sender as ContentPopup)?.IsContentVisible ?? false;
            if (sender == QuotePopup)
            {
                ShowQuote = isVisible;
            }
            if (isVisible)
            {
                if (sender == QuotePopup)
                {
                }
            }
        }

        private void SetNarrowStyle()
        {
            IsNarrowView = Shell.Instance?.IsNarrowView ?? false;
        }
    }
}
