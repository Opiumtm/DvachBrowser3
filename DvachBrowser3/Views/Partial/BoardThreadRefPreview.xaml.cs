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
    public sealed partial class BoardThreadRefPreview : UserControl, INotifyPropertyChanged
    {
        public BoardThreadRefPreview()
        {
            this.InitializeComponent();
            PostText.MaxLines = 5;
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IThreadPreviewViewModel ViewModel
        {
            get { return (IThreadPreviewViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IThreadPreviewViewModel), typeof (BoardThreadRefPreview),
            new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dd = d as BoardThreadRefPreview;
            if (dd != null)
            {
                if (e.Property == ViewModelProperty)
                {
                    dd.OnPropertyChanged("PostCollectionViewModel");
                }
            }
        }

        public IPostCollectionViewModel PostCollectionViewModel => ViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
