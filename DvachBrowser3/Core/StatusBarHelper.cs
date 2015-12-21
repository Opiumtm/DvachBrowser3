using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник для строки статуса.
    /// </summary>
    public static class StatusBarHelper
    {
        private static readonly Lazy<bool> IsStatusBarPresentGetter = new Lazy<bool>(GetIsStatusBarPresent);

        private static readonly Lazy<bool> IsApplicationViewPresentGetter = new Lazy<bool>(GetIsApplicationViewPresent);

        private static bool GetIsStatusBarPresent()
        {
            return Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");
        }

        private static bool GetIsApplicationViewPresent()
        {
            return Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView");
        }

        /// <summary>
        /// Статус бар присутствует в системе.
        /// </summary>
        public static bool IsStatusBarPresent => IsStatusBarPresentGetter.Value;

        public static bool IsApplicationViewPresent => IsApplicationViewPresentGetter.Value;

        /// <summary>
        /// Строка статуса.
        /// </summary>
        public static Windows.UI.ViewManagement.StatusBar StatusBar
        {
            get
            {
                if (!IsStatusBarPresent)
                {
                    return null;
                }
                return Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            }
        }

        /// <summary>
        /// Представление приложения.
        /// </summary>
        public static Windows.UI.ViewManagement.ApplicationView ApplicationView
        {
            get
            {
                if (!IsApplicationViewPresent)
                {
                    return null;
                }
                return Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            }
        }
    }
}