using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Navigation;
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class PostView : UserControl, INotifyPropertyChanged
    {
        public PostView()
        {
            this.InitializeComponent();
            MainGrid.DataContext = this;
            this.Loaded += OnLoaded;
            this.Unloaded += (sender, e) =>
            {
                Bindings.StopTracking();
                MainGrid.DataContext = null;
                ViewModel = null;
            };
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            PostLinesUpdated(MaxLines);
            PostTextView.TextRendered += PostTextViewOnTextRendered;
            PostTextViewOnTextRendered(PostTextView, EventArgs.Empty);
        }

        private void PostTextViewOnTextRendered(object sender, EventArgs eventArgs)
        {
            ExceedLines = PostTextView.ExceedLines;
        }

        private void PostLinesUpdated(int ml)
        {
            PostTextView.MaxLines = ml;
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
            new PropertyMetadata(null, ViewModelPropertyChangedCallback));

        private static void ViewModelPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PostView)?.ViewModelChanged?.Invoke(d, EventArgs.Empty);
        }

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
        /// Показывать разделитель превью.
        /// </summary>
        public bool ShowPreviewSeparator
        {
            get { return (bool) GetValue(ShowPreviewSeparatorProperty); }
            set { SetValue(ShowPreviewSeparatorProperty, value); }
        }

        /// <summary>
        /// Показывать разделитель превью.
        /// </summary>
        public static readonly DependencyProperty ShowPreviewSeparatorProperty = DependencyProperty.Register("ShowPreviewSeparator", typeof (bool), typeof (PostView),
            new PropertyMetadata(false));

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

        /// <summary>
        /// Текст показа треда целиком.
        /// </summary>
        public string ShowFullThreadText
        {
            get { return (string) GetValue(ShowFullThreadTextProperty); }
            set { SetValue(ShowFullThreadTextProperty, value); }
        }

        /// <summary>
        /// Текст показа треда целиком.
        /// </summary>
        public static readonly DependencyProperty ShowFullThreadTextProperty = DependencyProperty.Register("ShowFullThreadText", typeof (string), typeof (PostView),
            new PropertyMetadata("Показать целиком"));

        /// <summary>
        /// Кнопка показа полного поста.
        /// </summary>
        public bool ShowFullPostButton
        {
            get { return (bool) GetValue(ShowFullPostButtonProperty); }
            set { SetValue(ShowFullPostButtonProperty, value); }
        }

        /// <summary>
        /// Кнопка показа полного поста.
        /// </summary>
        public static readonly DependencyProperty ShowFullPostButtonProperty = DependencyProperty.Register("ShowFullPostButton", typeof (bool), typeof (PostView),
            new PropertyMetadata(false));

        /// <summary>
        /// Показать тред целиком.
        /// </summary>
        public event ShowFullThreadEventHandler ShowFullThread;

        /// <summary>
        /// Показать полный пост.
        /// </summary>
        public event ShowFullPostEventHandler ShowFullPost;

        private void ShowFullThreadButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowFullThread?.Invoke(this, new ShowFullThreadEventArgs(ViewModel?.Parent, ViewModel));
        }

        private void ShowFullPostButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowFullPost?.Invoke(this, new ShowFullPostEventArgs(ViewModel));
        }

        private async void Quote_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IPostQuoteViewModel;
                if (t != null)
                {
                    Shell.LinkNavigationManager.RaiseLinkNavigationObject(sender, t.Link, ViewModel);
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private async void CopyMediaLinkFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IPostMediaFileViewModel;
                if (t?.WebLink != null)
                {
                    var dp = new DataPackage();
                    dp.SetText(t.WebLink);
                    dp.SetWebLink(new Uri(t.WebLink, UriKind.Absolute));
                    Clipboard.SetContent(dp);
                    Clipboard.Flush();
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private async void OpenInBrowserMediaFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IPostMediaFileViewModel;
                if (t?.WebLink != null)
                {
                    var uri = new Uri(t.WebLink, UriKind.Absolute);
                    await Launcher.LaunchUriAsync(uri);
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private async void ImageBorder_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IPostMediaFileViewModel;
                if (t?.Link != null)
                {
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new MediaNavigationTarget(t.Link)
                    {
                        SizeKb = t.Size != null ? (double?)((double)t.Size.Value / 1024.0) : null
                    });
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Фаза рендеринга.
        /// </summary>
        public int RenderPhase
        {
            get { return (int) GetValue(RenderPhaseProperty); }
            set { SetValue(RenderPhaseProperty, value); }
        }

        /// <summary>
        /// Фаза рендеринга.
        /// </summary>
        public static readonly DependencyProperty RenderPhaseProperty = DependencyProperty.Register("RenderPhase", typeof (int), typeof (PostView), new PropertyMetadata(-1, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PostView)d).RenderPhaseChanged();
        }

        private void RenderPhaseChanged()
        {
            switch (RenderPhase)
            {
                case 0:
                    //PostTextView.RenderSuspended = true;
                    ImageLoadingSuspended = true;
                    break;
                case 1:
                    //PostTextView.RenderSuspended = false;
                    ImageLoadingSuspended = true;
                    break;
                default:
                    //PostTextView.RenderSuspended = false;
                    ImageLoadingSuspended = false;
                    break;
            }
        }

        /// <summary>
        /// Загрузка изображений приостановлена.
        /// </summary>
        public bool ImageLoadingSuspended
        {
            get { return (bool) GetValue(ImageLoadingSuspendedProperty); }
            set { SetValue(ImageLoadingSuspendedProperty, value); }
        }

        /// <summary>
        /// Загрузка изображений приостановлена.
        /// </summary>
        public static readonly DependencyProperty ImageLoadingSuspendedProperty = DependencyProperty.Register("ImageLoadingSuspended", typeof (bool), typeof (PostView), new PropertyMetadata(false));

        private void PreviewImage_OnLoaded(object sender, RoutedEventArgs e)
        {
            var obj = sender as PreviewImage;
            obj?.SetBinding(PreviewImage.LoadingSuspendedProperty, new Binding() { Source = this, Path = new PropertyPath("ImageLoadingSuspended"), Mode = BindingMode.OneWay });
        }

        public event EventHandler ViewModelChanged;

        /// <summary>
        /// Не кэшировать текст.
        /// </summary>
        public bool NotCacheText
        {
            get { return (bool) GetValue(NotCacheTextProperty); }
            set { SetValue(NotCacheTextProperty, value); }
        }

        /// <summary>
        /// Не кэшировать текст.
        /// </summary>
        public static readonly DependencyProperty NotCacheTextProperty = DependencyProperty.Register("NotCacheText", typeof (bool), typeof (PostView), new PropertyMetadata(false));
    }
}
