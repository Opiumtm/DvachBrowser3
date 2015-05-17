using System;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Сервис URI Youtube.
    /// </summary>
    public sealed class YoutubeUriService : ServiceBase, IYoutubeUriService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public YoutubeUriService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Получить URI для просмотра.
        /// </summary>
        /// <param name="youtubeId">Идентификатор.</param>
        /// <returns>URI.</returns>
        public Uri GetViewUri(string youtubeId)
        {
            return new Uri(string.Format("http://www.youtube.com/watch?v={0}", youtubeId), UriKind.Absolute);
        }

        /// <summary>
        /// Получить URI картинки предварительного просмотра.
        /// </summary>
        /// <param name="youtubeId">Идентификатор.</param>
        /// <returns>URI</returns>
        public Uri GetThumbnailUri(string youtubeId)
        {
            return new Uri(string.Format("http://i.ytimg.com/vi/{0}/0.jpg", youtubeId), UriKind.Absolute);
        }

        /// <summary>
        /// Получить URI для запуска.
        /// </summary>
        /// <param name="youtubeId">Идентификатор.</param>
        /// <returns>URI</returns>
        public Uri GetLaunchApplicationUri(string youtubeId)
        {
            return new Uri(string.Format("vnd.youtube:{0}?vndapp=youtube_mobile&vndclient=mv-google&vndel=watch", youtubeId));
        }

        /// <summary>
        /// Высота картинки.
        /// </summary>
        public int ThumbnailHeight
        {
            get { return 360; }
        }

        /// <summary>
        /// Ширина картинки.
        /// </summary>
        public int ThumbnailWidth
        {
            get { return 480; }
        }
    }
}