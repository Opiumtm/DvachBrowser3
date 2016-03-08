using System.Runtime.CompilerServices;
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

        private static readonly ConditionalWeakTable<Page, StyleManager> StyleManagers = new ConditionalWeakTable<Page, StyleManager>();

        /// <summary>
        /// Получить менеджер.
        /// </summary>
        /// <returns></returns>
        public IStyleManager GetManager()
        {
            var page = Shell.HamburgerMenu?.NavigationService?.FrameFacade?.Content as Page;
            if (page == null)
            {
                return new StyleManager();
            }
            StyleManager currentManager;
            if (StyleManagers.TryGetValue(page, out currentManager))
            {
                return currentManager;
            }
            var newManager = new StyleManager();
            StyleManagers.Add(page, newManager);
            return newManager;
        }
    }
}