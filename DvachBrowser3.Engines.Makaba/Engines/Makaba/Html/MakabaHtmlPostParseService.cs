using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using DvachBrowser3.Posts;
using HtmlAgilityPack;

namespace DvachBrowser3.Engines.Makaba.Html
{
    public sealed class MakabaHtmlPostParseService : ServiceBase, IMakabaHtmlPostParseService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public MakabaHtmlPostParseService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Парсить html поста.
        /// </summary>
        /// <param name="comment">Текст поста.</param>
        /// <returns>Узлы дерева поста.</returns>
        public ICollection<PostNodeBase> GetPostNodes(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return new List<PostNodeBase>();
            }
            var html = new HtmlDocument();
            html.LoadHtml(comment);
            ICollection<PostNodeBase> result = new List<PostNodeBase>();
            result = html.DocumentNode.ChildNodes.TreeWalk(result)
                         .GetChildren(node => node.ChildNodes)
                         .If(node => node.NodeType == HtmlNodeType.Text, (node, res) => AddToResult(res, new PostTextNode() { Text = WebUtility.HtmlDecode(node.InnerText) }), node => null)
                         .If(IsPostLink, AddPostLink, node => null)
                         .If(node => node.Name.EqualsNc("br"), (node, res) => AddToResult(res, new PostNodeBreak()))
                         .If(node => node.Name.EqualsNc("p"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Paragraph))
                         .If(node => node.Name.EqualsNc("em"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Italic))
                         .If(node => node.Name.EqualsNc("strong"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Bold))
                         .If(node => GetPreformatNode(node) != null, (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Monospace), node => GetPreformatNode(node).ChildNodes)
                         .If(node => CheckSpan(node, "u"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Underscore))
                         .If(node => CheckSpan(node, "o"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Overscore))
                         .If(node => CheckSpan(node, "spoiler"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Spoiler))
                         .If(node => CheckSpan(node, "s"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Strikeout))
                         .If(node => node.Name.EqualsNc("sub"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Sub))
                         .If(node => node.Name.EqualsNc("sup"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Sup))
                         .If(node => CheckSpan(node, "unkfunc"), (node, res) => CreateAttribute(res, PostNodeBasicAttribute.Quote))
                         .If(node => node.Name.EqualsNc("a") && !string.IsNullOrWhiteSpace(node.GetAttributeValue("href", null)), (node, res) => CreateNode(res, new PostNodeLinkAttribute()
                         {
                             LinkUri = GetLinkText(node.GetAttributeValue("href", null))
                         }))
                         .Else((node, res) => res)
                         .Run();
            return result;
        }

        private bool CheckSpan(HtmlNode node, string className)
        {
            if (!node.Name.EqualsNc("span"))
            {
                return false;
            }
            return node.GetAttributeValue("class", null).EqualsNc(className);
        }

        private HtmlNode GetPreformatNode(HtmlNode node)
        {
            return node
                .WalkTemplate(n => n.Name.EqualsNc("pre"), n => n.FirstNonTextChild())
                .WalkTemplate(n => n.Name.EqualsNc("code"), n => n);
        }

        private ICollection<PostNodeBase> CreateNode(ICollection<PostNodeBase> result, PostNodeAttributeBase attribute)
        {
            var r = new PostNode()
            {
                Attribute = attribute,
                Children = new List<PostNodeBase>()
            };
            result.Add(r);
            return r.Children;
        }

        private ICollection<PostNodeBase> CreateAttribute(ICollection<PostNodeBase> result, PostNodeBasicAttribute attribute)
        {
            return CreateNode(result, new PostNodeAttribute() { Attribute = attribute });
        }

        private ICollection<PostNodeBase> AddToResult(ICollection<PostNodeBase> result, PostNodeBase node)
        {
            result.Add(node);
            return result;
        }

        private bool IsPostLink(HtmlNode node)
        {
            if (!node.Name.EqualsNc("a"))
            {
                return false;
            }
            if (node.GetAttributeValue("href", null) == null)
            {
                return false;
            }
            if ("post-reply-link".Equals(node.GetAttributeValue("class", null), StringComparison.OrdinalIgnoreCase))
            {

            }
            else
            {
                if (node.GetAttributeValue("onclick", null) == null)
                {
                    return false;
                }
                if (!node.GetAttributeValue("onclick", "").StartsWith("highlight", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            var match = PostLinkRegex.Match(node.GetAttributeValue("href", ""));
            if (!match.Success)
            {
                match = PostLinkRegex2.Match(node.GetAttributeValue("href", ""));
            }
            return match.Success;
        }

        private ICollection<PostNodeBase> AddPostLink(HtmlNode node, ICollection<PostNodeBase> result)
        {
            var href = node.GetAttributeValue("href", "");
            var link = Services.GetServiceOrThrow<IMakabaUriService>().TryParsePostLink(href);
            if (link != null)
            {
                result.Add(new PostNodeBoardLink()
                {
                    BoardLink = link
                });
            }
            return result;
        }

        private string GetLinkText(string t)
        {
            return WebUtility.HtmlDecode(t);
        }

        /// <summary>
        /// Регурялное выражение для определения ссылок.
        /// </summary>
        public Regex PostLinkRegex
        {
            get { return Services.GetServiceOrThrow<IMakabaUriService>().PostLinkRegex; }
        }

        /// <summary>
        /// Второе регурялное выражение для определения ссылок.
        /// </summary>
        public Regex PostLinkRegex2
        {
            get { return Services.GetServiceOrThrow<IMakabaUriService>().PostLinkRegex2; }
        }
    }
}