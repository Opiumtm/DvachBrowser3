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

    }

    /// <summary>
    /// Аргумент события по нажатию на тред.
    /// </summary>
    public sealed class ThreadPreviewTappedEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="thread">Тред.</param>
        public ThreadPreviewTappedEventArgs(IThreadPreviewViewModel thread)
        {
            Thread = thread;
        }

        /// <summary>
        /// Тред.
        /// </summary>
        public IThreadPreviewViewModel Thread { get; }
    }

    /// <summary>
    /// Обработчик события по нажатию на тред.
    /// </summary>
    /// <param name="sender">Источник события.</param>
    /// <param name="e">Событие.</param>
    public delegate void ThreadPreviewTappedEventHandler(object sender, ThreadPreviewTappedEventArgs e);
}
