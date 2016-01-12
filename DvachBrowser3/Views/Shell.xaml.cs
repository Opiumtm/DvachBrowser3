using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Styles;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }

        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;

        public Shell(NavigationService navigationService)
        {
            Instance = this;
            this.InitializeComponent();
            MyHamburgerMenu.NavigationService = navigationService;
            VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true);
        }

        public static void SetBusyVisibility(Visibility visible, string text = null)
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                switch (visible)
                {
                    case Visibility.Visible:
                        Instance.FindName(nameof(BusyScreen));
                        Instance.BusyText.Text = text ?? string.Empty;
                        if (VisualStateManager.GoToState(Instance, Instance.BusyVisualState.Name, true))
                        {
                            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                                AppViewBackButtonVisibility.Collapsed;
                        }
                        break;
                    case Visibility.Collapsed:
                        if (VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true))
                        {
                            BootStrapper.Current.UpdateShellBackButton();
                        }
                        break;
                }
            });
        }

        public void SetBottomAppBar(AppBar bar)
        {
            BottomAppBar = bar;
        }

        /// <summary>
        /// Сжатый вид.
        /// </summary>
        public bool IsNarrowView
        {
            get { return (bool) GetValue(IsNarrowViewProperty); }
            set { SetValue(IsNarrowViewProperty, value); }
        }

        /// <summary>
        /// Сжатый вид.
        /// </summary>
        public static readonly DependencyProperty IsNarrowViewProperty = DependencyProperty.Register("IsNarrowView", typeof (bool), typeof (Shell),
            new PropertyMetadata(false, IsNarrowViewPropertyChangedCallback));

        private static void IsNarrowViewPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IsNarrowViewChanged.RaiseEvent(d, e);
        }

        /// <summary>
        /// Состояние свойства IsNarrowViewChanged изменилось.
        /// </summary>
        public static readonly Guid IsNarrowViewChangedId;

        /// <summary>
        /// Состояние свойства IsNarrowViewChanged изменилось.
        /// </summary>
        public static readonly IWeakEventChannel IsNarrowViewChanged;

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public static readonly IStyleManager StyleManager;

        static Shell()
        {
            IsNarrowViewChangedId = new Guid("{B87399B2-75DB-4F41-BE66-4B14ACCB295A}");
            IsNarrowViewChanged = new WeakEventChannel(IsNarrowViewChangedId);
            StyleManager = new StyleManager();
        }
    }
}
