using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class ContentPopup : UserControl, IWeakEventCallback, INotifyPropertyChanged
    {
        public ContentPopup()
        {
            this.InitializeComponent();
            MainView.DataContext = this;
            Loaded += (sender, e) => SetContentMargin();
            Shell.IsNarrowViewChanged.AddCallback(this);
            OnPropertyChanged(nameof(IsShowCloseButton));
        }

        private void ContentVisibleChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                if (newValue)
                {
                    (Resources["ShowContentAnimation"] as Storyboard)?.Begin();
                }
                else
                {
                    (Resources["HideContentAnimation"] as Storyboard)?.Begin();
                }
                IsContentVisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Содержимое видимо.
        /// </summary>
        public bool IsContentVisible
        {
            get { return (bool) GetValue(IsContentVisibleProperty); }
            set { SetValue(IsContentVisibleProperty, value); }
        }

        /// <summary>
        /// Содержимое видимо.
        /// </summary>
        public static readonly DependencyProperty IsContentVisibleProperty = DependencyProperty.Register("IsContentVisible", typeof (bool), typeof (ContentPopup),
            new PropertyMetadata(false, ContentVisibleCallback));

        private static void ContentVisibleCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ContentPopup)?.ContentVisibleChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Содержимое.
        /// </summary>
        public object ViewContent
        {
            get { return (object) GetValue(ViewContentProperty); }
            set { SetValue(ViewContentProperty, value); }
        }

        /// <summary>
        /// Содержимое.
        /// </summary>
        public static readonly DependencyProperty ViewContentProperty = DependencyProperty.Register("ViewContent", typeof (object), typeof (ContentPopup),
            new PropertyMetadata(null));

        /// <summary>
        /// Шаблон содержимого.
        /// </summary>
        public DataTemplate ViewContentTemplate
        {
            get { return (DataTemplate) GetValue(ViewContentTemplateProperty); }
            set { SetValue(ViewContentTemplateProperty, value); }
        }

        /// <summary>
        /// Шаблон содержимого.
        /// </summary>
        public static readonly DependencyProperty ViewContentTemplateProperty = DependencyProperty.Register("ViewContentTemplate", typeof (DataTemplate), typeof (ContentPopup),
            new PropertyMetadata(null));

        /// <summary>
        /// Выбор шаблона содержимого.
        /// </summary>
        public DataTemplateSelector ViewContentTemplateSelector
        {
            get { return (DataTemplateSelector) GetValue(ViewContentTemplateSelectorProperty); }
            set { SetValue(ViewContentTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Выбор шаблона содержимого.
        /// </summary>
        public static readonly DependencyProperty ViewContentTemplateSelectorProperty = DependencyProperty.Register("ViewContentTemplateSelector", typeof (DataTemplateSelector), typeof (ContentPopup),
            new PropertyMetadata(null));

        /// <summary>
        /// Скрывать кнопку закрытия.
        /// </summary>
        public bool HideCloseButton
        {
            get { return (bool) GetValue(HideCloseButtonProperty); }
            set { SetValue(HideCloseButtonProperty, value); }
        }

        /// <summary>
        /// Скрывать кнопку закрытия.
        /// </summary>
        public static readonly DependencyProperty HideCloseButtonProperty = DependencyProperty.Register("HideCloseButton", typeof (bool), typeof (ContentPopup), new PropertyMetadata(false, HideCloseButtonPropertyChangedCallback));

        private static void HideCloseButtonPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as ContentPopup;
            obj?.OnPropertyChanged(nameof(IsShowCloseButton));
        }

        /// <summary>
        /// Есть хардварная кнопка "назад".
        /// </summary>
        public bool IsHardwareBackButtonPresent
        {
            get
            {
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                {
                    return true;
                }
                return false;
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            IsContentVisible = false;
        }

        /// <summary>
        /// Изменено состояние видимости.
        /// </summary>
        public event EventHandler IsContentVisibleChanged;

        private void SetContentMargin()
        {
            if (!Shell.Instance?.IsNarrowView ?? false)
            {
                BodyContainer.Margin = new Thickness(24, 5, 24, 5);
            }
            else
            {
                BodyContainer.Margin = new Thickness(5);
            }
        }

        /// <summary>
        /// Показывать кнопку закрытия.
        /// </summary>
        public bool IsShowCloseButton => !IsHardwareBackButtonPresent && !HideCloseButton;

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
                SetContentMargin();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
