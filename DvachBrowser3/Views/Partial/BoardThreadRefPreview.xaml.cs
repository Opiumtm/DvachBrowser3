using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public sealed partial class BoardThreadRefPreview : UserControl, INotifyPropertyChanged, IWeakEventCallback
    {
        public BoardThreadRefPreview()
        {
            this.InitializeComponent();
            BindingRoot.DataContext = this;
            this.Loaded += OnLoaded;
            this.Unloaded += (sender, e) =>
            {
                Bindings.StopTracking();
                ViewModel = null;
                BindingRoot.DataContext = null;
            };
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Shell.IsNarrowViewChanged.AddCallback(this);
            SizeStateChanged();
        }

        private void SizeStateChanged()
        {
            if (Shell.Instance?.IsNarrowView ?? false)
            {
                VerticalHeaderVisibility = Visibility.Visible;
                HorizontalHeaderVisibility = Visibility.Collapsed;
            }
            else
            {
                VerticalHeaderVisibility = Visibility.Collapsed;
                HorizontalHeaderVisibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public IThreadPreviewViewModel ViewModel
        {
            get { return (IThreadPreviewViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// Модель представления.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof (IThreadPreviewViewModel), typeof (BoardThreadRefPreview),
            new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dd = d as BoardThreadRefPreview;
            if (dd != null)
            {
                if (e.Property == ViewModelProperty)
                {
                    // ReSharper disable once ExplicitCallerInfoArgument
                    dd.OnPropertyChanged("PostCollectionViewModel");
                }
            }
        }

        public IPostCollectionViewModel PostCollectionViewModel => ViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Показ вертикального хидера.
        /// </summary>
        public Visibility VerticalHeaderVisibility
        {
            get { return (Visibility) GetValue(VerticalHeaderVisibilityProperty); }
            set { SetValue(VerticalHeaderVisibilityProperty, value); }
        }

        /// <summary>
        /// Показ вертикального хидера.
        /// </summary>
        public static readonly DependencyProperty VerticalHeaderVisibilityProperty = DependencyProperty.Register("VerticalHeaderVisibility", typeof (Visibility), typeof (BoardThreadRefPreview),
            new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Показ горизонтального хидера.
        /// </summary>
        public Visibility HorizontalHeaderVisibility
        {
            get { return (Visibility) GetValue(HorizontalHeaderVisibilityProperty); }
            set { SetValue(HorizontalHeaderVisibilityProperty, value); }
        }

        /// <summary>
        /// Показ горизонтального хидера.
        /// </summary>
        public static readonly DependencyProperty HorizontalHeaderVisibilityProperty = DependencyProperty.Register("HorizontalHeaderVisibility", typeof (Visibility), typeof (BoardThreadRefPreview),
            new PropertyMetadata(Visibility.Visible));

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
                SizeStateChanged();
            }
        }

        /// <summary>
        /// Фаза оторбажения.
        /// </summary>
        public int Phase
        {
            get { return (int) GetValue(PhaseProperty); }
            set { SetValue(PhaseProperty, value); }
        }

        /// <summary>
        /// Фаза оторбажения.
        /// </summary>
        public static readonly DependencyProperty PhaseProperty = DependencyProperty.Register("Phase", typeof (int), typeof (BoardThreadRefPreview), new PropertyMetadata(-1, PhasePropertyChangedCallback));

        private void PhaseChanged()
        {
            switch (Phase)
            {
                case 0:
                    PostText.RenderSuspended = true;
                    ImagePreview.LoadingSuspended = true;
                    break;
                case 1:
                    PostText.RenderSuspended = false;
                    ImagePreview.LoadingSuspended = true;
                    break;
                default:
                    PostText.RenderSuspended = false;
                    ImagePreview.LoadingSuspended = false;
                    break;
            }
        }

        private static void PhasePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BoardThreadRefPreview)d).PhaseChanged();
        }
    }

    public class BoardThreadRefPreviewImageWidthValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double) value + 4.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
