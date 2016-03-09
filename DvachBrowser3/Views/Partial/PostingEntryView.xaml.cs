using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using DvachBrowser3.Styles;
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class PostingEntryView : UserControl
    {
        public PostingEntryView()
        {
            this.InitializeComponent();
            MainGrid.DataContext = this;
            this.Loaded += OnLoaded;
            MainGrid.SizeChanged += (sender, e) =>
            {
                AppHelpers.ActionOnUiThread(IsNarrowViewChanged);
            };
            this.Unloaded += (sender, e) =>
            {
                ViewModel = null;
            };
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            AppHelpers.ActionOnUiThread(IsNarrowViewChanged);
        }

        private Task IsNarrowViewChanged()
        {
            if (double.IsNaN(MainGrid.ActualWidth) || double.IsInfinity(MainGrid.ActualWidth))
            {
                return Task.CompletedTask;
            }
            bool isNarrow;
            bool isChange;
            if (MainGrid.ActualWidth >= 680)
            {
                isNarrow = false;
            }
            else
            {
                isNarrow = true;
            }
            if (isNarrow)
            {
                isChange = NarrowUiPanel.Visibility != Visibility.Visible;
                NarrowUiPanel.Visibility = Visibility.Visible;
                NormalUiPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                isChange = NormalUiPanel.Visibility != Visibility.Visible;
                NarrowUiPanel.Visibility = Visibility.Collapsed;
                NormalUiPanel.Visibility = Visibility.Visible;
            }
            if (isChange)
            {
                ViewModel?.Fields?.RaiseChanged();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IPostingViewModel ViewModel
        {
            get { return (IPostingViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IPostingViewModel), typeof (PostingEntryView), new PropertyMetadata(null));

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager { get; } = StyleManagerFactory.Current.GetManager();

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                var vm = tb.Tag as IPostingFieldViewModel<string>;
                if (vm != null)
                {
                    vm.UpdateValueInternal(tb.Text);
                }
            }
        }

        private void MarkupControl_OnApplyMarkup(object sender, ApplyMarkupEventArgs e)
        {
            e.ApplyToTextBox(CommentBox);
        }

        private void MarkupControl2_OnApplyMarkup(object sender, ApplyMarkupEventArgs e)
        {
            e.ApplyToTextBox(CommentBox2);
        }

        private static ApplyMarkupEventHandler CreateBottomBarHandler(WeakReference<TextBox> handler)
        {
            return (sender, e) =>
            {
                TextBox obj;
                if (handler.TryGetTarget(out obj))
                {
                    e.ApplyToTextBox(obj);
                }
            };
        }

        private void IconBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            var cb = sender as ComboBox;
            cb?.SetBinding(ComboBox.SelectedItemProperty, new Binding() { Source = this, Path = new PropertyPath("ViewModel.Fields.Icon.AsBaseIntf.Value"), Mode = BindingMode.TwoWay});
        }

        public MarkupControl CreateAppBarMarkupForNarrowView()
        {
            var mc = new MarkupControl()
            {
                Orientation = Orientation.Horizontal,
                MarkupPadding = new Thickness(2, 6, 2, 6)
            };
            mc.ApplyMarkup += CreateBottomBarHandler(new WeakReference<TextBox>(CommentBox2));
            mc.SetBinding(MarkupControl.MarkupProviderProperty, new Binding() { Source = this, Path = new PropertyPath("ViewModel.Fields.MarkupProvider"), Mode = BindingMode.OneWay});
            return mc;
        }

        /// <summary>
        /// Поле ввода выбрано.
        /// </summary>
        public bool IsNarrowEditFocused
        {
            get { return (bool) GetValue(IsNarrowEditFocusedProperty); }
            set { SetValue(IsNarrowEditFocusedProperty, value); }
        }

        /// <summary>
        /// Поле ввода выбрано.
        /// </summary>
        public static readonly DependencyProperty IsNarrowEditFocusedProperty = DependencyProperty.Register("IsNarrowEditFocused", typeof (bool), typeof (PostingEntryView), new PropertyMetadata(false, PropertyChangedCallback));

        public event EventHandler IsNarrowEditFocusedChanged;

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PostingEntryView)d).IsNarrowEditFocusedChanged?.Invoke(d, EventArgs.Empty);
        }

        private void CommentBox2_OnGotFocus(object sender, RoutedEventArgs e)
        {
            IsNarrowEditFocused = true;
        }

        private void CommentBox2_OnLostFocus(object sender, RoutedEventArgs e)
        {
            IsNarrowEditFocused = false;
        }
    }
}
