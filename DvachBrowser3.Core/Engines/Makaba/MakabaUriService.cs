using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Foundation.Metadata;
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
            if (link.Page == 0)
            {
                return new Uri(BaseUri, string.Format("{0}", link.Board));                
            }
            return new Uri(BaseUri, string.Format("{0}/{1}.html", link.Board, link.Page.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Получить URI треда.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="isReferer">Реферер.</param>
        /// <returns>URI.</returns>
        public Uri GetThreadUri(ThreadLink link, bool isReferer)
        {
            if (!isReferer)
            {
                return new Uri(BaseUri, string.Format("{0}/res/{1}.json", link.Board, link.Thread));
            }
            return new Uri(BaseUri, string.Format("{0}/res/{1}.html", link.Board, link.Thread));
        }

        /// <summary>
        /// Получить URI части треда.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="isReferer">Реферер.</param>
        /// <returns>URI.</returns>
        public Uri GetThreadPartUri(ThreadPartLink link, bool isReferer)
        {
            if (!isReferer)
            {
                return new Uri(BaseUri, string.Format("makaba/mobile.fcgi?task=get_thread&board={0}&thread={1}&num={2}", link.Board, link.Thread, link.FromPost));
            }
            return GetThreadUri(link, true);
        }

        /// <summary>
        /// Ссылка JSON.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public Uri GetJsonLink(BoardLinkBase link)
        {
            if (link is BoardPageLink)
            {
                return GetBoardPageUri((BoardPageLink)link, false);
            }
            if (link is BoardLink)
            {
                var l = new BoardPageLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = ((BoardLink)link).Board,
                    Page = 0,
                };
                return GetBoardPageUri(l, false);
            }
            if (link is ThreadPartLink)
            {
                return GetThreadPartUri(link as ThreadPartLink, false);
            }
            if (link is ThreadLink)
            {
                return GetThreadUri(link as ThreadLink, false);
            }
            return null;
        }

        /// <summary>
        /// Ссылка для браузера.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public Uri GetBrowserLink(BoardLinkBase link)
        {
            if (link is RootLink)
            {
                return BaseUri;
            }
            if (link is BoardPageLink)
            {
                return GetBoardPageUri((BoardPageLink) link, true);
            }
            if (link is BoardLink)
            {
                var l = new BoardPageLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = ((BoardLink) link).Board,
                    Page = 0,
                };
                return GetBoardPageUri(l, true);
            }
            if (link is ThreadPartLink)
            {
                return GetThreadPartUri(link as ThreadPartLink, true);
            }
            if (link is ThreadLink)
            {
                return GetThreadUri(link as ThreadLink, true);
            }
            return null;
        }

        private const string PostLinkRegexText = @"http[s]?://(?:[^/]+)/(?<board>[^/]+)/res/(?<parent>\d+).html(?:#(?<post>\d+))?$";
        private const string PostLinkRegex2Text = @"/?(?<board>[^/]+)/res/(?<parent>\d+).html(?:#(?<post>\d+))?$";

        /// <summary>
        /// Регурялное выражение для определения ссылок.
        /// </summary>
        public Regex PostLinkRegex
        {
            get { return Services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(PostLinkRegexText); }
        }

        /// <summary>
        /// Второе регурялное выражение для определения ссылок.
        /// </summary>
        public Regex PostLinkRegex2
        {
            get { return Services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(PostLinkRegex2Text); }
        }

        /// <summary>
        /// Попробовать распарсить ссылку.
        /// </summary>
        /// <param name="uri">URI.</param>
        /// <returns>Ссылка.</returns>
        public BoardLinkBase TryParsePostLink(string uri)
        {
            try
            {
                var regexes = new Regex[] { PostLinkRegex, PostLinkRegex2 };
                var match = regexes.Select(r => r.Match(uri)).FirstOrDefault(r => r.Success);
                if (match != null)
                {
                    if (match.Groups["post"].Captures.Count > 0)
                    {
                        return new PostLink()
                        {
                            Engine = CoreConstants.Engine.Makaba,
                            Board = match.Groups["board"].Captures[0].Value,
                            Thread = int.Parse(match.Groups["parent"].Captures[0].Value),
                            Post = int.Parse(match.Groups["post"].Captures[0].Value)
                        };
                    }
                    return new ThreadLink()
                    {
                        Engine = CoreConstants.Engine.Makaba,
                        Board = match.Groups["board"].Captures[0].Value,
                        Thread = int.Parse(match.Groups["parent"].Captures[0].Value),
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Uri BaseUri
        {
            get { return Services.GetServiceOrThrow<IMakabaEngineConfig>().BaseUri; }
        }
    }
}