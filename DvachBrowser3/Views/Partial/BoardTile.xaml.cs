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
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class BoardTile : UserControl
    {
        public BoardTile()
        {
            this.InitializeComponent();
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (ActualWidth <= 140)
            {
                EngineName.Visibility = Visibility.Collapsed;
            }
            else
            {
                EngineName.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IBoardListBoardViewModel ViewModel
        {
            get { return (IBoardListBoardViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IBoardListBoardViewModel), typeof (BoardTile),
            new PropertyMetadata(null));
    }
}
