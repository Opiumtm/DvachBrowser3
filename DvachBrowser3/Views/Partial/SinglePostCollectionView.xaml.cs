using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class SinglePostCollectionView : UserControl, IPostCollectionVisualIndexStore
    {
        public SinglePostCollectionView()
        {
            this.InitializeComponent();
            BindingRoot.DataContext = this;
            this.Loaded += OnLoaded;
            this.Unloaded += (sender, e) =>
            {
                ViewModel = null;
            };
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            SetCollectionViewSource(ViewModel);
        }

        private void SetCollectionViewSource(IPostCollectionViewModel viewModel)
        {
            if (CollectionView != null)
            {
                CollectionView.Source = viewModel?.Posts;
            }
        }

        private CollectionViewSource CollectionView => Resources["CollectionView"] as CollectionViewSource;

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostCollectionViewModel ViewModel
        {
            get { return (IPostCollectionViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IPostCollectionViewModel), typeof(SinglePostCollectionView),
            new PropertyMetadata(null, (d, e) => ((SinglePostCollectionView)d).SetCollectionViewSource(e.NewValue as IPostCollectionViewModel)));

        /// <summary>
        /// Максимальное количество строк.
        /// </summary>
        public int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        /// <summary>
        /// Максимальное количество строк.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(SinglePostCollectionView),
            new PropertyMetadata(0));

        /// <summary>
        /// Показывать счётчик постов.
        /// </summary>
        public bool ShowCounter
        {
            get { return (bool)GetValue(ShowCounterProperty); }
            set { SetValue(ShowCounterProperty, value); }
        }

        /// <summary>
        /// Показывать счётчик постов.
        /// </summary>
        public static readonly DependencyProperty ShowCounterProperty = DependencyProperty.Register("ShowCounter", typeof(bool), typeof(SinglePostCollectionView),
            new PropertyMetadata(true));

        /// <summary>
        /// Показывать разделитель превью треда.
        /// </summary>
        public bool ShowPreviewSeparator
        {
            get { return (bool)GetValue(ShowPreviewSeparatorProperty); }
            set { SetValue(ShowPreviewSeparatorProperty, value); }
        }

        /// <summary>
        /// Показывать разделитель превью треда.
        /// </summary>
        public static readonly DependencyProperty ShowPreviewSeparatorProperty = DependencyProperty.Register("ShowPreviewSeparator", typeof(bool), typeof(SinglePostCollectionView),
            new PropertyMetadata(false));

        /// <summary>
        /// Заголовок.
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(SinglePostCollectionView),
            new PropertyMetadata(null));

        /// <summary>
        /// Нижний заголовок.
        /// </summary>
        public object Footer
        {
            get { return (object)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        /// <summary>
        /// Нижний заголовок.
        /// </summary>
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof(object), typeof(SinglePostCollectionView),
            new PropertyMetadata(null));

        /// <summary>
        /// Шаблон заголовка.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Шаблон заголовка.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(SinglePostCollectionView),
            new PropertyMetadata(null));

        /// <summary>
        /// Шаблон нижнего заголовка.
        /// </summary>
        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        /// <summary>
        /// Шаблон нижнего заголовка.
        /// </summary>
        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(SinglePostCollectionView),
            new PropertyMetadata(null));

        /// <summary>
        /// Текст кнопки показа треда целиком.
        /// </summary>
        public string ShowFullThreadText
        {
            get { return (string)GetValue(ShowFullThreadTextProperty); }
            set { SetValue(ShowFullThreadTextProperty, value); }
        }

        /// <summary>
        /// Текст кнопки показа треда целиком.
        /// </summary>
        public static readonly DependencyProperty ShowFullThreadTextProperty = DependencyProperty.Register("ShowFullThreadText", typeof(string), typeof(SinglePostCollectionView),
            new PropertyMetadata("Показать целиком"));

        /// <summary>
        /// Кнопка показа поста целиком.
        /// </summary>
        public bool ShowFullPostButton
        {
            get { return (bool)GetValue(ShowFullPostButtonProperty); }
            set { SetValue(ShowFullPostButtonProperty, value); }
        }

        /// <summary>
        /// Кнопка показа поста целиком.
        /// </summary>
        public static readonly DependencyProperty ShowFullPostButtonProperty = DependencyProperty.Register("ShowFullPostButton", typeof(bool), typeof(SinglePostCollectionView),
            new PropertyMetadata(false));

        /// <summary>
        /// Показать тред целиком.
        /// </summary>
        public event ShowFullThreadEventHandler ShowFullThread;

        /// <summary>
        /// Показать пост целиком.
        /// </summary>
        public event ShowFullPostEventHandler ShowFullPost;

        private void PostView_OnShowFullThread(object sender, ShowFullThreadEventArgs e)
        {
            ShowFullThread?.Invoke(this, e);
        }

        private void PostView_OnShowFullPost(object sender, ShowFullPostEventArgs e)
        {
            ShowFullPost?.Invoke(this, e);
        }

        /// <summary>
        /// Получить индекс видимого элемента.
        /// </summary>
        /// <returns>Индекс.</returns>
        public IPostViewModel GetTopViewIndex()
        {
            return MainList.SelectedItem as IPostViewModel;
        }

        /// <summary>
        /// Показать видимый элемент.
        /// </summary>
        /// <param name="post">Пост.</param>
        public void ScrollIntoView(IPostViewModel post)
        {
            try
            {
                if (post != null)
                {
                    MainList.SelectedItem = post;
                }
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        /// <summary>
        /// Выбранный элемент.
        /// </summary>
        public object SelectedItem
        {
            get { return (object) GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Выбранный элемент.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof (object), typeof (SinglePostCollectionView), new PropertyMetadata(null, SelectedItemPropertyChangedCallback));

        private static void SelectedItemPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SinglePostCollectionView;
            if (obj != null)
            {
                obj.SelectedItemChanged?.Invoke(obj, EventArgs.Empty);
                if (e.NewValue != null)
                {
                    if (e.NewValue != obj.CollectionView?.View?.CurrentItem)
                    {
                        obj.CollectionView?.View?.MoveCurrentTo(e.NewValue);
                    }
                }
            }
        }

        private void ViewOnCurrentChanged(object sender, object o)
        {
        }

        private void MainList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CollectionView?.View?.CurrentItem != null)
            {
                if (SelectedItem != CollectionView.View.CurrentItem)
                {
                    SelectedItem = CollectionView.View.CurrentItem;
                }
            }
        }


        public event EventHandler SelectedItemChanged; 

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            BackButtonClick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Нажата кнопка "назад".
        /// </summary>
        public event EventHandler BackButtonClick;

        private void GoButton_OnClick(object sender, RoutedEventArgs e)
        {
            GoButtonClick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Нажата кнопка "перейти".
        /// </summary>
        public event EventHandler GoButtonClick;

        private readonly Lazy<IStyleManager> styleManager = new Lazy<IStyleManager>(() => StyleManagerFactory.Current.GetManager());

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager => styleManager.Value;

        private async void CopyTextFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IPostViewModel;
                if (t != null)
                {
                    var dp = new DataPackage();
                    var txt = t.Text.GetPlainText().Aggregate(new StringBuilder(), (sb, s) => sb.AppendLine(s)).ToString();
                    dp.SetText(txt);
                    Clipboard.SetContent(dp);
                    Clipboard.Flush();
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private async void CopyLinkFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IPostViewModel;
                if (t?.Link != null)
                {
                    var uri = t.Link.GetWebLink();
                    if (uri != null)
                    {
                        var dp = new DataPackage();
                        dp.SetText(uri.ToString());
                        dp.SetWebLink(uri);
                        Clipboard.SetContent(dp);
                        Clipboard.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private async void PostQuoteFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IPostViewModel;
                if (t != null)
                {
                    var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
                    ServiceLocator.Current.GetServiceOrThrow<IPageNavigationService>().Navigate(new PostingNavigationTarget(t.ParentLink, t.Text) { QuotePost = linkTransform.GetBackLinkDisplayString(t.Link) });
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        private void PostView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var postView = sender as PostView;
            postView?.SetBinding(PostView.MaxLinesProperty, new Binding() { Source = this, Path = new PropertyPath("MaxLines") });
            postView?.SetBinding(PostView.ShowCounterProperty, new Binding() { Source = this, Path = new PropertyPath("ShowCounter") });
            postView?.SetBinding(PostView.ShowPreviewSeparatorProperty, new Binding() { Source = this, Path = new PropertyPath("ShowPreviewSeparator") });
            postView?.SetBinding(PostView.ShowFullThreadTextProperty, new Binding() { Source = this, Path = new PropertyPath("ShowFullThreadText") });
            postView?.SetBinding(PostView.ShowFullPostButtonProperty, new Binding() { Source = this, Path = new PropertyPath("ShowFullPostButton") });
        }
    }
}
