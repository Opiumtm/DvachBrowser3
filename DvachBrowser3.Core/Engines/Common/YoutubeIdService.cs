using System;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Сервис получения ID ютубы.
    /// </summary>
    public sealed class YoutubeIdService : ServiceBase, IYoutubeIdService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public YoutubeIdService(IServiceProvider services) : base(services)
        {
        }

        private const string YoutubeRegex = @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)";

        /// <summary>
        /// Получить идентификатор ютубы.
        /// </summary>
        /// <param name="uri">URI.</param>
        /// <returns>Идентификатор.</returns>
        public string GetYoutubeIdFromUri(string uri)
        {
            try
            {
                if (uri == null) return null;
                var youtubeRegex = Services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(YoutubeRegex);
                var match = youtubeRegex.Match(uri);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}