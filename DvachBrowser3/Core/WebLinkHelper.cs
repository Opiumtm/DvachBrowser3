using System;
using System.Linq;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;

namespace DvachBrowser3
{
    /// <summary>
    /// Помощник в получении веб-ссылок.
    /// </summary>
    public static class WebLinkHelper
    {
        /// <summary>
        /// Получить веб-ссылку.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Веб-ссылка.</returns>
        public static Uri GetWebLink(this BoardLinkBase link)
        {
            if (link == null)
            {
                return null;
            }
            var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            if (!engines.ListEngines().Any(l => StringComparer.OrdinalIgnoreCase.Equals(l, link.Engine)))
            {
                return null;
            }
            var engine = engines.GetEngineById(link.Engine);
            return engine.EngineUriService.GetBrowserLink(link);
        }
    }
}