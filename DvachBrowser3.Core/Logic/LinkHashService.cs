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
                { typeof(BoardMediaLink), TypeFunc<BoardMediaLink>(a => string.Format("boardmedia-{0}-{1}-{2}", a.Engine, a.Board, GetHashId(a.RelativeUri))) },
                { typeof(MediaLink), TypeFunc<MediaLink>(a => string.Format("media-{0}-{1}-{2}", a.Engine, a.IsAbsolute ? 1 : 0, GetHashId(a.RelativeUri))) },
                { typeof(YoutubeLink), TypeFunc<YoutubeLink>(a => string.Format("youtube-{0}-{1}", a.Engine, a.YoutubeId)) },
                { typeof(RootLink), TypeFunc<RootLink>(a => string.Format("root-{0}", a.Engine)) },
                { typeof(ThreadTagLink), TypeFunc<ThreadTagLink>(a => string.Format("tag-{0}-{1}-{2}", a.Engine, a.Board, GetHashId(a.Tag))) },
            };
            comparer = new LinkComparer(this);
        }

        private readonly Dictionary<string, string> hashIdCache = new Dictionary<string, string>();

        private string GetHashId(string id)
        {
            lock (hashIdCache)
            {
                if (hashIdCache.Count > 512)
                {
                    hashIdCache.Clear();
                }
                var id1 = (id ?? "").ToLowerInvariant();
                if (!hashIdCache.ContainsKey(id1))
                {
                    hashIdCache[id1] = UniqueIdHelper.CreateIdString(id1);
                }
                return hashIdCache[id1];
            }
        }

        private readonly IEqualityComparer<BoardLinkBase> comparer;

        private static Func<BoardLinkBase, string> TypeFunc<T>(Func<T, string> func) where T : BoardLinkBase
        {
            return a => func((T) a);
        }

        public string GetLinkHash(BoardLinkBase link)
        {
            if (link == null)
            {
                return "null";
            }
            var t = link.GetType();
            if (typeFuncs.ContainsKey(t))
            {
                return typeFuncs[t](link).ToLowerInvariant();
            }
            throw new ArgumentException(string.Format("Неизвестный тип ссылки {0}", t.FullName));
        }

        /// <summary>
        /// Получить средство сравнения.
        /// </summary>
        /// <returns>Средство сравнения.</returns>
        public IEqualityComparer<BoardLinkBase> GetComparer()
        {
            return comparer;
        }

        /// <summary>
        /// Средство сравнения.
        /// </summary>
        private sealed class LinkComparer : IEqualityComparer<BoardLinkBase>
        {
            /// <summary>
            /// Родительский объект.
            /// </summary>
            private readonly LinkHashService parent;

            /// <summary>
            /// Конструктор.
            /// </summary>
            /// <param name="parent">Родительский объект.</param>
            public LinkComparer(LinkHashService parent)
            {
                this.parent = parent;
            }

            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
            public bool Equals(BoardLinkBase x, BoardLinkBase y)
            {
                return StringComparer.OrdinalIgnoreCase.Equals(parent.GetLinkHash(x) ?? "", parent.GetLinkHash(y) ?? "");
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <returns>
            /// A hash code for the specified object.
            /// </returns>
            /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
            public int GetHashCode(BoardLinkBase obj)
            {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(parent.GetLinkHash(obj) ?? "");
            }
        }
    }
}