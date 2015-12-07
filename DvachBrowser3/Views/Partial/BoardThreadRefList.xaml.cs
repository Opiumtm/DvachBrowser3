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
using DvachBrowser3.ViewModels;

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
    }
}
