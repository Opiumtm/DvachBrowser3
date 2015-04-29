using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сервис обработки данных треда.
    /// </summary>
    public sealed class ThreadTreeProcessService : ServiceBase, IThreadTreeProcessService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public ThreadTreeProcessService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Слить тред с его частью.
        /// </summary>
        /// <param name="src">Исходный тред.</param>
        /// <param name="part">Часть треда.</param>
        public void MergeTree(ThreadTree src, ThreadTreePartial part)
        {
            var linkHashService = Services.GetServiceOrThrow<ILinkHashService>();
            if (src.Posts == null)
            {
                src.Posts = new List<PostTree>();
            }
            var oldPosts = src.Posts.DeduplicateToDictionary(p => linkHashService.GetLinkHash(p.Link));
            foreach (var np in part.Posts)
            {
                oldPosts[linkHashService.GetLinkHash(np.Link)] = np;
            }
            src.Posts = oldPosts.Values.ToList();
        }

        /// <summary>
        /// Установить обратные ссылки.
        /// </summary>
        /// <param name="src">Исходный тред.</param>
        public void SetBackLinks(PostTreeCollection src)
        {
            if (src.Posts == null)
            {
                src.Posts = new List<PostTree>();
            }
            var hashService = Services.GetServiceOrThrow<ILinkHashService>();
            var transformService = Services.GetServiceOrThrow<ILinkTransformService>();
            var postLinks = src.Posts.DeduplicateToDictionary(p => p.Link, hashService.GetComparer());
            foreach (var post in src.Posts)
            {
                if (post.Quotes == null)
                {
                    post.Quotes = new List<BoardLinkBase>();
                }
            }
            foreach (var post in src.Posts)
            {
                if (post.Comment != null && post.Comment.Nodes != null)
                {
                    var links = post.Comment.Nodes.SelectMany(n => n.FlatHierarchy(nn =>
                    {
                        var n1 = nn as PostNode;
                        if (n1 != null)
                        {
                            return n1.Children ?? new List<PostNodeBase>();
                        }
                        return Enumerable.Empty<PostNodeBase>();
                    })).OfType<PostNodeBoardLink>()
                    .Where(l => l.BoardLink != null)
                    .Select(l => l.BoardLink)
                    .ToList();
                    foreach (var l in links)
                    {
                        if (postLinks.ContainsKey(l))
                        {
                            postLinks[l].Quotes.Add(post.Link);
                        }
                    }
                }
            }
            foreach (var post in src.Posts.Where(p => p.Quotes.Count > 1))
            {
                post.Quotes = post.Quotes.Distinct(hashService.GetComparer()).OrderBy(l => l, transformService.GetLinkComparer()).ToList();
            }
        }

        /// <summary>
        /// Сортировать дерево по возрастанию номеров постов.
        /// </summary>
        /// <param name="src">Исходный тред.</param>
        public void SortThreadTree(PostTreeCollection src)
        {
            if (src.Posts == null)
            {
                src.Posts = new List<PostTree>();
            }
            var comparer = Services.GetServiceOrThrow<ILinkTransformService>().GetLinkComparer();
            src.Posts.Sort((x, y) => comparer.Compare(x.Link, y.Link));
        }

        /// <summary>
        /// Получить короткую информацию о треде.
        /// </summary>
        /// <param name="src">Исходный тред.</param>
        /// <returns>Короткая информация.</returns>
        public ShortThreadInfo GetShortInfo(ThreadTree src)
        {
            if (src == null || src.Posts == null)
            {
                return null;
            }
            var firstPost = src.Posts.FirstOrDefault();
            if (firstPost == null)
            {
                return null;
            }
            PostImageWithThumbnail firstMedia = null;
            if (firstPost.Media != null)
            {
                firstMedia = firstPost.Media.OfType<PostImageWithThumbnail>().Where(l => l.Thumbnail != null && l.Thumbnail.Link != null).FirstOrDefault();
            }
            return new ShortThreadInfo()
            {
                Title = firstPost.Subject,
                SmallImage = firstMedia != null ? new ThreadPictureInfo()
                {
                    Link = firstMedia.Thumbnail.Link,
                    Height = firstMedia.Thumbnail.Height,
                    Width = firstMedia.Thumbnail.Width
                } : null
            };
        }
    }
}