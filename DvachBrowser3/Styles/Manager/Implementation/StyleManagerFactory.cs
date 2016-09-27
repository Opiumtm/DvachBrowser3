﻿using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using DvachBrowser3.Views;

namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Фабрика менеджера стилей.
    /// </summary>
    public sealed class StyleManagerFactory : IStyleManagerFactory
    {
        /// <summary>
        /// Экземпляр фабрики.
        /// </summary>
        public static readonly IStyleManagerFactory Current = new StyleManagerFactory();

        /// <summary>
        /// Получить менеджер.
        /// </summary>
        /// <returns></returns>
        public IStyleManager GetManager()
        {
            var page = Shell.HamburgerMenu?.NavigationService?.FrameFacade?.Content as IStyleManagerFactory;
            if (page == null)
            {
                return new StyleManager();
            }
            return page.GetManager();
        }
    }
}