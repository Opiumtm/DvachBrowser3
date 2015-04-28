using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Получить средство сравнения ссылок.
        /// </summary>
        /// <returns>Средство сравнения ссылок.</returns>
        public IComparer<BoardLinkBase> GetLinkComparer()
        {
            return LinkComparer.Instance;
        }

        /// <summary>
        /// Средство сравнения ссылок.
        /// </summary>
        private sealed class LinkComparer : IComparer<BoardLinkBase>
        {
            public static readonly LinkComparer Instance = new LinkComparer();

            private readonly Dictionary<Type, Func<BoardLinkBase, CompareValues>> valueGetters;

            private LinkComparer()
            {
                valueGetters = new Dictionary<Type, Func<BoardLinkBase, CompareValues>>()
                {
                    {typeof(BoardLink), CreateGetter<BoardLink>(l => new CompareValues() { Engine = l.Engine ?? "", Board = l.Board ?? "", Page = 0, Thread = 0, Post = 0, Other = ""})},
                    {typeof(BoardPageLink), CreateGetter<BoardPageLink>(l => new CompareValues() { Engine = l.Engine ?? "", Board = l.Board ?? "", Page = l.Page, Thread = 0, Post = 0, Other = ""})},
                    {typeof(ThreadLink), CreateGetter<ThreadLink>(l => new CompareValues() { Engine = l.Engine ?? "", Board = l.Board ?? "", Page = 0, Thread = l.Thread, Post = 0, Other = ""})},
                    {typeof(ThreadPartLink), CreateGetter<ThreadPartLink>(l => new CompareValues() { Engine = l.Engine ?? "", Board = l.Board ?? "", Page = 0, Thread = l.Thread, Post = 0, Other = ""})},
                    {typeof(PostLink), CreateGetter<PostLink>(l => new CompareValues() { Engine = l.Engine ?? "", Board = l.Board ?? "", Page = 0, Thread = l.Thread, Post = l.Post, Other = ""})},
                    {typeof(BoardMediaLink), CreateGetter<BoardMediaLink>(l => new CompareValues() { Engine = l.Engine ?? "", Board = l.Board ?? "", Page = 0, Thread = 0, Post = 0, Other = l.RelativeUri ?? ""})},
                    {typeof(MediaLink), CreateGetter<MediaLink>(l => new CompareValues() { Engine = l.Engine ?? "", Board = "", Page = 0, Thread = 0, Post = 0, Other = l.RelativeUri ?? ""})},
                    {typeof(YoutubeLink), CreateGetter<YoutubeLink>(l => new CompareValues() { Engine = l.Engine ?? "", Board = "", Page = 0, Thread = 0, Post = 0, Other = l.YoutubeId ?? ""})},
                    {typeof(RootLink), CreateGetter<RootLink>(l => new CompareValues() { Engine = l.Engine ?? "", Board = "", Page = 0, Thread = 0, Post = 0, Other = ""})},
                };
            }

            private static Func<BoardLinkBase, CompareValues> CreateGetter<T>(Func<T, CompareValues> func)
                where T : BoardLinkBase
            {
                return a => func((T) a);
            }

            private CompareValues GetValue(BoardLinkBase link)
            {
                if (link != null)
                {
                    var t = link.GetType();
                    if (valueGetters.ContainsKey(t))
                    {
                        return valueGetters[t](link);
                    }
                }
                return new CompareValues()
                {
                    Board = "",
                    Engine = "",
                    Other = "",
                    Post = 0,
                    Thread = 0,
                    Page = 0
                };
            }

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <returns>
            /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
            public int Compare(BoardLinkBase x, BoardLinkBase y)
            {
                var x1 = GetValue(x);
                var y1 = GetValue(y);
                return x1.CompareTo(y1);
            }            

            private struct CompareValues : IComparable<CompareValues>
            {
                public string Engine;

                public string Board;

                public int Page;

                public int Thread;

                public int Post;

                public string Other;

                /// <summary>
                /// Compares the current object with another object of the same type.
                /// </summary>
                /// <returns>
                /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
                /// </returns>
                /// <param name="other">An object to compare with this object.</param>
                public int CompareTo(CompareValues other)
                {
                    var er = StringComparer.OrdinalIgnoreCase.Compare(Engine, other.Engine);
                    if (er != 0)
                    {
                        return er;
                    }
                    var br = StringComparer.OrdinalIgnoreCase.Compare(Board, other.Board);
                    if (br != 0)
                    {
                        return br;
                    }
                    var bpr = Comparer<int>.Default.Compare(Page, other.Page);
                    if (bpr != 0)
                    {
                        return bpr;
                    }
                    var tr = Comparer<int>.Default.Compare(Thread, other.Thread);
                    if (tr != 0)
                    {
                        return tr;
                    }
                    var pr = Comparer<int>.Default.Compare(Post, other.Post);
                    if (pr != 0)
                    {
                        return pr;
                    }
                    var or = StringComparer.Ordinal.Compare(Other, other.Other);
                    if (or != 0)
                    {
                        return or;
                    }
                    return 0;
                }
            }
        }
    }
}