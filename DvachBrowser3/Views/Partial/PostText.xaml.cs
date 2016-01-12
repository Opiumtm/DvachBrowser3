using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class PostText : UserControl, INotifyPropertyChanged
    {
        private readonly SizeChangeDelayHelper sizeChangeDelay = new SizeChangeDelayHelper(TimeSpan.FromSeconds(0.25));

        public PostText()
        {
            this.InitializeComponent();
            MainGrid.SizeChanged += sizeChangeDelay.OnSizeChanged;
            sizeChangeDelay.SizeUpdated += SizeChangeDelayOnSizeUpdated;
            RefreshView();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            isLoaded = true;
            RefreshView();
        }

        private void SizeChangeDelayOnSizeUpdated(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void ClearOldData()
        {
            if (isCleared) return;
            ScrollablePlaceholderGrid.Children.Clear();
            isRendered = false;
            isCleared = true;
        }

        private bool isCleared = true;

        private bool isRendered = false;

        private double? lastWidth = null;

        private bool? lastNarrow = null;

        private Guid? lastId = null;

        private int? lastMaxLines = null;

        private bool isLoaded = false;

        private void RefreshView()
        {
            if (!isLoaded)
            {
                return;
            }
            if (ViewModel == null || ActualWidth < 50)
            {
                ClearOldData();
                return;
            }

            var isNarrow = Shell.Instance.IsNarrowView;

            if (lastWidth == ActualWidth && lastNarrow == isNarrow && ViewModel.UniqueId == lastId && lastMaxLines == MaxLines && isRendered)
            {
                return;
            }

            var renderFactory = new RenderTextElementFactory(ViewModel, isNarrow);
            var canvas = new Canvas();

            ScrollablePlaceholderGrid.Measure(new Size(MainGrid.ActualWidth, MainGrid.ActualHeight));
            canvas.Width = ScrollablePlaceholderGrid.ActualWidth;

            canvas.Height = 5;
            canvas.HorizontalAlignment = HorizontalAlignment.Left;
            canvas.VerticalAlignment = VerticalAlignment.Top;

            var logic = new TextRenderLogic(new TextRenderCommandFormer(), new CanvasTextRenderCommandExecutor(canvas, renderFactory, new WordSplitter()));
            logic.MaxLines = MaxLines > 0 ? (int?)MaxLines : null;
            ViewModel.RenderText(logic);

            ClearOldData();
            ScrollablePlaceholderGrid.Children.Add(canvas);

            ExceedLines = logic.ExceedLines && MaxLines > 0;
            TextRendered?.Invoke(this, EventArgs.Empty);

            isCleared = false;
            isRendered = true;
            lastWidth = ActualWidth;
            lastNarrow = isNarrow;
            lastId = ViewModel.UniqueId;
            lastMaxLines = MaxLines;
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
        public int MaxLines
        {
            get { return (int) GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        /// <summary>
        /// Максимальное количество линий.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof (int), typeof (PostText),
            new PropertyMetadata(0, RefreshNeedCallback));

        private static void RefreshNeedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as PostText;
            obj?.RefreshView();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Закончился лимит.
        /// </summary>
        public bool ExceedLines
        {
            get { return (bool) GetValue(ExceedLinesProperty); }
            set { SetValue(ExceedLinesProperty, value); }
        }

        /// <summary>
        /// Закончился лимит.
        /// </summary>
        public static readonly DependencyProperty ExceedLinesProperty = DependencyProperty.Register("ExceedLines", typeof (bool), typeof (PostText),
            new PropertyMetadata(false));

        /// <summary>
        /// Текст отрисован.
        /// </summary>
        public event EventHandler TextRendered;
    }
}
