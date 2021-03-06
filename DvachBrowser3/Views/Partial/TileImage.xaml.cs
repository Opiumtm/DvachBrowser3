﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.ViewModels;
using Template10.Common;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class TileImage : UserControl, INotifyPropertyChanged
    {
        public TileImage()
        {
            this.InitializeComponent();
            BindingRoot.DataContext = this;
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Loaded -= OnLoaded;
            this.Unloaded -= OnUnloaded;
            Bindings.StopTracking();
            BindingRoot.DataContext = null;
            ViewModel = null;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateState(null);
        }

        private void ViewModelChanged(IImageSourceViewModel oldValue, IImageSourceViewModel newValue)
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
            UpdateState(null);
            EnsureLoading();
        }

        private void EnsureLoading()
        {
            AppHelpers.DispatchAction(() =>
            {
                if (ViewModel?.Load != null && !LoadingSuspended)
                {
                    if (!ViewModel.ImageLoaded && !ViewModel.Load.Progress.IsActive)
                    {
                        ViewModel.Load.Start();
                    }
                }
                return Task.CompletedTask;
            }, false, 50);
        }

        private void LoadingSuspendedChanged()
        {
            EnsureLoading();
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateState(e.PropertyName);
        }

        private void UpdateState(string property)
        {
            if (ViewModel == null)
            {
                MainImage.Visibility = Visibility.Collapsed;
                ErrorIcon.Visibility = Visibility.Collapsed;
                NoIcon.Visibility = Visibility.Visible;
            }
            if (ViewModel?.Load?.Progress == null)
            {
                MainImage.Visibility = Visibility.Collapsed;
                ErrorIcon.Visibility = Visibility.Collapsed;
                NoIcon.Visibility = Visibility.Collapsed;
            }
            if (ViewModel?.Load?.Progress?.IsError ?? false)
            {
                MainImage.Visibility = Visibility.Collapsed;
                ErrorIcon.Visibility = Visibility.Visible;
                NoIcon.Visibility = Visibility.Collapsed;
            }
            else if (ViewModel?.ImageLoaded ?? false)
            {
                MainImage.Visibility = Visibility.Visible;
                if (!AttachedFlags.GetIsRendered(ViewModel))
                {
                    ((Storyboard)Resources["ImageStoryBoard"]).Begin();
                    AttachedFlags.SetIsRendered(ViewModel, true);
                }
                ErrorIcon.Visibility = Visibility.Collapsed;
                NoIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainImage.Visibility = Visibility.Collapsed;
                ErrorIcon.Visibility = Visibility.Collapsed;
                NoIcon.Visibility = Visibility.Collapsed;
            }
            if (property == null || property == "Image")
            {
                ImageData = ViewModel?.Image;
            }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IImageSourceViewModel ViewModel
        {
            get { return (IImageSourceViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IImageSourceViewModel), typeof (TileImage),
            new PropertyMetadata(null, ViewModelChangedCallback));

        private static void ViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var d1 = d as TileImage;
            d1?.ViewModelChanged(e.OldValue as IImageSourceViewModel, e.NewValue as IImageSourceViewModel);
        }

        private ImageSource imageData;

        /// <summary>
        /// Изображение.
        /// </summary>
        public ImageSource ImageData
        {
            get { return imageData; }
            set
            {
                imageData = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class AttachedFlags
        {
            public static bool GetIsRendered(IImageSourceViewModel model)
            {
                if (model == null)
                {
                    return true;
                }
                return model.IsRendered;
            }

            public static void SetIsRendered(IImageSourceViewModel model, bool value)
            {
                if (model == null)
                {
                    return;
                }
                model.IsRendered = value;
            }
        }

        /// <summary>
        /// Загрузка заморожена.
        /// </summary>
        public bool LoadingSuspended
        {
            get { return (bool) GetValue(LoadingSuspendedProperty); }
            set { SetValue(LoadingSuspendedProperty, value); }
        }

        /// <summary>
        /// Загрузка заморожена.
        /// </summary>
        public static readonly DependencyProperty LoadingSuspendedProperty = DependencyProperty.Register("LoadingSuspended", typeof (bool), typeof (TileImage), new PropertyMetadata(false, LoadingSuspendedPropertyChangedCallback));

        private static void LoadingSuspendedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TileImage) d).LoadingSuspendedChanged();
        }
    }
}
