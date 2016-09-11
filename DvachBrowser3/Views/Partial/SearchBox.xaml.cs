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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class SearchBox : UserControl, IWeakEventCallback
    {
        public SearchBox()
        {
            this.InitializeComponent();
            MainBorder.DataContext = this;
            Shell.IsNarrowViewChanged.AddCallback(this);
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Loaded -= OnLoaded;
            this.Unloaded -= OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateNarrowLayout();
            RegisterPropertyChangedCallback(VisibilityProperty, VisibilityChangedCallback);
            RegisterPropertyChangedCallback(AnimatedVisibilityProperty, AnimatedVisibilityChangedCallback);
            Visibility = Visibility.Collapsed;
        }

        private void VisibilityChangedCallback(DependencyObject sender, DependencyProperty dp)
        {
            if (Visibility == Visibility.Visible)
            {
                EntryBox.Text = FilterSting;
            }
        }

        private void AnimatedVisibilityChangedCallback(DependencyObject sender, DependencyProperty dp)
        {
            if (AnimatedVisibility == Visibility.Visible)
            {
                Visibility = Visibility.Visible;
                ((Storyboard)Resources["ShowAnimation"]).Begin();                
            }
            else
            {
                ((Storyboard)Resources["HideAnimation"]).Begin();
            }
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
                UpdateNarrowLayout();
            }
        }

        private void UpdateNarrowLayout()
        {
        }

        /// <summary>
        /// Фильтр обновлён.
        /// </summary>
        public event EventHandler FilterUpdated;

        /// <summary>
        /// Строка.
        /// </summary>
        public string FilterSting
        {
            get { return (string) GetValue(FilterStingProperty); }
            set { SetValue(FilterStingProperty, value); }
        }

        /// <summary>
        /// Строка.
        /// </summary>
        public static readonly DependencyProperty FilterStingProperty = DependencyProperty.Register("FilterSting", typeof (string), typeof (SearchBox),
            new PropertyMetadata("", FilterStringChangedCallback));

        /// <summary>
        /// Анимированная видимость.
        /// </summary>
        public Visibility AnimatedVisibility
        {
            get { return (Visibility) GetValue(AnimatedVisibilityProperty); }
            set { SetValue(AnimatedVisibilityProperty, value); }
        }

        /// <summary>
        /// Анимированная видимость.
        /// </summary>
        public static readonly DependencyProperty AnimatedVisibilityProperty = DependencyProperty.Register("AnimatedVisibility", typeof (Visibility), typeof (SearchBox),
            new PropertyMetadata(Visibility.Collapsed));


        private static void FilterStringChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SearchBox;
            obj?.FilterUpdated?.Invoke(d, EventArgs.Empty);
            if (obj != null)
            {
                if ((string)e.NewValue != obj.EntryBox.Text)
                {
                    obj.EntryBox.Text = (string) e.NewValue;
                }
            }
        }

        public void ApplyFilter()
        {
            FilterSting = EntryBox.Text;
        }

        private void HideAnimation_OnCompleted(object sender, object e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
