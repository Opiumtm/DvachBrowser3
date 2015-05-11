using System;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления квоты.
    /// </summary>
    public sealed class QuoteViewModel : ViewModelBase, IQuoteViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <param name="link">Ссылка.</param>
        public QuoteViewModel(IPostViewModel post, BoardLinkBase link)
        {
            if (post == null) throw new ArgumentNullException("post");
            if (link == null) throw new ArgumentNullException("link");
            this.Parent = post;
            this.Link = link;
        }

        /// <summary>
        /// Пост.
        /// </summary>
        public IPostViewModel Parent { get; private set; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; private set; }

        /// <summary>
        /// Заголовок ссылки.
        /// </summary>
        public string Title
        {
            get { return Services.GetServiceOrThrow<ILinkTransformService>().GetBackLinkDisplayString(Link); }
        }

        /// <summary>
        /// Навигация к квоте.
        /// </summary>
        public void NavigateTo()
        {
            Parent.Parent.GotoPost(Link);
        }
    }
}