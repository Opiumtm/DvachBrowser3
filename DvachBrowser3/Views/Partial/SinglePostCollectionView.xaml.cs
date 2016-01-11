﻿using System;
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
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class SinglePostCollectionView : UserControl
    {
        public SinglePostCollectionView()
        {
            this.InitializeComponent();
        }

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
            new PropertyMetadata(null));

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
    }
}