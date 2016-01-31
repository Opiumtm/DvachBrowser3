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
    public sealed partial class MarkupControl : UserControl
    {
        public MarkupControl()
        {
            this.InitializeComponent();
            MarkupChanged();
        }

        private IMarkupProvider markupProvider = null;

        private async void MarkupChanged()
        {
            try
            {
                try
                {
                    if (MarkupType != null)
                    {
                        markupProvider = ServiceLocator.Current.GetServiceOrThrow<IMarkupService>().GetProvider(MarkupType.Value);
                    }
                    else
                    {
                        markupProvider = null;
                    }
                }
                catch (Exception ex)
                {
                    DebugHelper.BreakOnError(ex);
                    markupProvider = null;
                }
                UpdateButtons();
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private void UpdateButtons()
        {
            ISet<MarkupTag> tags = new HashSet<MarkupTag>();
            if (markupProvider != null)
            {
                tags = markupProvider.SupportedTags;
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
                if (markupProvider != null && For != null)
                {
                    For.Text = markupProvider.SetMarkup(For.Text, tag, For.SelectionStart, For.SelectionLength);
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Для какого контрола ввода.
        /// </summary>
        public TextBox For
        {
            get { return (TextBox) GetValue(ForProperty); }
            set { SetValue(ForProperty, value); }
        }

        /// <summary>
        /// Для какого контрола ввода.
        /// </summary>
        public static readonly DependencyProperty ForProperty = DependencyProperty.Register("For", typeof (TextBox), typeof (MarkupControl), new PropertyMetadata(null));

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
        /// Тип разметки.
        /// </summary>
        public PostingMarkupType? MarkupType
        {
            get { return (PostingMarkupType?) GetValue(MarkupTypeProperty); }
            set { SetValue(MarkupTypeProperty, value); }
        }

        /// <summary>
        /// Тип разметки.
        /// </summary>
        public static readonly DependencyProperty MarkupTypeProperty = DependencyProperty.Register("MarkupType", typeof (PostingMarkupType?), typeof (MarkupControl), new PropertyMetadata(new PostingMarkupType?(), MarkupTypePropertyChangedCallback));

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
    }
}
