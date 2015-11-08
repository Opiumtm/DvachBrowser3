using System;
using DvachBrowser3.Links;
using DvachBrowser3.Navigation;

namespace DvachBrowser3
{
    /// <summary>
    /// Помощник навигации.
    /// </summary>
    public static class NavigationHelper
    {
        /// <summary>
        /// Получить ключ навигации.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Ключ навигации.</returns>
        public static INavigationKey GetNavigationKey(this BoardLinkBase link)
        {
            if (link == null)
            {
                return null;
            }
            return ServiceLocator.Current.GetServiceOrThrow<IBoardLinkKeyService>().GetKey(link);
        }

        /// <summary>
        /// Получить ссылку из параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>Ссылка.</returns>
        public static BoardLinkBase GetLinkFromParameter(object parameter)
        {
            var str = parameter as string;
            if (str == null)
            {
                return null;
            }
            var key = ServiceLocator.Current.GetServiceOrThrow<INavigationKeyService>().Deserialize(str);
            return key?.LinkData as BoardLinkBase;
        }
    }
}