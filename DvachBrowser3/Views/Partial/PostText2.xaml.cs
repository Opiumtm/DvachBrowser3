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
using DvachBrowser3.TextRender;
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class PostText2 : UserControl, ITextRender2ControlCallback, IWeakEventCallback
    {
        public PostText2()
        {
            this.InitializeComponent();
            RenderControl.TextRendered += (sender, e) =>
            {
                ExceedLines = RenderControl.ExceedLines;
                TextRendered?.Invoke(this, e);
            };
            Shell.IsNarrowViewChanged.AddCallback(this);
        }

        private void RefreshView()
        {
            RenderControl.InvalidateRenderedState();
        }

        /// <summary>
        /// Обратный вызов для ссылки.
        /// </summary>
        /// <param name="result">Результат рендеринга.</param>
        /// <param name="linkAttribute">Ссылка.</param>
        void ITextRender2RenderCallback.RenderLinkCallback(FrameworkElement result, ITextRenderLinkAttribute linkAttribute)
        {
            if (ViewModel != null && result != null && linkAttribute != null)
            {
                RenderLinkClickHelper.SetupLinkActions(result, linkAttribute, ViewModel);
            }
        }

        /// <summary>
        /// Размер шрифта.
        /// </summary>
        double ITextRender2RenderCallback.PostFontSize => Shell.StyleManager.Text.PostFontSize;

        /// <summary>
        /// Нормальный текст.
        /// </summary>
        Brush ITextRender2RenderCallback.PostNormalTextBrush => Application.Current.Resources["PostNormalTextBrush"] as Brush;

        /// <summary>
        /// Задний фон спойлера.
        /// </summary>
        Brush ITextRender2RenderCallback.PostSpoilerBackgroundBrush => Application.Current.Resources["PostSpoilerBackgroundBrush"] as Brush;

        /// <summary>
        /// Передний фон спойлера.
        /// </summary>
        Brush ITextRender2RenderCallback.PostSpoilerTextBrush => Application.Current.Resources["PostSpoilerTextBrush"] as Brush;

        /// <summary>
        /// Цвет квоты.
        /// </summary>
        Brush ITextRender2RenderCallback.PostQuoteTextBrush => Application.Current.Resources["PostQuoteTextBrush"] as Brush;

        /// <summary>
        /// Цвет ссылки.
        /// </summary>
        Brush ITextRender2RenderCallback.PostLinkTextBrush => Application.Current.Resources["PostLinkTextBrush"] as Brush;

        private ITextRender2RenderProgram program;

        private bool hasProgram;

        /// <summary>
        /// Получить программу рендеринга.
        /// </summary>
        /// <returns>Программа рендеринга.</returns>
        ITextRender2RenderProgram ITextRender2ControlCallback.GetRenderProgram()
        {
            if (!hasProgram)
            {
                program = ViewModel?.CreateProgram();
                hasProgram = true;
            }
            return program;
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostTextViewModel ViewModel
        {
            get { return (IPostTextViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(IPostTextViewModel), typeof(PostText2),
            new PropertyMetadata(null, RefreshNeedCallback));

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(PostText2),
            new PropertyMetadata(0));

        private static void RefreshNeedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as PostText2;
            if (obj != null && e.Property == ViewModelProperty)
            {
                obj.hasProgram = false;
            }
            obj?.RefreshView();
        }

        /// <summary>
        /// Закончился лимит.
        /// </summary>
        public bool ExceedLines
        {
            get { return (bool)GetValue(ExceedLinesProperty); }
            set { SetValue(ExceedLinesProperty, value); }
        }

        /// <summary>
        /// Закончился лимит.
        /// </summary>
        public static readonly DependencyProperty ExceedLinesProperty = DependencyProperty.Register("ExceedLines", typeof(bool), typeof(PostText2),
            new PropertyMetadata(false));

        /// <summary>
        /// Текст отрисован.
        /// </summary>
        public event EventHandler TextRendered;

        public ITextRender2ControlCallback RenderCallback => this;

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
                RefreshView();
            }
        }
    }
}
