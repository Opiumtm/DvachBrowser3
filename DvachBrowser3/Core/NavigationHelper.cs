using System;
using DvachBrowser3.Engines.Makaba;
using DvachBrowser3.Links;
using DvachBrowser3.Navigation;
using DvachBrowser3.TextRender;
using Template10.Services.SerializationService;

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
            var str1 = parameter as string;
            if (str1 == null)
            {
                return null;
            }
            var str = SerializationService.Json.Deserialize<string>(str1);
            if (str == null)
            {
                return null;
            }
            var key = ServiceLocator.Current.GetServiceOrThrow<INavigationKeyService>().Deserialize(str);
            return key?.LinkData as BoardLinkBase;
        }
    }
}