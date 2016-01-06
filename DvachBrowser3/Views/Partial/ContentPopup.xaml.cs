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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvachBrowser3.Views.Partial
{
    public sealed partial class ContentPopup : UserControl
    {
        public ContentPopup()
        {
            this.InitializeComponent();
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
            }
        }

        /// <summary>
        /// Отступ всплывающего содержимого.
        /// </summary>
        public Thickness PopupMargin
        {
            get { return (Thickness) GetValue(PopupMarginProperty); }
            set { SetValue(PopupMarginProperty, value); }
        }

        /// <summary>
        /// Отступ всплывающего содержимого.
        /// </summary>
        public static readonly DependencyProperty PopupMarginProperty = DependencyProperty.Register("PopupMargin", typeof (Thickness), typeof (ContentPopup),
            new PropertyMetadata(new Thickness(5)));

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
    }
}
