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
    public sealed partial class PostView : UserControl, INotifyPropertyChanged
    {
        public PostView()
        {
            this.InitializeComponent();
            MainGrid.SizeChanged += MainGridOnSizeChanged;
            PostLinesUpdated(MaxLines);
            PostTextView.TextRendered += PostTextViewOnTextRendered;
            PostTextView2.TextRendered += PostTextViewOnTextRendered;
            PostTextViewOnTextRendered(PostTextView, EventArgs.Empty);
        }

        private void PostTextViewOnTextRendered(object sender, EventArgs eventArgs)
        {
            if (MaxLines == 0)
            {
                ExceedLines = PostTextView2.ExceedLines;
            }
            else
            {
                ExceedLines = PostTextView.ExceedLines;
            }
        }

        private void MainGridOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= 500)
            {
                ImageWidth = 100;
            }
            else
            {
                ImageWidth = 150;
            }
        }

        private void PostLinesUpdated(int ml)
        {
            if (ml == 0)
            {
                PlainText.Visibility = Visibility.Collapsed;
                ScrollText.Visibility = Visibility.Visible;
            }
            else
            {
                PlainText.Visibility = Visibility.Visible;
                ScrollText.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostViewModel ViewModel
        {
            get { return (IPostViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IPostViewModel), typeof (PostView),
            new PropertyMetadata(null));

        /// <summary>
        /// Показывать порядковый номер поста.
        /// </summary>
        public bool ShowCounter
        {
            get { return (bool) GetValue(ShowCounterProperty); }
            set { SetValue(ShowCounterProperty, value); }
        }

        /// <summary>
        /// Показывать порядковый номер поста.
        /// </summary>
        public static readonly DependencyProperty ShowCounterProperty = DependencyProperty.Register("ShowCounter", typeof (bool), typeof (PostView),
            new PropertyMetadata(true));

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public int MaxLines
        {
            get { return (int) GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof (int), typeof (PostView),
            new PropertyMetadata(0, MaxLinesChanged));

        private static void MaxLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as PostView;
            var ml = (int)e.NewValue;
            if (obj != null)
            {
                obj.PostLinesUpdated(ml);                                                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double imageWidth = 150;

        public double ImageWidth
        {
            get { return imageWidth; }
            private set
            {
                imageWidth = value;
                OnPropertyChanged();
            }
        }

        private bool exceedLines;

        public bool ExceedLines
        {
            get { return exceedLines; }
            set
            {
                exceedLines = value;
                OnPropertyChanged();
            }
        }
    }
}
