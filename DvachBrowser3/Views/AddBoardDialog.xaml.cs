using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using DvachBrowser3.Engines;
using DvachBrowser3.ViewModels;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DvachBrowser3.Views
{
    public sealed partial class AddBoardDialog : ContentDialog
    {
        public AddBoardDialog()
        {
            this.InitializeComponent();
            this.DataContext = this;
            ViewModel = new AddBoardViewModel();
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IAddBoardViewModel ViewModel
        {
            get { return (IAddBoardViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IAddBoardViewModel), typeof (AddBoardDialog), new PropertyMetadata(null));

        public AddBoardDialog This => this;

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.ResetValues(EngineBox.SelectedItem, ShortNameBox.Text, DescBox.Text);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        public void ApplyShortName()
        {
            ViewModel.ShortName = ShortNameBox.Text;
        }
    }

    public sealed class AddBoardSelectedEngineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value as INetworkEngine;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value as INetworkEngine;
        }
    }
}
