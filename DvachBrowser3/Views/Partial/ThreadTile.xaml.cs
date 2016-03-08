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
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class ThreadTile : UserControl, IWeakEventCallback, INotifyPropertyChanged
    {
        private DispatcherTimer timer;

        private WeakReference<DoubleAnimation> imageSlideInAnimationHandle;
        private WeakReference<DoubleAnimation> imageSlideOutAnimationHandle;

        public ThreadTile()
        {
            this.InitializeComponent();
            MainBorder.DataContext = this;
            Shell.IsNarrowViewChanged.AddCallback(this);
            this.timerOnClick = CreateTimerOnTick(new WeakReference<ThreadTile>(this));
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private readonly EventHandler<object> timerOnClick;

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            timer = new DispatcherTimer();
            SetTimePeriod();
            timer.Tick += timerOnClick;
            timer.Start();
            TileImageTransform.Y = styleManager.Tiles.BoardTileHeight;
            var imageSlideInAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1.0 / 2)),
                EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseIn },
            };
            imageSlideInAnimationHandle = new WeakReference<DoubleAnimation>(imageSlideInAnimation);
            Storyboard.SetTargetName(imageSlideInAnimation, "TileImageTransform");
            Storyboard.SetTargetProperty(imageSlideInAnimation, "Y");
            var imageSlideOutAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(1.0 / 2)),
                EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut },
            };
            imageSlideOutAnimationHandle = new WeakReference<DoubleAnimation>(imageSlideOutAnimation);
            Storyboard.SetTargetName(imageSlideOutAnimation, "TileImageTransform");
            Storyboard.SetTargetProperty(imageSlideOutAnimation, "Y");
            ((Storyboard)Resources["ImageSlideIn"]).Children.Add(imageSlideInAnimation);
            ((Storyboard)Resources["ImageSlideOut"]).Children.Add(imageSlideOutAnimation);
            UpdateAnimationData();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel = null;
            var t = timer;
            if (t != null)
            {
                t.Stop();
                t.Tick -= timerOnClick;
            }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public ICommonTileViewModel ViewModel
        {
            get { return (ICommonTileViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (ICommonTileViewModel), typeof (ThreadTile), new PropertyMetadata(null));

        private bool isImageSlided = false;

        private static readonly Random Rnd = new Random();

        private IStyleManager styleManager = StyleManagerFactory.Current.GetManager();

        private void SetTimePeriod()
        {
            var t = timer;
            if (t != null)
            {
                double sec;
                if (isImageSlided)
                {
                    sec = (Rnd.NextDouble() * 3.5 + 1.5) * 1.5;
                }
                else
                {
                    sec = (Rnd.NextDouble() * 3.5 + 1.5) * 2.5;
                }
                t.Interval = TimeSpan.FromSeconds(sec);
            }
        }

        private static EventHandler<object> CreateTimerOnTick(WeakReference<ThreadTile> handle)
        {
            return (sender, o) =>
            {
                ThreadTile obj;
                if (handle.TryGetTarget(out obj))
                {
                    obj.TimerOnTickHandler(sender, o);
                }
            };
        }

        private void TimerOnTickHandler(object sender, object o)
        {
            if (isImageSlided)
            {
                var storyBoard = (Storyboard) Resources["ImageSlideOut"];
                storyBoard.Begin();
            }
            else
            {
                var storyBoard = (Storyboard)Resources["ImageSlideIn"];
                storyBoard.Begin();
            }
            isImageSlided = !isImageSlided;
            SetTimePeriod();
        }

        /// <summary>
        /// Получить событие.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="e">Параметр события.</param>
        /// <param name="channel">Канал.</param>
        public void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e)
        {
            if (channel?.Id == Shell.IsNarrowViewChangedId)
            {
                UpdateAnimationData();
            }
        }

        private void UpdateAnimationData()
        {
            DoubleAnimation imageSlideInAnimation;
            if (imageSlideInAnimationHandle.TryGetTarget(out imageSlideInAnimation))
            {
                imageSlideInAnimation.From = -1 * styleManager.Tiles.BoardTileHeight;
            }
            DoubleAnimation imageSlideOutAnimation;
            if (imageSlideOutAnimationHandle.TryGetTarget(out imageSlideOutAnimation))
            {
                imageSlideOutAnimation.To = styleManager.Tiles.BoardTileHeight;
            }
            MainBorder.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, styleManager.Tiles.BoardTileWidth, styleManager.Tiles.BoardTileHeight)
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
