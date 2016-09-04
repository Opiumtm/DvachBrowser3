using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posts;
using DvachBrowser3.Styles;
using DvachBrowser3.TextRender;
using DvachBrowser3.Views;
using Ipatov.MarkupRender;
using Template10.Mvvm;
using AttributeRenderProgramElement = DvachBrowser3.TextRender.AttributeRenderProgramElement;
using LineBreakRenderProgramElement = DvachBrowser3.TextRender.LineBreakRenderProgramElement;

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
            var rlogic = new RenderProgramV1Logic();
            rlogic.CreateProgram(logic, post.Comment.Nodes);
        }

        /// <summary>
        /// Создать программу.
        /// </summary>
        /// <returns>Программа.</returns>
        public ITextRender2RenderProgram CreateProgram()
        {
            var logic = new RenderProgramV1Logic();
            return logic.CreateProgram(post.Comment.Nodes);
        }

        /// <summary>
        /// Получить команды для рендеринга.
        /// </summary>
        /// <returns>Команды.</returns>
        public IRenderCommandsSource GetRenderCommands()
        {
            var logic = new RenderProgramV2Logic();
            return logic.CreateProgram(post.Comment.Nodes);
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

        /// <summary>
        /// Получить текст цитаты.
        /// </summary>
        /// <returns>Текст цитаты.</returns>
        public string GetQuoteText()
        {
            var sb = new StringBuilder();
            var text = GetPlainText();
            if (text == null)
            {
                return null;
            }
            var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
            if (Parent?.Link != null)
            {
                sb.AppendLine(linkTransform.GetBackLinkDisplayString(Parent.Link));
            }
            foreach (var line in text)
            {
                sb.Append(">");
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Вызов события по клику на ссылку.
        /// </summary>
        /// <param name="command">Элемент текста.</param>
        public void OnLinkClick(IRenderCommand command)
        {
            if (command != null)
            {
                if (command.Attributes.ContainsKey(CommonTextAttributes.Link))
                {
                    var link = command.Attributes[CommonTextAttributes.Link];
                    var uriLink = link as ITextAttributeData<string>;
                    var boardLink = link as ITextAttributeData<BoardLinkBase>;
                    if (uriLink?.Value != null)
                    {
                        OnLinkClick(new LinkTextRenderAttribute(uriLink.Value));
                    }
                    if (boardLink?.Value != null)
                    {
                        OnLinkClick(new LinkTextRenderAttribute("[data]", boardLink.Value));
                    }
                }
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
                var e = new LinkClickEventArgs(link) { LinkContext = Parent };
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
            public IStyleManager StyleManager { get; } = StyleManagerFactory.Current.GetManager();
        }
    }
}