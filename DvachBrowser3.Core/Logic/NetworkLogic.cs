using System;
using Windows.Storage;
using DvachBrowser3.Board;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Logic.NetworkLogic;
using DvachBrowser3.Posting;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сетевая логика.
    /// </summary>
    public sealed class NetworkLogicService : ServiceBase, INetworkLogic
    {
        /// <summary>
        /// Сервис сетевой логики.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public NetworkLogicService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Проверить тред на наличие изменений.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат (null - невозможно проверить или ошибка проверки).</returns>
        public IEngineOperationsWithProgress<bool?, EngineProgress> CheckThread(BoardLinkBase link)
        {
            if ((link.LinkKind & BoardLinkKind.Thread) == 0)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            return new CheckObjectOperation(Services, link);
        }

        /// <summary>
        /// Проверить страницу борды на наличие изменений.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат (null - невозможно проверить или ошибка проверки).</returns>
        public IEngineOperationsWithProgress<bool?, EngineProgress> CheckBoardPage(BoardLinkBase link)
        {
            if ((link.LinkKind & BoardLinkKind.BoardPage) == 0)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            return new CheckObjectOperation(Services, link);
        }

        /// <summary>
        /// Загрузить тред.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="mode">Режим обновления данных.</param>
        /// <returns>Результат.</returns>
        public IEngineOperationsWithProgress<ThreadTree, EngineProgress> LoadThread(BoardLinkBase link, UpdateThreadMode mode = UpdateThreadMode.Default)
        {
            if ((link.LinkKind & BoardLinkKind.Thread) == 0)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            return new LoadThreadOperation(Services, new LoadThreadOperationParameter() { Link = link, Mode = mode });
        }

        /// <summary>
        /// Загрузить страницу борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="mode">Режим обновления данных.</param>
        /// <returns>Результат.</returns>
        public IEngineOperationsWithProgress<BoardPageTree, EngineProgress> LoadBoardPage(BoardLinkBase link, UpdateBoardPageMode mode = UpdateBoardPageMode.CheckETag)
        {
            if ((link.LinkKind & BoardLinkKind.BoardPage) == 0)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            return new LoadBoardPageOperation(Services, new LoadBoardPageOperationParameter() {Link = link, Mode = mode});
        }

        /// <summary>
        /// Загрузить маленький медиафайл.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="mode">Режим загрузки.</param>
        /// <returns>Результат.</returns>
        public IEngineOperationsWithProgress<StorageFile, EngineProgress> LoadMediaFile(BoardLinkBase link, LoadMediaFileMode mode)
        {
            if ((link.LinkKind & BoardLinkKind.Media) == 0 && (link.LinkKind & BoardLinkKind.Youtube) == 0)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            return new LoadMediaFileOperation(Services, new LoadMediaFileOperationParameter() {Link = link, Mode = mode});
        }

        /// <summary>
        /// Отправить пост.
        /// </summary>
        /// <param name="data">Данные поста.</param>
        /// <param name="captcha">Данные капчи.</param>
        /// <param name="mode">Режим постинга.</param>
        /// <returns>Ссылка-результат (null, если нет такой ссылки).</returns>
        public IEngineOperationsWithProgress<BoardLinkBase, EngineProgress> Post(PostingData data, CaptchaPostingData captcha, PostingMode mode = PostingMode.Default)
        {
            if ((data.Link.LinkKind & BoardLinkKind.Thread) == 0 && (data.Link.LinkKind & BoardLinkKind.BoardPage) == 0)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            return new PostOperation(Services, new PostOperationParameter() {Captcha = captcha, Data = data, Mode = mode});
        }

        /// <summary>
        /// Загрузить тред в архив.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Идентификатор архива.</returns>
        public IEngineOperationsWithProgress<Guid, EngineProgress> DownloadToArchive(BoardLinkBase link)
        {
            if ((link.LinkKind & BoardLinkKind.Thread) == 0)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            return new DownloadArchiveOperation(Services, new DownloadArchiveOperationParameter() {Link = link});
        }

        /// <summary>
        /// Проверить избранные треды на обновления.
        /// </summary>
        /// <param name="mode">Режим проверки.</param>
        /// <returns>Данные избранных тредов (null - нет данных).</returns>
        public IEngineOperationsWithProgress<LinkCollection, EngineProgress> CheckFavoriteThreadsForUpdates(CheckFavoriteThreadsMode mode = CheckFavoriteThreadsMode.Default)
        {
            return new CheckFavoritesOperation(Services, new CheckFavoritesParameter() {Mode = mode});
        }

        /// <summary>
        /// Загрузить список борд.
        /// </summary>
        /// <param name="rootLink">Корневая ссылка.</param>
        /// <param name="forceUpdate">Форсировать обновление.</param>
        /// <returns>Список борд (null - нет данных).</returns>
        public IEngineOperationsWithProgress<BoardReferences, EngineProgress> GetBoardReferences(BoardLinkBase rootLink, bool forceUpdate = false)
        {
            return new BoardReferenceOperation(Services, new BoardReferencesParameter() { RootLink = rootLink, ForceUpdate = forceUpdate });
        }

        /// <summary>
        /// Получение каталога.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="sortMode">Режим сортировки.</param>
        /// <param name="mode">Режим обновления.</param>
        /// <returns>Каталог.</returns>
        public IEngineOperationsWithProgress<CatalogTree, EngineProgress> GetCatalog(BoardLinkBase link, CatalogSortMode sortMode = CatalogSortMode.Default,
            UpdateCatalogMode mode = UpdateCatalogMode.Default)
        {
            if ((link.LinkKind & BoardLinkKind.BoardPage) == 0 && (link.LinkKind & BoardLinkKind.ThreadTag) == 0)
            {
                throw new ArgumentException("Неправильный тип ссылки");
            }
            return new CatalogOperation(Services, new CatalogParameter() { Link = link, SortMode = sortMode, UpdateMode = mode });
        }
    }
}