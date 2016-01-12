using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posts;
using DvachBrowser3.Styles;
using DvachBrowser3.TextRender;
using DvachBrowser3.Views;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления текста поста.
    /// </summary>
    public sealed class PostTextViewModel : PostPartViewModelBase, IPostTextViewModel
    {
        private readonly PostTree post;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parent">Родительская модель.</param>
        /// <param name="post">Пост.</param>
        public PostTextViewModel(IPostViewModel parent, PostTree post) : base(parent)
        {
            this.UniqueId = Guid.NewGuid();
            this.post = post;
            if (post?.Quotes != null)
            {
                var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
                var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
                var q = post.Quotes
                    .Distinct(linkHash.GetComparer())
                    .OrderBy(a => a, linkTransform.GetLinkComparer())
                    .ToArray();
                foreach (var a in q)
                {
                    HasQuotes = true;
                    Quotes.Add(new QuoteViewModel()
                    {
                        Parent = parent,
                        Name = linkTransform.GetBackLinkDisplayString(a),
                        Link = a
                    });
                }
            }
        }

        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public Guid UniqueId { get; }

        /// <summary>
        /// Отобразить текст.
        /// </summary>
        /// <param name="logic">Логика рендеринга поста.</param>
        public void RenderText(ITextRenderLogic logic)
        {
            if (post?.Comment == null)
            {
                return;
            }
            var lastBreak = false;
            RenderElements(logic, post.Comment.Nodes, ref lastBreak);
            logic.Flush();
        }

        /// <summary>
        /// Клик на ссылку.
        /// </summary>
        public event LinkClickEventHandler LinkClick;

        /// <summary>
        /// Квоты.
        /// </summary>
        public IList<IPostQuoteViewModel> Quotes { get; } = new ObservableCollection<IPostQuoteViewModel>();

        private bool hasQuotes = false;

        /// <summary>
        /// Есть квоты.
        /// </summary>
        public bool HasQuotes
        {
            get { return hasQuotes; }
            private set
            {
                hasQuotes = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Получить текст без разметки.
        /// </summary>
        /// <returns>Текст.</returns>
        public IList<string> GetPlainText()
        {
            return post?.ToPlainText();
        }

        private void RenderElements(ITextRenderLogic logic, IEnumerable<PostNodeBase> nodes, ref bool lastBreak)
        {
            if (nodes == null)
            {
                return;
            }
            foreach (var node in nodes)
            {
                RenderElement(logic, node, ref lastBreak);
            }
        }

        private void RenderElement(ITextRenderLogic logic, PostNodeBase node, ref bool lastBreak)
        {
            if (node == null)
            {
                return;
            }
            if (node is PostNodeBreak)
            {
                logic.PushProgramElement(new LineBreakRenderProgramElement());
                lastBreak = true;
            }
            else if (node is PostTextNode)
            {
                var tnode = (PostTextNode) node;
                logic.PushProgramElement(new TextRenderProgramElement(tnode.Text ?? ""));
                lastBreak = false;
            }
            else if (node is PostNode)
            {                
                RenderAttributedNodes(logic, (PostNode)node, ref lastBreak);
            }
            else if (node is PostNodeBoardLink)
            {
                var lnode = (PostNodeBoardLink) node;
                if (lnode.BoardLink != null)
                {
                    var lattr = new LinkTextRenderAttribute("[data]", lnode.BoardLink);
                    logic.PushProgramElement(new AttributeRenderProgramElement(lattr, true));
                    logic.PushProgramElement(new TextRenderProgramElement(GetBoardLinkName(lnode.BoardLink)));
                    logic.PushProgramElement(new AttributeRenderProgramElement(lattr, false));
                    lastBreak = false;
                }
            }
        }

        private string GetBoardLinkName(BoardLinkBase link)
        {
            return ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().GetBackLinkDisplayString(link);
        }

        private void RenderAttributedNodes(ITextRenderLogic logic, PostNode node, ref bool lastBreak)
        {
            if (node == null)
            {
                return;
            }
            var nodeAttr = node.Attribute as PostNodeAttribute;
            var nodeLink = node.Attribute as PostNodeLinkAttribute;

            ITextRenderAttribute attribute = null;

            ITextRenderLinkAttribute linkAttribute = null;

            bool isParagraph = false;

            if (nodeAttr != null)
            {
                if (nodeAttr.Attribute == PostNodeBasicAttribute.Paragraph)
                {
                    isParagraph = true;
                }
                else
                {
                    attribute = CreateAttribute(nodeAttr.Attribute);
                }
            }

            if (nodeLink?.LinkUri != null)
            {
                linkAttribute = new LinkTextRenderAttribute(nodeLink.LinkUri);
            }

            if (attribute != null)
            {
                logic.PushProgramElement(new AttributeRenderProgramElement(attribute, true));
            }

            if (linkAttribute != null)
            {
                logic.PushProgramElement(new AttributeRenderProgramElement(linkAttribute, true));
            }

            if (!lastBreak && isParagraph)
            {
                logic.PushProgramElement(new LineBreakRenderProgramElement());
                lastBreak = true;
            }

            RenderElements(logic, node.Children, ref lastBreak);

            if (attribute != null)
            {
                logic.PushProgramElement(new AttributeRenderProgramElement(attribute, false));
            }

            if (linkAttribute != null)
            {
                logic.PushProgramElement(new AttributeRenderProgramElement(linkAttribute, false));
            }

            if (!lastBreak && isParagraph)
            {
                logic.PushProgramElement(new LineBreakRenderProgramElement());
            }
        }

        private ITextRenderAttribute CreateAttribute(PostNodeBasicAttribute attr)
        {
            switch (attr)
            {
                case PostNodeBasicAttribute.Bold:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Bold);
                case PostNodeBasicAttribute.Italic:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Italic);
                case PostNodeBasicAttribute.Monospace:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Fixed);
                case PostNodeBasicAttribute.Overscore:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Overline);
                case PostNodeBasicAttribute.Quote:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Quote);
                case PostNodeBasicAttribute.Spoiler:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Spoiler);
                case PostNodeBasicAttribute.Strikeout:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Strikethrough);
                case PostNodeBasicAttribute.Sub:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Subscript);
                case PostNodeBasicAttribute.Sup:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Superscript);
                case PostNodeBasicAttribute.Underscore:
                    return new FlagTextRenderAttribute(CommonTextRenderAttributes.Undeline);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Обратный вызов.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        public void OnLinkClick(ITextRenderLinkAttribute link)
        {
            if (link != null)
            {
                var e = new LinkClickEventArgs(link);
                LinkClick?.Invoke(this, e);
                ViewModelEvents.LinkClick.RaiseEvent(this, e);
            }
        }

        private class QuoteViewModel : ViewModelBase, IPostQuoteViewModel
        {
            /// <summary>
            /// Родительская модель.
            /// </summary>
            public IPostViewModel Parent { get; set; }

            /// <summary>
            /// Имя.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Ссылка.
            /// </summary>
            public BoardLinkBase Link { get; set; }

            /// <summary>
            /// Менеджер стилей.
            /// </summary>
            public IStyleManager StyleManager => Shell.StyleManager;
        }
    }
}