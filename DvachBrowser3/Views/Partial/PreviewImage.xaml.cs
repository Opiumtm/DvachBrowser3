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
using Template10.Common;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class PreviewImage : UserControl, INotifyPropertyChanged
    {
        public PreviewImage()
        {
            this.InitializeComponent();
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdateSize();
        }

        private void ViewModelChanged(IImageSourceViewModelWithSize oldValue, IImageSourceViewModelWithSize newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ViewModelOnPropertyChanged;
                if (oldValue.Load != null)
                {
                    oldValue.Load.PropertyChanged -= ViewModelOnPropertyChanged;
                }
            }
            if (newValue != null)
            {
                newValue.PropertyChanged += ViewModelOnPropertyChanged;
                if (newValue.Load != null)
                {
                    newValue.Load.PropertyChanged += ViewModelOnPropertyChanged;
                }
            }
            UpdateSize();
            UpdateState(null);
            if (newValue?.Load != null)
            {
                if (!newValue.ImageLoaded && !newValue.Load.Progress.IsActive)
                {
                    newValue.Load.Start();
                }
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateState(e.PropertyName);
        }

        private void UpdateState(string property)
        {
            if (ViewModel?.Load?.Progress == null)
            {
                IsImageVisible = Visibility.Collapsed;
                IsLoadingVisible = Visibility.Visible;
                IsLoadingRingVisible = Visibility.Collapsed;
                IsErrorVisible = Visibility.Collapsed;
                return;
            }
            IsImageVisible = (ViewModel.ImageLoaded && !ViewModel.Load.Progress.IsActive && !ViewModel.Load.Progress.IsError) ? Visibility.Visible : Visibility.Collapsed;
            IsLoadingVisible = (ViewModel.Load.Progress.IsActive) ? Visibility.Visible : Visibility.Collapsed;
            IsLoadingRingVisible = (ViewModel.Load.Progress.IsActive && !ViewModel.Load.Progress.IsWaiting) ? Visibility.Visible : Visibility.Collapsed;
            IsErrorVisible = (!ViewModel.Load.Progress.IsActive && ViewModel.Load.Progress.IsError) ? Visibility.Visible : Visibility.Collapsed;
            if (property == null || property == "Image")
            {
                ImageData = ViewModel.Image;
            }
        }

        private ImageSource imageData;

        public ImageSource ImageData
        {
            get { return imageData; }
            private set
            {
                imageData = value;
                OnPropertyChanged();
            }
        }

        private Visibility isImageVisible = Visibility.Collapsed;

        public Visibility IsImageVisible
        {
            get { return isImageVisible; }
            set
            {
                isImageVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility isLoadingVisible = Visibility.Collapsed;

        public Visibility IsLoadingVisible
        {
            get { return isLoadingVisible; }
            set
            {
                isLoadingVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility isLoadingRingVisible = Visibility.Collapsed;

        public Visibility IsLoadingRingVisible
        {
            get { return isLoadingRingVisible; }
            set
            {
                isLoadingRingVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility isErrorVisible = Visibility.Collapsed;

        public Visibility IsErrorVisible
        {
            get { return isErrorVisible; }
            set
            {
                isErrorVisible = value;
                OnPropertyChanged();
            }
        }

        private void UpdateSize()
        {
            if (ViewModel == null)
            {
                imgSize = new Size(0,0);
            }
            else
            {
                imgSize = new Size(ViewModel.Width, ViewModel.Height).ScaleTo(new Size(ActualWidth, ActualHeight));
            }
            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(ImageHeight));
            OnPropertyChanged(nameof(ImageWidth));
            // ReSharper restore ExplicitCallerInfoArgument
        }

        private Size imgSize = new Size(0,0);

        public double ImageHeight => imgSize.Height;

        public double ImageWidth => imgSize.Width;

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IImageSourceViewModelWithSize ViewModel
        {
            get { return (IImageSourceViewModelWithSize) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IImageSourceViewModelWithSize), typeof (PreviewImage),
            new PropertyMetadata(null, ViewModelChangedCallback));

        private static void ViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var d1 = d as PreviewImage;
            d1?.ViewModelChanged(e.OldValue as IImageSourceViewModelWithSize, e.NewValue as IImageSourceViewModelWithSize);
        }

        /// <summary>
        /// Возникает при смене значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void ErrorSymbol_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var ex = ViewModel?.Load?.Progress?.Exception;
            if (ex != null)
            {
                await AppHelpers.ShowError(ex);
            }
            ViewModel?.Load?.Start();
        }
    }
}
