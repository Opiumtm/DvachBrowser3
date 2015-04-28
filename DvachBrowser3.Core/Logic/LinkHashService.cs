using System;
using System.Collections.Generic;
using DvachBrowser3.Links;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сервис хэша ссылок.
    /// </summary>
    public class LinkHashService : ServiceBase, ILinkHashService
    {
        private readonly Dictionary<Type, Func<BoardLinkBase, string>> typeFuncs;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public LinkHashService(IServiceProvider services) : base(services)
        {
            typeFuncs = new Dictionary<Type, Func<BoardLinkBase, string>>()
            {
                { typeof(BoardLink), TypeFunc<BoardLink>(a => string.Format("board-{0}-{1}", a.Engine, a.Board)) },
                { typeof(BoardPageLink), TypeFunc<BoardPageLink>(a => string.Format("boardpage-{0}-{1}-{2}", a.Engine, a.Board, a.Page)) },
                { typeof(ThreadLink), TypeFunc<ThreadLink>(a => string.Format("thread-{0}-{1}-{2}", a.Engine, a.Board, a.Thread)) },
                { typeof(ThreadPartLink), TypeFunc<ThreadPartLink>(a => string.Format("threadpart-{0}-{1}-{2}-{3}", a.Engine, a.Board, a.Thread, a.FromPost)) },
                { typeof(PostLink), TypeFunc<PostLink>(a => string.Format("post-{0}-{1}-{2}-{3}", a.Engine, a.Board, a.Thread, a.Post)) },
                { typeof(BoardMediaLink), TypeFunc<BoardMediaLink>(a => string.Format("boardmedia-{0}-{1}-{2}", a.Engine, a.Board, UniqueIdHelper.CreateId(a.RelativeUri.ToLower()))) },
                { typeof(MediaLink), TypeFunc<MediaLink>(a => string.Format("media-{0}-{1}-{2}", a.Engine, a.IsAbsolute ? 1 : 0, UniqueIdHelper.CreateId(a.RelativeUri.ToLower()))) },
                { typeof(YoutubeLink), TypeFunc<YoutubeLink>(a => string.Format("youtube-{0}-{1}", a.Engine, a.YoutubeId)) },
                { typeof(RootLink), TypeFunc<RootLink>(a => string.Format("root-{0}", a.Engine)) },
            };
        }

        private static Func<BoardLinkBase, string> TypeFunc<T>(Func<T, string> func) where T : BoardLinkBase
        {
            return a => func((T) a);
        }

        public string GetLinkHash(BoardLinkBase link)
        {
            var t = link.GetType();
            if (typeFuncs.ContainsKey(t))
            {
                return typeFuncs[t](link).ToLower();
            }
            throw new ArgumentException(string.Format("Неизвестный тип ссылки {0}", t.FullName));
        }
    }
}