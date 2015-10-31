using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Сервисы страницы.
    /// </summary>
    public class PageServices
    {
        /// <summary>
        /// Сервисы страницы.
        /// </summary>
        public static readonly DependencyProperty ServicesProperty = DependencyProperty.RegisterAttached("Services", typeof(PageServiceCollection), typeof(PageServices), new PropertyMetadata(null, PropertyChangedCallback));

        /// <summary>
        /// Установить сервисы страницы.
        /// </summary>
        /// <param name="d">Объект.</param>
        /// <param name="v">Сервисы.</param>
        public static void SetServices(DependencyObject d, PageServiceCollection v)
        {
            d.SetValue(ServicesProperty, v);
        }

        /// <summary>
        /// Получить сервисы страницы.
        /// </summary>
        /// <param name="d">Объект.</param>
        /// <returns>Сервисы.</returns>
        public static PageServiceCollection GetServices(DependencyObject d)
        {
            return (PageServiceCollection)d.GetValue(ServicesProperty);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as PageServiceCollection;
            var newValue = e.NewValue as PageServiceCollection;
            oldValue?.Attach(null);
            newValue?.Attach(d as Page);
        }
    }
}