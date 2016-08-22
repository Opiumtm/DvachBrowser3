using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DvachBrowser3.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class ProgressIndicatorControl : UserControl, INotifyPropertyChanged
    {
        public ProgressIndicatorControl()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            BindingRoot.DataContext = this;
            this.Unloaded += (sender, e) =>
            {
                Bindings.StopTracking();
                BindingRoot.DataContext = null;
                ViewModel = null;
            };
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateAllSync();
        }

        private async void UpdateAllSync()
        {
            await UpdateAll();
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IOperationProgressViewModel ViewModel
        {
            get { return (IOperationProgressViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IOperationProgressViewModel), typeof (ProgressIndicatorControl),
            new PropertyMetadata(null, ViewModelChangedCallback));

        private static void ViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as ProgressIndicatorControl;
            obj?.ViewModelChanged(e);
        }

        private async void ViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as IOperationProgressViewModel;
            var oldValue = e.OldValue as IOperationProgressViewModel;
            if (newValue != null)
            {
                newValue.PropertyChanged += ViewModelPropertyChanged;
            }
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ViewModelPropertyChanged;
            }
            await UpdateAll();
        }

        private async void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IOperationProgressViewModel.IsActive))
            {
                await UpdateVisibility();
            }
            if (e.PropertyName == nameof(IOperationProgressViewModel.Message))
            {
                UpdateMessage();
            }
            if (e.PropertyName == nameof(IOperationProgressViewModel.Progress) || e.PropertyName == nameof(IOperationProgressViewModel.IsIndeterminate))
            {
                UpdateProgress();
            }
        }

        /// <summary>
        /// Показывать сообщение.
        /// </summary>
        public bool ShowMessage
        {
            get { return (bool) GetValue(ShowMessageProperty); }
            set { SetValue(ShowMessageProperty, value); }
        }

        /// <summary>
        /// Показывать сообщение.
        /// </summary>
        public static readonly DependencyProperty ShowMessageProperty = DependencyProperty.Register("ShowMessage", typeof (bool), typeof (ProgressIndicatorControl),
            new PropertyMetadata(false, ShowMessageChangedCallback));

        private static void ShowMessageChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as ProgressIndicatorControl;
            obj?.ShowMessageChanged();
        }

        private void ShowMessageChanged()
        {
            UpdateMessage();
        }

        /// <summary>
        /// Возникает при смене значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Показывать.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                if (StatusBarHelper.IsStatusBarPresent)
                {
                    return false;
                }
                return ViewModel?.IsActive ?? false;
            }
        }

        /// <summary>
        /// Отображать сообщение.
        /// </summary>
        public bool IsShowMessage => ShowMessage && !string.IsNullOrEmpty(ViewModel?.Message);

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message => ViewModel?.Message ?? "";

        /// <summary>
        /// Неясное значение.
        /// </summary>
        public bool IsIndeterminate => ViewModel?.IsIndeterminate ?? true;

        /// <summary>
        /// Прогресс.
        /// </summary>
        public double Progress => ViewModel?.Progress ?? 0.0;

        private async Task UpdateAll()
        {
            await UpdateVisibility();
            UpdateMessage();
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            try
            {
                var bar = StatusBarHelper.StatusBar;
                if (bar != null)
                {
                    var isd = ViewModel?.IsIndeterminate ?? true;
                    var d = (ViewModel?.Progress ?? 0.0) / 100.0;
                    bar.ProgressIndicator.ProgressValue = isd ? null : (double?)d;
                }
                OnPropertyChanged(nameof(IsIndeterminate));
                OnPropertyChanged(nameof(Progress));
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private void UpdateMessage()
        {
            try
            {
                var bar = StatusBarHelper.StatusBar;
                if (bar != null)
                {
                    if (ShowMessage)
                    {
                        bar.ProgressIndicator.Text = ViewModel?.Message ?? "";
                    }
                    else
                    {
                        bar.ProgressIndicator.Text = "";
                    }
                }
                OnPropertyChanged(nameof(IsShowMessage));
                OnPropertyChanged(nameof(Message));
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }

        private async Task UpdateVisibility()
        {
            try
            {
                var bar = StatusBarHelper.StatusBar;
                if (bar != null)
                {
                    if (ViewModel?.IsActive ?? false)
                    {
                        await bar.ProgressIndicator.ShowAsync();
                    }
                    else
                    {
                        await bar.ProgressIndicator.HideAsync();
                    }
                }
                OnPropertyChanged(nameof(IsVisible));
            }
            catch (Exception ex)
            {
                DebugHelper.BreakOnError(ex);
            }
        }
    }
}
