using System;
using System.Threading.Tasks;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic;
using DvachBrowser3.Posts;
using DvachBrowser3.Styles;
using DvachBrowser3.Views;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления каталога.
    /// </summary>
    public sealed class BoardCatalogViewModel : UpdateablePostCollectionViewModelBase<CatalogTree>, IBoardCatalogViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public BoardCatalogViewModel(BoardLinkBase link)
        {
            if (link == null) throw new ArgumentNullException(nameof(link));
            if (link.LinkKind != BoardLinkKind.Catalog)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            Link = link;
            BoardLink = ServiceLocator.Current.GetServiceOrThrow<ILinkTransformService>().BoardLinkFromAnyLink(Link);
            AppHelpers.DispatchAction(async () =>
            {
                var t = await BoardTitleHelper.GetBoardTitle(BoardLink) ?? "";
                Title = $"[к] {t}";
            });
        }

        /// <summary>
        /// Создать модель представления поста.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Модель представления поста.</returns>
        protected override IPostViewModel CreatePostViewModel(PostTree post)
        {
            return new PostViewModel(post, this);
        }

        /// <summary>
        /// Фабрика операций.
        /// </summary>
        /// <param name="o">Данные.</param>
        /// <returns>Операция.</returns>
        protected override IEngineOperationsWithProgress<CatalogTree, EngineProgress> UpdateOperationFactory(object o)
        {
            var updateMode = BoardCatalogUpdateMode.Load;
            if (o != null)
            {
                if (o.GetType() == typeof(BoardCatalogUpdateMode))
                {
                    updateMode = (BoardCatalogUpdateMode)o;
                }
            }
            return new BoardCatalogLoaderOperation(ServiceLocator.Current, new BoardCatalogLoaderArgument()
            {
                Link = Link,
                UpdateMode = updateMode
            });
        }

        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; }

        /// <summary>
        /// Заголовок борды.
        /// </summary>
        public BoardLinkBase BoardLink { get; }

        private string title;

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title
        {
            get { return title; }
            private set
            {
                title = value;
                RaisePropertyChanged();
            }
        }

        private bool isBackNavigatedToViewModel;

        /// <summary>
        /// Была навигация назад.
        /// </summary>
        public bool IsBackNavigatedToViewModel
        {
            get { return isBackNavigatedToViewModel; }
            set
            {
                isBackNavigatedToViewModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Менеджер стилей.
        /// </summary>
        public IStyleManager StyleManager => Shell.StyleManager;

        /// <summary>
        /// Запуск.
        /// </summary>
        /// <returns>Задача.</returns>
        public override Task Start()
        {
            if (IsBackNavigatedToViewModel)
            {
                Update.Start2(BoardCatalogUpdateMode.GetFromCache);
            }
            else
            {
                Update.Start2(BoardCatalogUpdateMode.Load);
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Сливать и сортировать посты.
        /// </summary>
        protected override bool MergeAndSortPosts => false;

        /// <summary>
        /// Обновить счётчик постов.
        /// </summary>
        protected override void UpdatePostCounters()
        {
            foreach (var p in Posts)
            {
                if (p.Counter != null)
                {
                    p.Counter = null;
                }
            }
        }
    }
}