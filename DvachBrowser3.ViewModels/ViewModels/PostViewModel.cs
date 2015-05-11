using System;
using System.Collections.Generic;
using System.Linq;
using DvachBrowser3.Engines;
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
            hyperlink = new Lazy<string>(GetHyperlink);
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
    }
}