using System;
using System.Globalization;
using DvachBrowser3.Links;

namespace DvachBrowser3.Engines.Makaba
{
    /// <summary>
    /// Сервис URI для макабы.
    /// </summary>
    public sealed class MakabaUriService : ServiceBase, IMakabaUriService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public MakabaUriService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Получить URI страницы борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="isReferer">Реферер.</param>
        /// <returns>URI.</returns>
        public Uri GetBoardPageUri(BoardPageLink link, bool isReferer)
        {
            if (!isReferer)
            {
                return new Uri(BaseUri, string.Format("{0}/{1}.json", link.Board, link.Page == 0 ? "index" : link.Page.ToString(CultureInfo.InvariantCulture)));
            }
            return new Uri(BaseUri, string.Format("{0}/{1}.html", link.Board, link.Page == 0 ? "index" : link.Page.ToString(CultureInfo.InvariantCulture)));
        }

        private Uri BaseUri
        {
            get { return Services.GetServiceOrThrow<IMakabaEngineConfig>().BaseUri; }
        }
    }
}