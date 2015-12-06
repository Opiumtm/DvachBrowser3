using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник для строки статуса.
    /// </summary>
    public static class StatusBarHelper
    {
        private static readonly Lazy<bool> IsStatusBarPresentGetter = new Lazy<bool>(GetIsStatusBarPresent);

        private static bool GetIsStatusBarPresent()
        {
            return Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");
        }

        /// <summary>
        /// Статус бар присутствует в системе.
        /// </summary>
        public static bool IsStatusBarPresent => IsStatusBarPresentGetter.Value;

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
    }
}