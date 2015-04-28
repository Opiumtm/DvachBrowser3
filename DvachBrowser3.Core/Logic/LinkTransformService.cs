using System;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сервис изменения ссылок.
    /// </summary>
    public sealed class LinkTransformService : ServiceBase, ILinkTransformService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Cервисы.</param>
        public LinkTransformService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Получить ссылку на тред из ссылки на часть треда.
        /// </summary>
        /// <param name="link">Ссылка на часть треда.</param>
        /// <returns>Ссылка на тред.</returns>
        public BoardLinkBase ThreadLinkFromThreadPartLink(BoardLinkBase link)
        {
            if (link is ThreadPartLink)
            {
                var l = link as ThreadLink;
                return new ThreadLink()
                {
                    Engine = l.Engine,
                    Board = l.Board,
                    Thread = l.Thread
                };
            }
            if (link is ThreadLink)
            {
                var l = link as ThreadLink;
                return new ThreadLink()
                {
                    Engine = l.Engine,
                    Board = l.Board,
                    Thread = l.Thread
                };
            }
            return null;
        }

        /// <summary>
        /// Получить ссылку на часть треда из ссылки на тред.
        /// </summary>
        /// <param name="threadLink">Ссылка на тред.</param>
        /// <param name="lastPostLink">Ссылка на последний пост.</param>
        /// <returns>Ссылка на часть треда.</returns>
        public BoardLinkBase ThreadPartLinkFromThreadLink(BoardLinkBase threadLink, BoardLinkBase lastPostLink)
        {
            if (threadLink is ThreadLink && lastPostLink is PostLink)
            {
                var l = threadLink as ThreadLink;
                var pl = lastPostLink as PostLink;
                if (string.Equals(l.Engine, pl.Engine, StringComparison.OrdinalIgnoreCase) && string.Equals(l.Board, pl.Board, StringComparison.OrdinalIgnoreCase) && l.Thread == pl.Thread)
                {
                    return new ThreadPartLink()
                    {
                        Engine = l.Engine,
                        Board = l.Board,
                        Thread = l.Thread,
                        FromPost = pl.Post
                    };
                }
            }
            return null;
        }
    }
}