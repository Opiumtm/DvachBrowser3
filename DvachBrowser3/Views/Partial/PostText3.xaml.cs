using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvachBrowser3.ViewModels;
using Ipatov.MarkupRender;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class PostText3 : UserControl
    {
        private readonly DvachTextRenderStyle _textRenderStyle = new DvachTextRenderStyle();

        private readonly MarkupRenderData _markupRenderData;

        public PostText3()
        {
            this.InitializeComponent();
            _markupRenderData = new MarkupRenderData() { Commands = null, Style = _textRenderStyle };
            RenderControl.RenderData = _markupRenderData;
            RenderControl.ExceedsLinesChanged += RenderControlOnExceedsLinesChanged;
            RenderControl.TextTapped += RenderControlOnTextTapped;
            Unloaded += OnUnloaded;
        }

        private void RenderControlOnTextTapped(object sender, IRenderCommand renderCommand)
        {
            ViewModel?.OnLinkClick(renderCommand);
        }

        private void RenderControlOnExceedsLinesChanged(object sender, EventArgs eventArgs)
        {
            ExceedLines = RenderControl.ExceedsLines;
            TextRendered?.Invoke(this, EventArgs.Empty);
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _textRenderStyle.Dispose();
            RenderControl.ExceedsLinesChanged -= RenderControlOnExceedsLinesChanged;
            RenderControl.TextTapped -= RenderControlOnTextTapped;
        }

        private void ViewModelChanged()
        {
            _markupRenderData.Commands = ViewModel?.GetRenderCommands();
        }

        private void MaxLinesChanged()
        {
            _textRenderStyle.MaxLines = MaxLines > 0 ? (int?) MaxLines : null;
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof (IPostTextViewModel), typeof (PostText3), new PropertyMetadata(default(IPostTextViewModel), ViewModelChangedCallback));

        private static void ViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PostText3)?.ViewModelChanged();
        }

        public IPostTextViewModel ViewModel
        {
            get { return (IPostTextViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register(
            "MaxLines", typeof (int), typeof (PostText3), new PropertyMetadata(default(int), MaxLinesChangedCallback));

        private static void MaxLinesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PostText3)?.MaxLinesChanged();
        }

        public int MaxLines
        {
            get { return (int) GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        public static readonly DependencyProperty ExceedLinesProperty = DependencyProperty.Register(
            "ExceedLines", typeof (bool), typeof (PostText3), new PropertyMetadata(default(bool)));

        public bool ExceedLines
        {
            get { return (bool) GetValue(ExceedLinesProperty); }
            set { SetValue(ExceedLinesProperty, value); }
        }

        /// <summary>
        /// Текст отрисован.
        /// </summary>
        public event EventHandler TextRendered;
    }
}
