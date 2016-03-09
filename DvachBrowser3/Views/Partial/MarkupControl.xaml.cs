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
using DvachBrowser3.Markup;
using DvachBrowser3.Posting;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class MarkupControl : UserControl, ICommandBarElement
    {
        public MarkupControl()
        {
            this.InitializeComponent();
            BindingRoot.DataContext = this;
            this.Loaded += OnLoaded;
            this.Unloaded += (sender, e) =>
            {
                MarkupProvider = null;
            };
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            MarkupChanged();
        }

        private void MarkupChanged()
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            ISet<MarkupTag> tags = new HashSet<MarkupTag>();
            if (MarkupProvider != null)
            {
                tags = MarkupProvider.SupportedTags;
            }
            BoldButton.Visibility = tags.Contains(MarkupTag.Bold) ? Visibility.Visible : Visibility.Collapsed;
            ItalicButton.Visibility = tags.Contains(MarkupTag.Italic) ? Visibility.Visible : Visibility.Collapsed;
            SpoilerButton.Visibility = tags.Contains(MarkupTag.Spoiler) ? Visibility.Visible : Visibility.Collapsed;
            StrikeoutButton.Visibility = tags.Contains(MarkupTag.Strikeout) ? Visibility.Visible : Visibility.Collapsed;
            MonospaceButton.Visibility = tags.Contains(MarkupTag.Monospace) ? Visibility.Visible : Visibility.Collapsed;
            UnderlineButton.Visibility = tags.Contains(MarkupTag.Underline) ? Visibility.Visible : Visibility.Collapsed;
            SubButton.Visibility = tags.Contains(MarkupTag.Sub) ? Visibility.Visible : Visibility.Collapsed;
            SupButton.Visibility = tags.Contains(MarkupTag.Sup) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void ApplyTag(MarkupTag tag)
        {
            try
            {
                if (MarkupProvider != null)
                {
                    ApplyMarkup?.Invoke(this, new ApplyMarkupEventArgs(MarkupProvider, tag));
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Применить разметку.
        /// </summary>
        public event ApplyMarkupEventHandler ApplyMarkup;

        /// <summary>
        /// Ориентация.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Ориентация.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof (Orientation), typeof (MarkupControl), new PropertyMetadata(Orientation.Horizontal));

        /// <summary>
        /// Провайдер разметки.
        /// </summary>
        public IMarkupProvider MarkupProvider
        {
            get { return (IMarkupProvider) GetValue(MarkupProviderProperty); }
            set { SetValue(MarkupProviderProperty, value); }
        }

        /// <summary>
        /// Провайдер разметки.
        /// </summary>
        public static readonly DependencyProperty MarkupProviderProperty = DependencyProperty.Register("MarkupProvider", typeof (IMarkupProvider), typeof (MarkupControl), new PropertyMetadata(null, MarkupTypePropertyChangedCallback));

        private static void MarkupTypePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as MarkupControl;
            obj?.MarkupChanged();
        }

        private void BoldButton_OnClick(object sender, RoutedEventArgs e)
        {
            ApplyTag(MarkupTag.Bold);
        }

        private void ItalicButton_OnClick(object sender, RoutedEventArgs e)
        {
            ApplyTag(MarkupTag.Italic);
        }

        private void SpoilerButton_OnClick(object sender, RoutedEventArgs e)
        {
            ApplyTag(MarkupTag.Spoiler);
        }

        private void StrikeoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            ApplyTag(MarkupTag.Strikeout);
        }

        private void MonospaceButton_OnClick(object sender, RoutedEventArgs e)
        {
            ApplyTag(MarkupTag.Monospace);
        }

        private void UnderlineButton_OnClick(object sender, RoutedEventArgs e)
        {
            ApplyTag(MarkupTag.Underline);
        }

        private void SupButton_OnClick(object sender, RoutedEventArgs e)
        {
            ApplyTag(MarkupTag.Sup);
        }

        private void SubButton_OnClick(object sender, RoutedEventArgs e)
        {
            ApplyTag(MarkupTag.Sub);
        }

        /// <summary>
        /// Компактное представление.
        /// </summary>
        public bool IsCompact
        {
            get { return (bool) GetValue(IsCompactProperty); }
            set { SetValue(IsCompactProperty, value); }
        }

        /// <summary>
        /// Компактное представление.
        /// </summary>
        public static readonly DependencyProperty IsCompactProperty = DependencyProperty.Register("IsCompact", typeof (bool), typeof (MarkupControl), new PropertyMetadata(false));

        /// <summary>
        /// Отступ.
        /// </summary>
        public Thickness MarkupPadding
        {
            get { return (Thickness) GetValue(MarkupPaddingProperty); }
            set { SetValue(MarkupPaddingProperty, value); }
        }

        /// <summary>
        /// Отступ.
        /// </summary>
        public static readonly DependencyProperty MarkupPaddingProperty = DependencyProperty.Register("MarkupPadding", typeof (Thickness), typeof (MarkupControl), new PropertyMetadata(new Thickness(2)));

    }
}
