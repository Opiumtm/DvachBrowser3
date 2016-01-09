using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Менеджер всплывающих панелей.
    /// </summary>
    public sealed class ContentPopupManager : DependencyObject
    {
        /// <summary>
        /// Менеджер.
        /// </summary>
        public static DependencyProperty ManagerProperty = DependencyProperty.RegisterAttached("Manager", typeof(ContentPopupManager), typeof(ContentPopupManager), 
            new PropertyMetadata(null, ManagerPropertyChangedCallback));

        /// <summary>
        /// Установить менеджер.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <param name="value">Значение.</param>
        public static void SetManager(UIElement element, ContentPopupManager value)
        {
            element.SetValue(ManagerProperty, value);
        }

        /// <summary>
        /// Получить менеджер.
        /// </summary>
        /// <param name="element">Элемент.</param>
        /// <returns>Менеджер.</returns>
        public static ContentPopupManager GetManager(UIElement element)
        {
            return (ContentPopupManager) element.GetValue(ManagerProperty);
        }        

        private static void ManagerPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (e.OldValue as ContentPopupManager)?.Detach(d);
            (e.NewValue as ContentPopupManager)?.Attach(d);
        }

        private readonly Dictionary<ContentPopup, ContentPopupInfo> popups = new Dictionary<ContentPopup, ContentPopupInfo>();

        private void Attach(DependencyObject d)
        {
            var popup = d as ContentPopup;
            if (popup != null)
            {
                if (!popups.ContainsKey(popup))
                {
                    var info = new ContentPopupInfo();
                    popups.Add(popup, info);
                    info.IsContentVisibleToken = popup.RegisterPropertyChangedCallback(ContentPopup.IsContentVisibleProperty, IsContentVisibleCallback);
                }
            }
        }

        private void Detach(DependencyObject d)
        {
            var popup = d as ContentPopup;
            if (popup != null)
            {
                if (popups.ContainsKey(popup))
                {
                    var info = popups[popup];
                    popups.Remove(popup);
                    popup.UnregisterPropertyChangedCallback(ContentPopup.IsContentVisibleProperty, info.IsContentVisibleToken);
                }
            }
        }

        private void IsContentVisibleCallback(DependencyObject sender, DependencyProperty dp)
        {
            var popup = sender as ContentPopup;
            if (popup != null)
            {
                if (popups.ContainsKey(popup))
                {
                    var isVisible = popup.IsContentVisible;
                    if (isVisible)
                    {
                        var toHide = popups.Keys.Where(item => item != popup && item.IsContentVisible);
                        foreach (var item in toHide)
                        {
                            item.IsContentVisible = false;
                        }
                    }
                }
            }
        }

        private class ContentPopupInfo
        {
            public long IsContentVisibleToken;
        }
    }
}