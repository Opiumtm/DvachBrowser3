using System;
using Windows.Storage;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Загрузить маленький медиафайл.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public IEngineOperationsWithProgress<StorageFile, EngineProgress> LoadSmallMediaFile(BoardLinkBase link)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Загрузить полноразмерный медиафайл.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        public IEngineOperationsWithProgress<StorageFile, EngineProgress> LoadFullSizeMediaFile(BoardLinkBase link)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Отправить пост.
        /// </summary>
        /// <param name="data">Данные поста.</param>
        /// <param name="captcha">Данные капчи.</param>
        /// <returns>Ссылка-результат (null, если нет такой ссылки).</returns>
        public IEngineOperationsWithProgress<BoardLinkBase, EngineProgress> Post(PostingData data, CaptchaPostingData captcha)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Загрузить тред в архив.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Идентификатор архива.</returns>
        public IEngineOperationsWithProgress<Guid, EngineProgress> DownloadToArchive(BoardLinkBase link)
        {
            throw new NotImplementedException();
        }
    }
}