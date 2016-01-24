using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Windows.Foundation.Metadata;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;

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
        /// Получить URI поста.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>URI.</returns>
        private Uri GetPostUri(PostLink link)
        {
            if (link.Post == link.Thread)
            {
                return new Uri(BaseUri, string.Format("{0}/res/{1}.html", link.Board, link.Thread));
            }
            return new Uri(BaseUri, string.Format("{0}/res/{1}.html#{2}", link.Board, link.Thread, link.Post));
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
            if (link is MediaLink)
            {
                return GetMediaLink(link as MediaLink);
            }
            if (link is BoardMediaLink)
            {
                return GetMediaLink(link as BoardMediaLink);
            }
            if (link is YoutubeLink)
            {
                return Services.GetServiceOrThrow<IYoutubeUriService>().GetViewUri((link as YoutubeLink).YoutubeId);
            }
            if (link is BoardCatalogLink)
            {
                return GetCatalogUri(link as BoardCatalogLink, false);
            }
            return null;
        }

        /// <summary>
        /// Ссылка HTML.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public Uri GetHtmlLink(BoardLinkBase link)
        {
            if (link is BoardPageLink)
            {
                return GetBoardPageUri((BoardPageLink)link, true);
            }
            if (link is BoardLink)
            {
                var l = new BoardPageLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = ((BoardLink)link).Board,
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
            if (link is MediaLink)
            {
                return GetMediaLink(link as MediaLink);
            }
            if (link is BoardMediaLink)
            {
                return GetMediaLink(link as BoardMediaLink);
            }
            if (link is YoutubeLink)
            {
                return Services.GetServiceOrThrow<IYoutubeUriService>().GetViewUri((link as YoutubeLink).YoutubeId);
            }
            if (link is BoardCatalogLink)
            {
                return GetCatalogUri(link as BoardCatalogLink, true);
            }
            return null;
        }

        /// <summary>
        /// Ссылка на медиа.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Ссылка.</returns>
        public Uri GetMediaLink(BoardMediaLink link)
        {
            return new Uri(BaseUri, string.Format("{0}/{1}", link.Board, link.RelativeUri));
        }

        /// <summary>
        /// Ссылка на медиа.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Ссылка.</returns>
        public Uri GetMediaLink(MediaLink link)
        {
            if (link.IsAbsolute)
            {
                return new Uri(link.RelativeUri, UriKind.Absolute);
            }
            else
            {
                return new Uri(BaseUri, link.RelativeUri);
            }
        }

        /// <summary>
        /// Ссылка на медиа.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Ссылка.</returns>
        public Uri GetMediaLink(YoutubeLink link)
        {
            return Services.GetServiceOrThrow<IYoutubeUriService>().GetThumbnailUri(link.YoutubeId);
        }

        /// <summary>
        /// Получить URI капчи.
        /// </summary>
        /// <param name="captchaType">Тип капчи.</param>
        /// <param name="forThread">Для треда.</param>
        /// <returns>URI капчи.</returns>
        public Uri GetCaptchaUri(CaptchaType captchaType, bool forThread)
        {
            if (captchaType == CaptchaType.Yandex)
            {
                return new Uri(BaseUri, CaptchaUri);
            }
            if (captchaType == CaptchaType.Recaptcha)
            {
                return new Uri(BaseUri, CaptchaUri + "?type=recaptcha");
            }
            if (captchaType == CaptchaType.GoogleRecaptcha2СhV1)
            {
                return new Uri(BaseUri, CaptchaUri + "?type=recaptchav1&mobile=1");
            }
            if (captchaType == CaptchaType.GoogleRecaptcha2СhV2)
            {
                return new Uri(BaseUri, CaptchaUri + "?type=recaptcha&mobile=1");
            }
            if (captchaType == CaptchaType.DvachCaptcha)
            {
                if (forThread)
                {
                    return new Uri(BaseUri, CaptchaUri + "?type=2chaptcha&action=thread");
                }
                return new Uri(BaseUri, CaptchaUri + "?type=2chaptcha");
            }
            return null;
        }

        private const string CaptchaUri = "makaba/captcha.fcgi";
        
        /// <summary>
        /// Получить URI для постинга.
        /// </summary>
        /// <returns>URI для постинга.</returns>
        public Uri GetPostingUri()
        {
            return new Uri(BaseUri, "/makaba/posting.fcgi?json=1");
        }

        /// <summary>
        /// Получить информацию о треде.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Информация.</returns>
        public Uri GetLastThreadInfoUri(ThreadLink link)
        {
            return new Uri(BaseUri, string.Format("makaba/mobile.fcgi?task=get_thread_last_info&board={0}&thread={1}", link.Board, link.Thread));
        }

        /// <summary>
        /// Получить URI списка борд.
        /// </summary>
        /// <returns>Ссылка.</returns>
        public Uri GetBoardsListUri()
        {
            return new Uri(BaseUri, "makaba/mobile.fcgi?task=get_boards");
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
            if (link is PostLink)
            {
                return GetPostUri(link as PostLink);
            }
            if (link is MediaLink)
            {
                return GetMediaLink(link as MediaLink);
            }
            if (link is BoardMediaLink)
            {
                return GetMediaLink(link as BoardMediaLink);
            }
            if (link is BoardCatalogLink)
            {
                return GetCatalogUri(link as BoardCatalogLink, true);
            }
            if (link is YoutubeLink)
            {
                return Services.GetServiceOrThrow<IYoutubeUriService>().GetViewUri((link as YoutubeLink).YoutubeId);
            }
            return null;
        }

        private const string PostLinkRegexText = @"http[s]?://(?:[^/]+)/(?<board>[^/]+)/res/(?<parent>\d+).html(?:#(?<post>\d+))?$";
        private const string PostLinkRegex2Text = @"/?(?<board>[^/]+)/res/(?<parent>\d+).html(?:#(?<post>\d+))?$";

        /// <summary>
        /// Регурялное выражение для определения ссылок.
        /// </summary>
        public Regex PostLinkRegex => Services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(PostLinkRegexText);

        /// <summary>
        /// Второе регурялное выражение для определения ссылок.
        /// </summary>
        public Regex PostLinkRegex2 => Services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(PostLinkRegex2Text);

        /// <summary>
        /// Получить URI для постинга без капчи.
        /// </summary>
        /// <param name="check">Проверить.</param>
        /// <param name="appId">ID приложения.</param>
        /// <returns>Ссылка.</returns>
        public Uri GetNocaptchaUri(bool check, string appId)
        {
            if (check)
            {
                return new Uri(BaseUri, $"/makaba/captcha.fcgi?appid={WebUtility.UrlEncode(appId)}&check=1");
            }
            return new Uri(BaseUri, $"/makaba/captcha.fcgi?appid={WebUtility.UrlEncode(appId)}");
        }

        /// <summary>
        /// Получить URI каталога.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="html">Для браузера.</param>
        /// <returns>URI каталога.</returns>
        public Uri GetCatalogUri(BoardCatalogLink link, bool html)
        {
            string board = link?.Board;
            if (board == null)
            {
                return null;
            }
            var ext = html ? "html" : "json";
            switch (link.Sort)
            {
                case BoardCatalogSort.CreateDate:
                    return new Uri(BaseUri, $"{board}/catalog_num.{ext}");
                default:
                    return new Uri(BaseUri, $"{board}/catalog.{ext}");
            }
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
            get
            {
                var engines = Services.GetServiceOrThrow<INetworkEngines>();
                var makaba = engines.GetEngineById(CoreConstants.Engine.Makaba);
                var config = (IMakabaEngineConfig)makaba.Configuration;
                return config.BaseUri;
            }
        }
    }
}