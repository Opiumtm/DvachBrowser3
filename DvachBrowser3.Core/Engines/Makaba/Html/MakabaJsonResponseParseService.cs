using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;
using DvachBrowser3.Makaba;
using DvachBrowser3.Posts;
using HtmlAgilityPack;

namespace DvachBrowser3.Engines.Makaba.Html
{
    /// <summary>
    /// Сервис парсинга ответов JSON.
    /// </summary>
    public sealed class MakabaJsonResponseParseService : ServiceBase, IMakabaJsonResponseParseService
    {
        private const string IpIdRegexText = @"Аноним ID: <span class=""postertripid"">(?<id>.*)</span>.*$";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public MakabaJsonResponseParseService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Разобрать HTML в посте.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="link">Ссылка.</param>
        /// <param name="isPreview">Предварительный просмотр треда.</param>
        /// <returns>Результат разбора.</returns>
        private PostTree Parse(BoardPost2 data, ThreadLink link, bool isPreview)
        {
            var ipIdRegex = Services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(IpIdRegexText);
            PostFlags flags = 0;
            if (data.Banned != "0" && !string.IsNullOrWhiteSpace(data.Banned))
            {
                flags |= PostFlags.Banned;
            }
            if (data.Closed != "0" && !string.IsNullOrWhiteSpace(data.Closed))
            {
                flags |= PostFlags.Closed;
            }
            if (data.Sticky != "0" && !string.IsNullOrWhiteSpace(data.Sticky))
            {
                flags |= PostFlags.Sticky;
            }
            if (isPreview)
            {
                flags |= PostFlags.ThreadPreview;
            }
            if (data.Op != "0" && !string.IsNullOrWhiteSpace(data.Op))
            {
                flags |= PostFlags.Op;
            }
            if ("mailto:sage".Equals((data.Email ?? "").Trim(), StringComparison.OrdinalIgnoreCase))
            {
                flags |= PostFlags.Sage;
            }
            if (data.Edited != "0" && !string.IsNullOrWhiteSpace(data.Edited))
            {
                flags |= PostFlags.IsEdited;
            }
            var number = data.Number.TryParseWithDefault();
            var postNodes = Services.GetServiceOrThrow<IMakabaHtmlPostParseService>().GetPostNodes(data.Comment ?? "");
            var name = WebUtility.HtmlDecode(data.Name ?? string.Empty);
            var result = new PostTree()
            {
                Link = link,
                ParentLink = new ThreadLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = link.Board,
                    Thread = link.Thread
                },
                Subject = WebUtility.HtmlDecode(data.Subject ?? string.Empty),
                BoardSpecificDate = data.Date,
                Date = Services.GetServiceOrThrow<IDateService>().FromUnixTime(data.Timestamp.TryParseWithDefault()),
                Flags = flags,
                Quotes = new List<BoardLinkBase>(),
                Comment = new PostNodes()
                {
                    Nodes = postNodes.ToList()
                },
                Hash = data.Md5,
                Extensions = new List<PostTreeExtension>(),
                Email = data.Email,
                Media = new List<PostMediaBase>()
            };
            if (string.IsNullOrWhiteSpace(result.Subject) && number == link.Thread)
            {
                try
                {
                    var lines = ToPlainText(result);
                    if (lines.Count > 0)
                    {
                        var s = lines.Where(l => !string.IsNullOrWhiteSpace(l)).FirstOrDefault();
                        if (s != null)
                        {
                            if (s.Length >= 50)
                            {
                                s = s.Substring(0, 50 - 3) + "...";
                            }
                            result.Subject = s;
                        }
                    }
                }
                catch
                {
                }
            }
            var match = ipIdRegex.Match(name);
            if (match.Success)
            {
                name = match.Groups["id"].Captures[0].Value;
            }
            else if (name == "Аноним ID: Heaven")
            {
                name = "Heaven";
            }
            if (name == "Аноним ID:&nbspHeaven")
            {
                name = "Heaven";
            }
            if (name.StartsWith("Аноним ID:", StringComparison.OrdinalIgnoreCase))
            {
                name = name.Remove(0, "Аноним ID:".Length).Trim();
            }
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrWhiteSpace(data.Tripcode))
            {
                result.Extensions.Add(new PostTreePosterExtension()
                {
                    Name = name,
                    Tripcode = data.Tripcode
                });
            }
            var iconExtension = ParseIcon(data.Icon);
            if (iconExtension != null)
            {
                result.Extensions.Add(iconExtension);
            }
            if (data.Files != null)
            {
                foreach (var f in data.Files)
                {
                    var mediaLink = new BoardMediaLink()
                        {
                            Engine = CoreConstants.Engine.Makaba,
                            Board = link.Board,
                            RelativeUri = f.Path
                        };
                    var tnLink = new BoardMediaLink()
                    {
                        Engine = CoreConstants.Engine.Makaba,
                        Board = link.Board,
                        RelativeUri = f.Thumbnail
                    };
                    var media = new PostImageWithThumbnail()
                    {
                        Link = mediaLink,
                        ParentLink = result.Link,
                        Size = f.Size,
                        Height = f.Heigth,
                        Width = f.Width,
                        Name = f.Name,
                        Thumbnail = new PostImage()
                        {
                            Link = tnLink,
                            ParentLink = mediaLink,
                            Height = f.TnHeight,
                            Width = f.TnWidth,
                            Size = null,
                            Name = f.Name
                        }
                    };
                    result.Media.Add(media);
                }
            }
            return result;
        }

        /// <summary>
        /// Парсить данные страницы борды.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public BoardPageTree ParseBoardPage(BoardEntity2 data, BoardPageLink link)
        {
            var entity = GetEntityTree(data);
            var result = new BoardPageTree()
            {
                Extensions = new List<BoardPageTreeExtension>(),
                LastModified = null,
                Link = link,
                ParentLink = new RootLink()
                {
                    Engine = CoreConstants.Engine.Makaba
                },
                PageNumber = link.Page,
                Threads = new List<ThreadPreviewTree>(),
                MaxPage = entity.Pages != null ? entity.Pages.OrderByDescending(p => p).Select(p => (int?)p).FirstOrDefault() : null,
            };
            result.Extensions.Add(new MakabaBoardPageExtension()
            {
                Entity = entity
            });
            if (data.Threads != null)
            {
                foreach (var t in data.Threads)
                {
                    var posts = t.Posts.OrderBy(p => p.Number.TryParseWithDefault()).ToArray();
                    var tnum = posts.First().Number.TryParseWithDefault();
                    var tlink = new ThreadLink()
                    {
                        Engine = CoreConstants.Engine.Makaba,
                        Board = link.Board,
                        Thread = tnum,
                    };
                    var thread = new ThreadPreviewTree()
                    {
                        Extensions = new List<PostTreeCollectionExtension>(),
                        Link = tlink,
                        ParentLink = link,
                        ImageCount =
                            t.ImagesCount.TryParseWithDefault() +
                            posts.Sum(p => (p.Files ?? new BoardPostFile2[0]).Length),
                        Omit = t.PostsCount.TryParseWithDefault(),
                        OmitImages = t.ImagesCount.TryParseWithDefault(),
                        ReplyCount = Math.Max(t.PostsCount.TryParseWithDefault() + posts.Count() - 1, 0),
                        Posts = posts.Select(p => Parse(p, tlink, true)).ToList(),
                    };
                    for (var i = 0; i < thread.Posts.Count; i++)
                    {
                        if (i == 0)
                        {
                            thread.Posts[i].Counter = 1;
                        }
                        else
                        {
                            var ni = thread.Posts.Count - i;
                            thread.Posts[i].Counter = thread.ReplyCount - ni;
                        }
                    }
                    result.Threads.Add(thread);
                }
            }
            return result;
        }

        private MakabaEntityTree GetEntityTree(BoardEntity2 data)
        {
            var entity = new MakabaEntityTree()
            {
                Board = data.Board,
                BoardBannerImage = data.BoardBannerImage,
                BoardBannerLink = data.BoardBannerLink,
                BoardName = data.BoardName,
                BoardSpeed = data.BoardSpeed,
                CurrentPage = data.CurrentPage.TryParseWithDefault(),
                CurrentThread = data.CurrentThread.TryParseWithDefault(),
                Icons = data.Icons != null
                    ? data.Icons.Select(i => new MakabaIconReference()
                    {
                        Name = i.Name,
                        Number = i.NumberInt,
                        Url = i.Url
                    }).OrderBy(i => i.Number).ToList()
                    : null,
                Pages =
                    data.Pages != null
                        ? data.Pages.Select(s => s.TryParseWithDefault(-1))
                            .Where(i => i >= 0)
                            .Distinct()
                            .OrderBy(i => i)
                            .ToList()
                        : null,
                IsBoard = data.IsBoard.TryParseWithDefault() != 0
            };
            return entity;
        }

        /// <summary>
        /// Парсить данные треда.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public ThreadTree ParseThread(BoardEntity2 data, ThreadLink link)
        {
            var entity = GetEntityTree(data);
            var result = new ThreadTree()
            {
                Link = link,
                ParentLink = new BoardLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = link.Board,
                },
                Extensions = new List<PostTreeCollectionExtension>(),
                Posts = data.Threads.SelectMany(p => p.Posts).OrderBy(p => p.Number.TryParseWithDefault()).Select(p => Parse(p, link, false)).ToList(),                
            };
            result.Extensions.Add(new MakabaCollectionExtension() { Entity = entity });
            for (int i = 0; i < result.Posts.Count; i++)
            {
                result.Posts[i].Counter = i + 1;
            }
            return result;
        }

        /// <summary>
        /// Парсить частичные данные треда.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public ThreadTreePartial ParseThreadPartial(BoardPost2[] data, ThreadPartLink link)
        {
            var threadLink = new ThreadLink()
            {
                Engine = CoreConstants.Engine.Makaba,
                Board = link.Board,
                Thread = link.Thread
            };
            var result = new ThreadTreePartial()
            {
                Link = threadLink,
                Posts = data.OrderBy(p => p.Number.TryParseWithDefault()).Select(p => Parse(p, threadLink, false)).ToList(),
                Extensions = new List<PostTreeCollectionExtension>(),
                ParentLink = new BoardLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = link.Board,
                },
            };
            return result;
        }

        /// <summary>
        /// Перевести в простой текст.
        /// </summary>
        /// <param name="tree">Дерево поста.</param>
        /// <returns>Текст.</returns>
        public static IList<string> ToPlainText(PostTree tree)
        {
            var context = new { sb = new StringBuilder(), result = new List<string>() };
            var rules = tree.Comment.Nodes.TreeWalk(context)
                .GetChildren(n => (n is PostNode) ? ((PostNode)n).Children : null)
                .If(n => n is PostTextNode, (n, ctx) =>
                {
                    ctx.sb.Append(((PostTextNode)n).Text);
                    return ctx;
                })
                .If(n => IsAttribute(n, PostNodeBasicAttribute.Paragraph) || n is PostNodeBreak, (n, ctx) =>
                {
                    ctx.result.Add(ctx.sb.ToString());
                    ctx.sb.Clear();
                    return ctx;
                })
                .If(n => n is PostNodeBoardLink, (n, ctx) =>
                {
                    var l = (PostNodeBoardLink)n;
                    var pl = l.BoardLink as PostLink;
                    var tl = l.BoardLink as ThreadLink;
                    if (pl != null)
                    {
                        ctx.sb.Append(">>" + pl.Post);
                    } else if (tl != null)
                    {
                        ctx.sb.Append(">>" + tl.Thread);
                    }
                    return ctx;
                })
                .Else((n, c) => c);
            rules.Run();
            if (context.sb.Length > 0)
            {
                context.result.Add(context.sb.ToString());
            }
            return context.result;
        }

        private static bool IsAttribute(PostNodeBase node, PostNodeBasicAttribute attribute)
        {
            var n = node as PostNode;
            if (n == null)
            {
                return false;
            }
            var a = n.Attribute as PostNodeAttribute;
            if (a == null)
            {
                return false;
            }
            return a.Attribute == attribute;
        }

        /// <summary>
        /// Парсить иконку.
        /// </summary>
        /// <param name="str">Строка.</param>
        /// <returns>Иконка.</returns>
        public PostTreeIconExtension ParseIcon(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            try
            {
                var html = new HtmlDocument();
                html.LoadHtml(str);
                var result = html
                    .WalkTemplate(obj => obj.DocumentNode)
                    .WalkTemplate(obj => obj.FirstNonTextChild())
                    .WalkTemplate(obj => obj.Name.EqualsNc("img"), obj => obj)
                    .WalkTemplate(
                        obj =>
                            obj.GetAttributeValue("src", null) != null && obj.GetAttributeValue("title", null) != null,
                        obj =>
                            new PostTreeIconExtension()
                            {
                                Uri = obj.GetAttributeValue("src", null),
                                Description = obj.GetAttributeValue("title", null)
                            });
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}