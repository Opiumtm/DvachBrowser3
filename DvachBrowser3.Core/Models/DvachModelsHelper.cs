using System.Collections.Generic;
using System.Text;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник.
    /// </summary>
    public static class DvachModelsHelper
    {
        /// <summary>
        /// Перевести в простой текст.
        /// </summary>
        /// <param name="tree">Дерево поста.</param>
        /// <returns>Текст.</returns>
        public static IList<string> ToPlainText(this PostTree tree)
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
                    }
                    else if (tl != null)
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

    }
}