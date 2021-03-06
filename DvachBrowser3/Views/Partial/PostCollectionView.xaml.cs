﻿using System;
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
using DvachBrowser3.Engines;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class PostCollectionView : UserControl, IPostCollectionVisualIndexStore
    {
        public PostCollectionView()
        {
            this.InitializeComponent();
            BindingRoot.DataContext = this;
            this.Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Unloaded -= OnUnloaded;
            Bindings.StopTracking();
            BindingRoot.DataContext = null;
            ViewModel = null;
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostCollectionViewModel ViewModel
        {
            get { return (IPostCollectionViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IPostCollectionViewModel), typeof (PostCollectionView),
            new PropertyMetadata(null));

        /// <summary>
        /// Максимальное количество строк.
        /// </summary>
        public int MaxLines
        {
            get { return (int) GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        /// <summary>
        /// Максимальное количество строк.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof (int), typeof (PostCollectionView),
            new PropertyMetadata(20));

        /// <summary>
        /// Показывать счётчик постов.
        /// </summary>
        public bool ShowCounter
        {
            get { return (bool) GetValue(ShowCounterProperty); }
            set { SetValue(ShowCounterProperty, value); }
        }

        /// <summary>
        /// Показывать счётчик постов.
        /// </summary>
        public static readonly DependencyProperty ShowCounterProperty = DependencyProperty.Register("ShowCounter", typeof (bool), typeof (PostCollectionView),
            new PropertyMetadata(true));

        /// <summary>
        /// Показывать разделитель превью треда.
        /// </summary>
        public bool ShowPreviewSeparator
        {
            get { return (bool) GetValue(ShowPreviewSeparatorProperty); }
            set { SetValue(ShowPreviewSeparatorProperty, value); }
        }

        /// <summary>
        /// Показывать разделитель превью треда.
        /// </summary>
        public static readonly DependencyProperty ShowPreviewSeparatorProperty = DependencyProperty.Register("ShowPreviewSeparator", typeof (bool), typeof (PostCollectionView),
            new PropertyMetadata(false));

        /// <summary>
        /// Заголовок.
        /// </summary>
        public object Header
        {
            get { return (object) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof (object), typeof (PostCollectionView),
            new PropertyMetadata(null));

        /// <summary>
        /// Нижний заголовок.
        /// </summary>
        public object Footer
        {
            get { return (object) GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        /// <summary>
        /// Нижний заголовок.
        /// </summary>
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof (object), typeof (PostCollectionView),
            new PropertyMetadata(null));

        /// <summary>
        /// Шаблон заголовка.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate) GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Шаблон заголовка.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof (DataTemplate), typeof (PostCollectionView),
            new PropertyMetadata(null));

        /// <summary>
        /// Шаблон нижнего заголовка.
        /// </summary>
        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate) GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        /// <summary>
        /// Шаблон нижнего заголовка.
        /// </summary>
        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof (DataTemplate), typeof (PostCollectionView),
            new PropertyMetadata(null));

        /// <summary>
        /// Текст кнопки показа треда целиком.
        /// </summary>
        public string ShowFullThreadText
        {
            get { return (string) GetValue(ShowFullThreadTextProperty); }
            set { SetValue(ShowFullThreadTextProperty, value); }
        }

        /// <summary>
        /// Текст кнопки показа треда целиком.
        /// </summary>
        public static readonly DependencyProperty ShowFullThreadTextProperty = DependencyProperty.Register("ShowFullThreadText", typeof (string), typeof (PostCollectionView),
            new PropertyMetadata("Показать целиком"));

        /// <summary>
        /// Кнопка показа поста целиком.
        /// </summary>
        public bool ShowFullPostButton
        {
            get { return (bool) GetValue(ShowFullPostButtonProperty); }
            set { SetValue(ShowFullPostButtonProperty, value); }
        }

        /// <summary>
        /// Кнопка показа поста целиком.
        /// </summary>
        public static readonly DependencyProperty ShowFullPostButtonProperty = DependencyProperty.Register("ShowFullPostButton", typeof (bool), typeof (PostCollectionView),
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
            try
            {
                var r = MainList.GetVisibleToWindowElements<IPostViewModel>().OrderBy(o => o.Item1).FirstOrDefault();
                return r?.Item2;
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
                return null;
            }
        }

        /// <summary>
        /// Показать тред.
        /// </summary>
        /// <param name="post">Пост.</param>
        public void ScrollIntoView(IPostViewModel post)
        {
            if (post == null)
            {
                return;
            }
            try
            {
                MainList.ScrollIntoView(post, ScrollIntoViewAlignment.Leading);
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

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

        private async void ShowFullScreenFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = sender as FrameworkElement;
                var t = f?.Tag as IPostViewModel;
                if (t != null)
                {
                    ShowFullScreen?.Invoke(this, new ShowFullPostEventArgs(t));
                }
            }
            catch (Exception ex)
            {
                await AppHelpers.ShowError(ex);
            }
        }

        /// <summary>
        /// Показать во весь экран.
        /// </summary>
        public event ShowFullPostEventHandler ShowFullScreen;

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

        private readonly BoolToVisibilityConverter conv = new BoolToVisibilityConverter();

        private void FullScreenMenuFlyoutItem_OnLoaded(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            menuItem?.SetBinding(MenuFlyoutItem.VisibilityProperty, new Binding() { Source = this, Path = new PropertyPath("ShowFullPostButton"), Converter = conv, ConverterParameter = "false"});
        }

        private void MainList_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var templateRoot = args.ItemContainer.ContentTemplateRoot as PostView;
            if (templateRoot != null)
            {
                templateRoot.RenderPhase = 0;
                args.RegisterUpdateCallback(Phase1Callback);
            }
        }

        private void Phase1Callback(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var templateRoot = args.ItemContainer.ContentTemplateRoot as PostView;
            if (templateRoot != null)
            {
                templateRoot.RenderPhase = 1;
                args.RegisterUpdateCallback(Phase2Callback);
            }
        }

        private void Phase2Callback(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var templateRoot = args.ItemContainer.ContentTemplateRoot as PostView;
            if (templateRoot != null)
            {
                templateRoot.RenderPhase = 2;
            }
        }
    }
}
