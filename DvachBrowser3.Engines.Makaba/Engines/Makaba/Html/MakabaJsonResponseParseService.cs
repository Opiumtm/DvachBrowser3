using System;
using System.Collections.Generic;
using System.Globalization;
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
        private const string IpIdRegexText = @"(?:.*)\s+ID:\s+<span\s+class=""postertripid"">(?<id>.*)</span>.*$";

        private const string IpIdRegexText2 = @"(?:.*)\s+ID:\s+<span\s+id=""[^""]*""\s+style=""(?<style>[^""]*)"">(?<id>.*)</span>.*$";

        private const string ColorRegexText = @"color:rgb\((?<r>\d+),(?<g>\d+),(?<b>\d+)\)\;$";

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
            var ipIdRegex2 = Services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(IpIdRegexText2);
            var colorRegex = Services.GetServiceOrThrow<IRegexCacheService>().CreateRegex(ColorRegexText);
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
            string admName = null;
            if (data.Tripcode != null)
            {
                if (data.Tripcode.StartsWith("!!%") && data.Tripcode.EndsWith("%!!"))
                {
                    if ("!!%mod%!!".Equals(data.Tripcode))
                    {
                        admName = "## Mod ##";
                    }
                    else if ("!!%adm%!!".Equals(data.Tripcode))
                    {
                        admName = "## Abu ##";
                    }
                    else if ("!!%Inquisitor%!!".Equals(data.Tripcode))
                    {
                        admName = "## Applejack ##";
                    }
                    else if ("!!%coder%!!".Equals(data.Tripcode))
                    {
                        admName = "## Кодер ##";
                    }
                    else admName = data.Tripcode.Replace("!!%", "## ").Replace("%!!", " ##");
                    flags |= PostFlags.AdminTrip;
                }
            }
            var number = data.Number.TryParseWithDefault();
            var postNodes = Services.GetServiceOrThrow<IMakabaHtmlPostParseService>().GetPostNodes(data.Comment ?? "");
            var name = admName != null && string.IsNullOrWhiteSpace(data.Name) ? admName : WebUtility.HtmlDecode(data.Name ?? string.Empty).Replace("&nbsp;", " ");
            var result = new PostTree()
            {
                Link = new PostLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = link.Board,
                    Thread = link.Thread,
                    Post = number
                },
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
            string nameColor = null;
            PostColor color = null;
            var match = ipIdRegex.Match(name);
            var match2 = ipIdRegex2.Match(name);
            if (match.Success)
            {
                name = match.Groups["id"].Captures[0].Value;
            }
            else if (match2.Success)
            {
                name = match2.Groups["id"].Captures[0].Value;
                nameColor = match2.Groups["style"].Captures[0].Value;
                var cmatch = colorRegex.Match(nameColor);
                if (cmatch.Success)
                {
                    try
                    {
                        var r = byte.Parse(cmatch.Groups["r"].Captures[0].Value, CultureInfo.InvariantCulture.NumberFormat);
                        var g = byte.Parse(cmatch.Groups["g"].Captures[0].Value, CultureInfo.InvariantCulture.NumberFormat);
                        var b = byte.Parse(cmatch.Groups["b"].Captures[0].Value, CultureInfo.InvariantCulture.NumberFormat);
                        color = new PostColor() { R = r, G = g, B = b };
                    }
                    catch (Exception)
                    {
                        color = null;
                    }
                }
            }
            else if (name.StartsWith("Аноним ID:", StringComparison.OrdinalIgnoreCase))
            {
                name = name.Remove(0, "Аноним ID:".Length).Trim();
            }
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrWhiteSpace(data.Tripcode))
            {
                result.Extensions.Add(new PostTreePosterExtension()
                {
                    Name = Windows.Data.Html.HtmlUtilities.ConvertToText(name ?? ""),
                    Tripcode = data.Tripcode,
                    NameColorStr = nameColor,
                    NameColor = color
                });
            }
            var flagAndIcon = ParseFlagAndIcon(data.Icon);
            if (flagAndIcon.Icon != null)
            {
                result.Extensions.Add(flagAndIcon.Icon);
            }
            if (flagAndIcon.Country != null)
            {
                result.Extensions.Add(flagAndIcon.Country);
            }
            if (!string.IsNullOrWhiteSpace(data.Tags))
            {
                result.Extensions.Add(new PostTreeTagsExtension()
                {
                    TagString = data.Tags,
                    Tags = new List<string>() { data.Tags }
                });
            }
            if (data.Files != null)
            {
                foreach (var f in data.Files)
                {
                    var mediaLink = new BoardMediaLink()
                        {
                            Engine = CoreConstants.Engine.Makaba,
                            Board = link.Board,
                            RelativeUri = f.Path,
                            KnownMediaType = f.Type == MakabaMediaTypes.Webm ? KnownMediaType.Webm : KnownMediaType.Default
                        };
                    var tnLink = new BoardMediaLink()
                    {
                        Engine = CoreConstants.Engine.Makaba,
                        Board = link.Board,
                        RelativeUri = f.Thumbnail,
                        KnownMediaType = KnownMediaType.Default
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
                        ReplyCount = Math.Max(t.PostsCount.TryParseWithDefault() + posts.Count(), 0),
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
                BoardSpeed = string.IsNullOrWhiteSpace(data.BoardSpeed) ? "" : $"{data.BoardSpeed} п./час",
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
                IsBoard = data.IsBoard.TryParseWithDefault() != 0,
                IsIndex = data.IsIndex.TryParseWithDefault() != 0,
                ThreadTags = data.Tags != null ? data.Tags.Select(t => new ThreadTagLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = data.Board,
                    Tag = t
                }).Where(t => t.Tag != null).OfType<BoardLinkBase>().ToList() : null
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
        /// Парсить данные каталога.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public CatalogTree ParseCatalogTree(CatalogEntity data, BoardLinkBase link)
        {
            var result = new CatalogTree()
            {
                Link = link,
                Posts = (data.Threads ?? new BoardPost2[0]).OrderBy(p => p.Number.TryParseWithDefault()).Select(p => Parse(p, new ThreadLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = data.Board,
                    Thread = p.Number.TryParseWithDefault()
                }, true)).ToList(),
                Extensions = new List<PostTreeCollectionExtension>(),
                ParentLink = new BoardLink()
                {
                    Engine = CoreConstants.Engine.Makaba,
                    Board = data.Board
                }
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
            return tree.ToPlainText();
        }

        /// <summary>
        /// Парсить иконку.
        /// </summary>
        /// <param name="str">Строка.</param>
        /// <returns>Иконка.</returns>
        [Obsolete("Использовать ParseFlagAndIcon")]
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

        /// <summary>
        /// Парсить иконку и флаг.
        /// </summary>
        /// <param name="str">Строка.</param>
        /// <returns>Иконка и флаг.</returns>
        public FlagAndIcon ParseFlagAndIcon(string str)
        {
            var emptyResult = new FlagAndIcon() { Icon = null, Country = null };
            if (string.IsNullOrWhiteSpace(str))
            {
                return emptyResult;
            }
            try
            {
                var html = new HtmlDocument();
                html.LoadHtml(str);
                if (html.DocumentNode == null || html.DocumentNode.ChildNodes == null)
                {
                    return emptyResult;
                }
                var images = html.DocumentNode
                    .ChildNodes
                    .Where(n => n.NodeType == HtmlNodeType.Element)
                    .Where(n => n.Name.EqualsNc("img"))
                    .ToArray();
                PostTreeIconExtension icon = null;
                PostTreeCountryExtension country = null;

                icon = images
                    .Where(obj => obj.GetAttributeValue("src", null) != null && obj.GetAttributeValue("title", null) != null)
                    .Select(obj => new PostTreeIconExtension()
                    {
                        Uri = obj.GetAttributeValue("src", null),
                        Description = obj.GetAttributeValue("title", null)
                    })
                    .FirstOrDefault();

                country = images
                    .Where(obj => obj.GetAttributeValue("src", null) != null)
                    .Where(obj => (obj.GetAttributeValue("src", null) ?? "").StartsWith("/flags/", StringComparison.OrdinalIgnoreCase))
                    .Select(obj => new PostTreeCountryExtension()
                    {
                        Uri = obj.GetAttributeValue("src", null),
                    })
                    .FirstOrDefault();

                return new FlagAndIcon() { Icon = icon, Country = country};
            }
            catch
            {
                return emptyResult;
            }
        }

        /// <summary>
        /// Флаг и иконка.
        /// </summary>
        public struct FlagAndIcon
        {
            /// <summary>
            /// Иконка.
            /// </summary>
            public PostTreeIconExtension Icon;

            /// <summary>
            /// Флаг.
            /// </summary>
            public PostTreeCountryExtension Country;
        }
    }
}