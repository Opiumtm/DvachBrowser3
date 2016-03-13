using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Navigation;
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;
using Template10.Services.NavigationService;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class BoardThreadRefList : UserControl, INotifyPropertyChanged
    {
        public BoardThreadRefList()
        {
            this.InitializeComponent();
            BindingRoot.DataContext = this;
            this.Unloaded += (sender, e) =>
            {
                ViewModel = null;
            };
        }

        private void ViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            //var oldData = e.OldValue as IBoardPageViewModel;
            //var newData = e.NewValue as IBoardPageViewModel;
            OnPropertyChanged(nameof(ShowBanner));
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IBoardPageViewModel ViewModel
        {
            get { return (IBoardPageViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IBoardPageViewModel), typeof (BoardThreadRefList),
            new PropertyMetadata(null, (o, args) => (o as BoardThreadRefList)?.ViewModelChanged(args)));

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Показывать баннер.
        /// </summary>
        public bool ShowBanner => ViewModel?.Banner?.Behavior == PageBannerBehavior.Enabled;

        private void Banner_OnBannerTapped(object sender, BannerTappedEventArgs e)
        {
            ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new BoardPageNavigationTarget(e.Link));
        }

        /// <summary>
        /// Получить индекс видимого элемента.
        /// </summary>
        /// <returns>Индекс.</returns>
        public IThreadPreviewViewModel GetTopViewIndex()
        {
            try
            {
                var r = MainList.GetVisibleToWindowElements<IThreadPreviewViewModel>().OrderBy(o => o.Item1).FirstOrDefault();
                return r?.Item2;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }            
        }

        /// <summary>
        /// Показать тред.
        /// </summary>
        /// <param name="thread">Тред.</param>
        public void ScrollIntoView(IThreadPreviewViewModel thread)
        {
            if (thread == null)
            {
                return;
            }
            try
            {
                MainList.ScrollIntoView(thread, ScrollIntoViewAlignment.Leading);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Нажат тред.
        /// </summary>
        public event ThreadPreviewTappedEventHandler ThreadTapped;

        private void ThreadElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var f = sender as FrameworkElement;
            var t = f?.Tag as IThreadPreviewViewModel;
            ThreadTapped?.Invoke(this, new ThreadPreviewTappedEventArgs(t));
        }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string ListTitle
        {
            get { return (string) GetValue(ListTitleProperty); }
            set { SetValue(ListTitleProperty, value); }
        }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public static readonly DependencyProperty ListTitleProperty = DependencyProperty.Register("ListTitle", typeof (string), typeof (BoardThreadRefList),
            new PropertyMetadata(null));

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager { get; } = StyleManagerFactory.Current.GetManager();

        private async void MarkAsReadFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IThreadPreviewViewModel;
                t?.MarkAsRead();
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private async void OpenThreadFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IThreadPreviewViewModel;
                if (t?.ThreadLink != null)
                {
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new ThreadNavigationTarget(t.ThreadLink));
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private void MainList_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var prev = GetPreview(args);
            if (prev != null)
            {
                prev.Phase = 0;
                args.RegisterUpdateCallback(Phase1Callback);
            }
        }

        private void Phase1Callback(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var prev = GetPreview(args);
            if (prev != null)
            {
                prev.Phase = 1;
                args.RegisterUpdateCallback(Phase2Callback);
            }
        }

        private void Phase2Callback(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var prev = GetPreview(args);
            if (prev != null)
            {
                prev.Phase = 2;
            }
        }

        private BoardThreadRefPreview GetPreview(ContainerContentChangingEventArgs args)
        {
            var contentRoot = args.ItemContainer.ContentTemplateRoot as Border;
            return contentRoot?.Child as BoardThreadRefPreview;
        }
    }
}
