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
            this.SizeChanged += OnSizeChanged;
            SizeStateChanged();
        }

        private void SizeStateChanged()
        {
            if (this.ActualWidth <= 500)
            {
                VerticalHeaderVisibility = Visibility.Visible;
                HorizontalHeaderVisibility = Visibility.Collapsed;
                PostText.MaxLines = 7;
                ImageWidth = 100;
                ImageWidthWithBorder = 104;
            }
            else
            {
                VerticalHeaderVisibility = Visibility.Collapsed;
                HorizontalHeaderVisibility = Visibility.Visible;
                PostText.MaxLines = 5;
                ImageWidth = 150;
                ImageWidthWithBorder = 154;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            SizeStateChanged();
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

        /// <summary>
        /// Показ вертикального хидера.
        /// </summary>
        public Visibility VerticalHeaderVisibility
        {
            get { return (Visibility) GetValue(VerticalHeaderVisibilityProperty); }
            set { SetValue(VerticalHeaderVisibilityProperty, value); }
        }

        /// <summary>
        /// Показ вертикального хидера.
        /// </summary>
        public static readonly DependencyProperty VerticalHeaderVisibilityProperty = DependencyProperty.Register("VerticalHeaderVisibility", typeof (Visibility), typeof (BoardThreadRefPreview),
            new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Показ горизонтального хидера.
        /// </summary>
        public Visibility HorizontalHeaderVisibility
        {
            get { return (Visibility) GetValue(HorizontalHeaderVisibilityProperty); }
            set { SetValue(HorizontalHeaderVisibilityProperty, value); }
        }

        /// <summary>
        /// Показ горизонтального хидера.
        /// </summary>
        public static readonly DependencyProperty HorizontalHeaderVisibilityProperty = DependencyProperty.Register("HorizontalHeaderVisibility", typeof (Visibility), typeof (BoardThreadRefPreview),
            new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Ширина изображения.
        /// </summary>
        public double ImageWidth
        {
            get { return (double) GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        /// <summary>
        /// Ширина изображения.
        /// </summary>
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof (double), typeof (BoardThreadRefPreview),
            new PropertyMetadata(100.0));

        /// <summary>
        /// Ширина изображения с бордюром.
        /// </summary>
        public double ImageWidthWithBorder
        {
            get { return (double) GetValue(ImageWidthWithBorderProperty); }
            set { SetValue(ImageWidthWithBorderProperty, value); }
        }

        /// <summary>
        /// Ширина изображения с бордюром.
        /// </summary>
        public static readonly DependencyProperty ImageWidthWithBorderProperty = DependencyProperty.Register("ImageWidthWithBorder", typeof (double), typeof (BoardThreadRefPreview),
            new PropertyMetadata(104.0));
    }
}
