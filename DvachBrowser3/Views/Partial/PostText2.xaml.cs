using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.Styles;
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
            BindingRoot.DataContext = this;
            RenderControl.TextRendered += (sender, e) =>
            {
                ExceedLines = RenderControl.ExceedLines;
                TextRendered?.Invoke(this, e);
            };
            Shell.IsNarrowViewChanged.AddCallback(this);
            this.Unloaded += (sender, e) =>
            {
                ViewModel = null;
            };
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (ViewModel != null && result != null && linkAttribute != null)
                {
                    RenderLinkClickHelper.SetupLinkActions(result, linkAttribute, ViewModel);
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager { get; } = StyleManagerFactory.Current.GetManager();

        /// <summary>
        /// Размер шрифта.
        /// </summary>
        double ITextRender2RenderCallback.PostFontSize => StyleManager.Text.PostFontSize;

        /// <summary>
        /// Нормальный текст.
        /// </summary>
        Brush ITextRender2RenderCallback.PostNormalTextBrush => Application.Current.Resources["PostNormalTextBrush"] as Brush;

        /// <summary>
        /// Нормальный текст.
        /// </summary>
        Color ITextRender2RenderCallback.PostNormalTextColor => (Application.Current.Resources["PostNormalTextBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        /// <summary>
        /// Задний фон спойлера.
        /// </summary>
        Brush ITextRender2RenderCallback.PostSpoilerBackgroundBrush => Application.Current.Resources["PostSpoilerBackgroundBrush"] as Brush;

        /// <summary>
        /// Задний фон спойлера.
        /// </summary>
        Color ITextRender2RenderCallback.PostSpoilerBackgroundColor => (Application.Current.Resources["PostSpoilerBackgroundBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        /// <summary>
        /// Передний фон спойлера.
        /// </summary>
        Brush ITextRender2RenderCallback.PostSpoilerTextBrush => Application.Current.Resources["PostSpoilerTextBrush"] as Brush;

        /// <summary>
        /// Передний фон спойлера.
        /// </summary>
        Color ITextRender2RenderCallback.PostSpoilerTextColor => (Application.Current.Resources["PostSpoilerTextBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        /// <summary>
        /// Цвет квоты.
        /// </summary>
        Brush ITextRender2RenderCallback.PostQuoteTextBrush => Application.Current.Resources["PostQuoteTextBrush"] as Brush;

        /// <summary>
        /// Цвет квоты.
        /// </summary>
        Color ITextRender2RenderCallback.PostQuoteTextColor => (Application.Current.Resources["PostQuoteTextBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        /// <summary>
        /// Цвет ссылки.
        /// </summary>
        Brush ITextRender2RenderCallback.PostLinkTextBrush => Application.Current.Resources["PostLinkTextBrush"] as Brush;

        /// <summary>
        /// Цвет ссылки.
        /// </summary>
        Color ITextRender2RenderCallback.PostLinkTextColor => (Application.Current.Resources["PostQuoteTextBrush"] as SolidColorBrush)?.Color ?? Colors.Black;

        /// <summary>
        /// Идентификатор программы.
        /// </summary>
        public Guid ProgramId => ViewModel?.UniqueId ?? Guid.Empty;

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
