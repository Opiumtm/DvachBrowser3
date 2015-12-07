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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.ViewModels;
using Template10.Common;
using XamlAnimatedGif;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class BannerView : UserControl
    {
        public BannerView()
        {
            this.InitializeComponent();
        }

        private void ViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as IPageBannerViewModel;
            var newValue = e.NewValue as IPageBannerViewModel;
            if (oldValue != null)
            {
                oldValue.BannerLoaded -= OnBannerLoaded;
            }
            if (newValue != null)
            {
                newValue.BannerLoaded += OnBannerLoaded;
            }
            if (ViewModel != null && ViewModel.Behavior == PageBannerBehavior.Enabled && !ViewModel.IsLoaded)
            {
                if (ViewModel.Behavior == PageBannerBehavior.Enabled && !ViewModel.IsLoaded)
                {
                    GifBannerImage.Visibility = Visibility.Collapsed;
                    BannerImage.Visibility = Visibility.Collapsed;
                    AppHelpers.DispatchAction(ViewModel.TryLoadBanner);
                }
            } else if (ViewModel?.IsLoaded == true)
            {
                ShowBanner();
            }
            else
            {
                GifBannerImage.Visibility = Visibility.Collapsed;
                BannerImage.Visibility = Visibility.Collapsed;
            }
        }

        private async void ShowBanner()
        {
            await AppHelpers.Dispatcher.DispatchAsync(() =>
            {
                try
                {
                    if (ViewModel?.LoadedBannerUri != null)
                    {
                        if (ViewModel.MediaType == PageBannerMediaType.Gif)
                        {
                            AnimationBehavior.SetRepeatBehavior(GifBannerImage, RepeatBehavior.Forever);
                            AnimationBehavior.SetAutoStart(GifBannerImage, true);
                            AnimationBehavior.SetSourceUri(GifBannerImage, ViewModel.LoadedBannerUri);
                            GifBannerImage.Visibility = Visibility.Visible;
                            BannerImage.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            BannerImage.Source = new BitmapImage(ViewModel.LoadedBannerUri);
                            GifBannerImage.Visibility = Visibility.Collapsed;
                            BannerImage.Visibility = Visibility.Visible;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                }
            });
        }

        private void OnBannerLoaded(object sender, BannerLoadedEventArgs e)
        {
            ShowBanner();
        }


        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPageBannerViewModel ViewModel
        {
            get { return (IPageBannerViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IPageBannerViewModel), typeof (BannerView),
            new PropertyMetadata(null, ViewModelChangedCallback));

        private static void ViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = d as BannerView;
            o?.ViewModelChanged(e);
        }

        private async void ErrorSymbol_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var ex = ViewModel?.Error;
            if (ex != null)
            {
                await AppHelpers.ShowError(ex);
            }
        }
    }
}
