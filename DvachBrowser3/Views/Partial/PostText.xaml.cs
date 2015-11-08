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
using DvachBrowser3.TextRender;
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class PostText : UserControl
    {
        public PostText()
        {
            this.InitializeComponent();
            MainCanvas.SizeChanged += MainCanvasOnSizeChanged;
            RefreshView();
        }

        private void MainCanvasOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            MainCanvas.Children.Clear();
            if (ViewModel == null || ActualWidth < 50)
            {
                return;
            }
            var renderFactory = new RenderTextElementFactory(ViewModel);
            var logic = new TextRenderLogic(new TextRenderCommandFormer(), new CanvasTextRenderCommandExecutor(MainCanvas, renderFactory, new WordSplitter()));
            logic.MaxLines = MaxLines;
            ViewModel.RenderText(logic);
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostTextViewModel ViewModel
        {
            get { return (IPostTextViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IPostTextViewModel), typeof (PostText),
            new PropertyMetadata(null, RefreshNeedCallback));

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public int? MaxLines
        {
            get { return (int?) GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof (int?), typeof (PostText),
            new PropertyMetadata(new int?(), RefreshNeedCallback));

        private static void RefreshNeedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as PostText;
            obj?.RefreshView();
        }
    }
}
