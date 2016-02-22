using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Makaba;
using DvachBrowser3.Posts;
using DvachBrowser3.Storage;
using DvachBrowser3.Styles;
using DvachBrowser3.Views;
using Template10.Mvvm;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления доски.
    /// </summary>
    public sealed class BoardPageViewModel : ViewModelBase, IBoardPageViewModel
    {
        private readonly BoardPageTree data;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Данные.</param>
        public BoardPageViewModel(BoardPageTree data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            this.data = data;
        }

        /// <summary>
        /// Ссылка на борду.
        /// </summary>
        public BoardLinkBase BoardLink { get; private set; }

        /// <summary>
        /// Ссылка на страницу.
        /// </summary>
        public BoardLinkBase PageLink => data?.Link;

        /// <summary>
        /// Ссылка на следующую страницу.
        /// </summary>
        public BoardLinkBase NextPageLink { get; private set; }

        /// <summary>
        /// Ссылка на предыдущую страницу.
        /// </summary>
        public BoardLinkBase PrevPageLink { get; private set; }

        /// <summary>
        /// Получить ссылку на страницу.
        /// </summary>
        /// <param name="page">Страница.</param>
        /// <returns>Ссылка на страницу.</returns>
        public BoardLinkBase GetPageLink(int page)
        {
            return ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().SetBoardPage(data?.Link, page);
        }

        private int[] pages;

        /// <summary>
        /// Получить список страниц.
        /// </summary>
        /// <returns>Список страниц.</returns>
        public int[] GetPages()
        {
            return pages;
        }

        /// <summary>
        /// Треды.
        /// </summary>
        public IList<IThreadPreviewViewModel> Threads { get; } = new List<IThreadPreviewViewModel>();

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Номер страницы.
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Можно перейти к следующей странице.
        /// </summary>
        public bool CanGoNextPage { get; private set; }

        /// <summary>
        /// Можно перейти к предыдущей странице.
        /// </summary>
        public bool CanGoPrevPage { get; private set; }

        /// <summary>
        /// Скорость постинга на борде.
        /// </summary>
        public string BoardSpeed { get; private set; }

        /// <summary>
        /// Баннер.
        /// </summary>
        public IPageBannerViewModel Banner { get; private set; }

        /// <summary>
        /// Клик на ссылку.
        /// </summary>
        public event LinkClickEventHandler LinkClick;

        /// <summary>
        /// Провести асинхронную инициализацию.
        /// </summary>
        /// <returns>Задача.</returns>
        public async Task Initialize()
        {
            var linkTransform = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>();
            var linkHash = ServiceLocator.Current.GetServiceOrThrow<ILinkHashService>();
            var makabaData = data.Extensions?.OfType<MakabaBoardPageExtension>()?.FirstOrDefault()?.Entity;
            if (makabaData != null)
            {
                if (makabaData.Pages != null)
                {
                    pages = makabaData.Pages.Distinct().OrderBy(p => p).ToArray();
                }
                BoardSpeed = !string.IsNullOrWhiteSpace(makabaData.BoardSpeed) ? makabaData.BoardSpeed.Trim() : null;
                if (makabaData.BoardBannerImage != null && makabaData.BoardBannerLink != null && PageLink?.Engine != null)
                {
                    Banner = new MakabaPageBannerViewModel(makabaData, PageLink.Engine);
                }
            }
            if (data.PageNumber != null)
            {
                PageNumber = data.PageNumber.Value;
            }
            else
            {
                PageNumber = linkTransform.GetBoardPage(PageLink);
            }
            BoardLink = linkTransform.BoardLinkFromBoardPageLink(PageLink);
            BoardReferences refs = null;
            var engines = ServiceLocator.Current.GetServiceOrThrow<INetworkEngines>();
            var store = ServiceLocator.Current.GetServiceOrThrow<IStorageService>();
            if (engines.ListEngines().Any(e => StringComparer.OrdinalIgnoreCase.Equals(data.ParentLink?.Engine, e)))
            {
                var engine = engines.GetEngineById(data.ParentLink?.Engine);
                var rootLink = engine.RootLink;
                refs = await store.ThreadData.LoadBoardReferences(rootLink);
            }
            var favs = (await store.ThreadData.FavoriteBoards.LoadLinkCollectionForReadOnly()) as BoardLinkCollection;
            FavoriteBoardInfo fav = null;
            if (favs?.BoardInfo != null)
            {
                var hash = linkHash.GetLinkHash(BoardLink);
                if (favs.BoardInfo.ContainsKey(hash))
                {
                    fav = favs.BoardInfo[hash];
                }
            }
            BoardReference bref = null;
            if (refs?.References != null)
            {
                var cmp = linkHash.GetComparer();
                bref = refs.References.FirstOrDefault(r => cmp.Equals(r.Link, BoardLink));
            }
            MakabaBoardReferenceExtension mref = null;
            if (bref?.Extensions != null)
            {
                mref = bref.Extensions.OfType<MakabaBoardReferenceExtension>().FirstOrDefault();
            }
            if (pages == null)
            {
                if (data.MaxPage != null)
                {
                    var l = new List<int>();
                    for (var i = 0; i <= data.MaxPage.Value; i++)
                    {
                        l.Add(i);
                    }
                    pages = l.ToArray();
                }
                else if (mref != null)
                {
                    var l = new List<int>();
                    for (var i = 0; i < mref.Pages; i++)
                    {
                        l.Add(i);
                    }
                    pages = l.ToArray();
                }
                else
                {
                    pages = new int[0];
                }
            }
            CanGoNextPage = pages.Any(p => p == PageNumber + 1);
            CanGoPrevPage = pages.Any(p => p == PageNumber - 1);
            NextPageLink = linkTransform.SetBoardPage(PageLink, PageNumber + 1);
            PrevPageLink = linkTransform.SetBoardPage(PageLink, PageNumber - 1);
            var title = (!string.IsNullOrEmpty(fav?.DisplayName) ? fav?.DisplayName : bref?.DisplayName) ?? "";
            Title = $"/{linkTransform.GetBoardShortName(PageLink)}/ {title}";
            Threads.Clear();
            if (data.Threads != null)
            {
                foreach (var tv in from t in data.Threads where t != null select new ThreadPreviewViewModel(this, t))
                {
                    tv.LinkClick += TvOnLinkClick;
                    Threads.Add(tv);
                }
            }
        }

        private bool isObsolete;

        /// <summary>
        /// Страница устарела.
        /// </summary>
        public bool IsObsolete
        {
            get { return isObsolete; }
            set
            {
                isObsolete = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Пометить как устаревшую.
        /// </summary>
        public void TriggerObsolete()
        {
            IsObsolete = true;
        }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager { get; } = new StyleManager();

        private void TvOnLinkClick(object sender, LinkClickEventArgs e)
        {
            LinkClick?.Invoke(sender, e);
        }
    }
}