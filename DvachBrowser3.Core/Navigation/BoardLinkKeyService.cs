using System;
using System.Collections.Generic;
using DvachBrowser3.Links;

namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Сервис получения ключей для ссылок.
    /// </summary>
    public sealed class BoardLinkKeyService : ServiceBase, IBoardLinkKeyService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public BoardLinkKeyService(IServiceProvider services) : base(services)
        {
            keyFuncs = new Dictionary<Type, Func<BoardLinkBase, INavigationKey>>()
            {
                {typeof(BoardLink), l => new BoardNavigationKey((BoardLink)l)},
                {typeof(BoardPageLink), l => new BoardPageNavigationKey((BoardPageLink)l)},
                {typeof(ThreadLink), l => new ThreadNavigationKey((ThreadLink)l)},
                {typeof(ThreadPartLink), l => new ThreadNavigationKey((ThreadPartLink)l)},
                {typeof(PostLink), l => new PostNavigationKey((PostLink)l)},
                {typeof(BoardMediaLink), l => new BoardMediaNavigationKey((BoardMediaLink)l)},
                {typeof(MediaLink), l => new MediaNavigationKey((MediaLink)l)},
                {typeof(YoutubeLink), l => new YoutubeNavigationKey((YoutubeLink)l)},
                {typeof(RootLink), l => new RootNavigationKey((RootLink)l)},
            };
        }

        private readonly Dictionary<Type, Func<BoardLinkBase, INavigationKey>> keyFuncs;

        /// <summary>
        /// Получить ключ.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Ключ.</returns>
        public INavigationKey GetKey(BoardLinkBase link)
        {
            if (link == null)
            {
                return null;
            }
            var t = link.GetType();
            if (keyFuncs.ContainsKey(t))
            {
                return keyFuncs[t](link);
            }
            throw new ArgumentException(string.Format("Неизвестный тип ссылки {0}", t.FullName));
        }
    }
}