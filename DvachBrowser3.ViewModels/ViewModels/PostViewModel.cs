using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Navigation;
using DvachBrowser3.Posts;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления поста.
    /// </summary>
    public class PostViewModel : ViewModelBase, IPostViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="parent">Родительская коллекция.</param>
        public PostViewModel(PostTree data, IPostCollectionViewModel parent)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (parent == null) throw new ArgumentNullException("parent");
            Data = data;
            Parent = parent;
            if (Data.Quotes != null)
            {
                var comparer = Services.GetServiceOrThrow<ILinkTransformService>().GetLinkComparer();
                Quotes = Data.Quotes.OrderBy(l => l, comparer).Select(l => new QuoteViewModel(this, l)).OfType<IQuoteViewModel>().ToList();
            }
            else
            {
                Quotes = new List<IQuoteViewModel>();
            }
            if (Data.Media != null)
            {
                Media = Data.Media.Select(m => new ThumbnailViewModel(this, m)).OfType<IThumbnailViewModel>().ToList();
            }
            else
            {
                Media = new List<IThumbnailViewModel>();
            }
            hyperlink = new Lazy<string>(GetHyperlink);
            if (data.Extensions == null)
            {
                data.Extensions = new List<PostTreeExtension>();
            }
            var posterExtension = data.Extensions.OfType<PostTreePosterExtension>().FirstOrDefault();
            if (posterExtension != null)
            {
                Poster = new PosterViewModel(posterExtension, this);
            }
            if (data.Link != null)
            {
                var iconExtension = data.Extensions.OfType<PostTreeIconExtension>().FirstOrDefault();
                if (iconExtension != null)
                {
                    Icon = new GlobalBoardIconViewModel(this, new MediaLink() { Engine = data.Link.Engine, IsAbsolute = false, RelativeUri = iconExtension.Uri }, true, iconExtension.Description);
                }
                var countryExtension = data.Extensions.OfType<PostTreeCountryExtension>().FirstOrDefault();
                if (countryExtension != null)
                {
                    Country = new GlobalBoardIconViewModel(this, new MediaLink() { Engine = data.Link.Engine, IsAbsolute = false, RelativeUri = countryExtension.Uri}, true);
                }
            }
            if (!string.IsNullOrWhiteSpace(data.Email))
            {
                var mail = data.Email.Trim();
                if (mail.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                {
                    mail = mail.Remove(0, "mailto:".Length);
                    mail = mail.Trim();
                }
                if ("sage".Equals(mail, StringComparison.OrdinalIgnoreCase))
                {
                    Email = "";
                    HasEmail = false;
                }
                else
                {
                    Email = mail;
                    HasEmail = true;
                }
            }
            else
            {
                Email = "";
                HasEmail = false;
            }
            Flags = new PostFlagsViewModel(this, data.Flags);
        }

        /// <summary>
        /// Родительская коллекция.
        /// </summary>
        public IPostCollectionViewModel Parent { get; private set; }

        /// <summary>
        /// Данные.
        /// </summary>
        public PostTree Data { get; private set; }

        /// <summary>
        /// Номер поста.
        /// </summary>
        public string PostNumber
        {
            get { return Services.GetServiceOrThrow<ILinkTransformService>().GetPostNumberDisplayString(Data.Link); }
        }

        /// <summary>
        /// Номер треда.
        /// </summary>
        public string ThreadNumber
        {
            get { return Services.GetServiceOrThrow<ILinkTransformService>().GetPostNumberDisplayString(Data.ParentLink); }
        }

        /// <summary>
        /// Ключ навигации.
        /// </summary>
        public INavigationKey NavigationKey
        {
            get { return LinkKeyService.GetKey(Data.Link); }
        }

        /// <summary>
        /// Родительский ключ навигации.
        /// </summary>
        public INavigationKey ParentNavigationKey
        {
            get { return LinkKeyService.GetKey(Data.ParentLink); }
        }

        /// <summary>
        /// Ответы на этот пост.
        /// </summary>
        public IList<IQuoteViewModel> Quotes { get; private set; }

        public IList<IThumbnailViewModel> Media { get; private set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Subject
        {
            get { return Data.Subject ?? ""; }
        }

        /// <summary>
        /// Дата.
        /// </summary>
        public string Date
        {
            get { return Configuration.View.BoardNativeDates ? Data.BoardSpecificDate ?? "" : CommonDate; }
        }

        /// <summary>
        /// Постер.
        /// </summary>
        public IPosterViewModel Poster { get; private set; }

        /// <summary>
        /// Страна.
        /// </summary>
        public IIconViewModel Country { get; private set; }

        /// <summary>
        /// Иконка.
        /// </summary>
        public IIconWithNameViewModel Icon { get; private set; }

        /// <summary>
        /// Почта.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Есть почта.
        /// </summary>
        public bool HasEmail { get; private set; }

        /// <summary>
        /// Флаги.
        /// </summary>
        public IPostFlagsViewModel Flags { get; private set; }

        /// <summary>
        /// Счётчик постов.
        /// </summary>
        public int Counter
        {
            get { return Data.Counter; }
        }

        /// <summary>
        /// Хэш поста.
        /// </summary>
        public string Hash
        {
            get { return Data.Hash; }
        }

        /// <summary>
        /// Текст поста.
        /// </summary>
        public PostNodes Comment
        {
            get { return Data.Comment ?? new PostNodes() {Nodes = new List<PostNodeBase>()}; }
        }

        private string CommonDate
        {
            get { return Services.GetServiceOrThrow<IDateService>().ToUserString(Data.Date); }
        }

        private readonly Lazy<string> hyperlink;

        private string GetHyperlink()
        {
            var engines = Services.GetServiceOrThrow<INetworkEngines>();
            var engine = engines.GetEngineById(Data.Link.Engine);
            return engine.EngineUriService.GetBrowserLink(Data.Link).ToString();
        }

        /// <summary>
        /// Гиперссылка.
        /// </summary>
        public string Hyperlink
        {
            get { return hyperlink.Value; }
        }

        /// <summary>
        /// Вход на страницу.
        /// </summary>
        public void OnPageEntry()
        {           
            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged("Date");
            // ReSharper restore ExplicitCallerInfoArgument
        }

        /// <summary>
        /// Получить токен отмены.
        /// </summary>
        /// <returns>Токен отмены.</returns>
        CancellationToken ICancellationTokenSource.GetToken()
        {
            return Parent.GetToken();
        }
    }
}