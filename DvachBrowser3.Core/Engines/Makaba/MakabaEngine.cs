﻿using System;
using DvachBrowser3.Board;
using DvachBrowser3.Configuration.Makaba;
using DvachBrowser3.Engines.Makaba.BoardInfo;
using DvachBrowser3.Engines.Makaba.Operations;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines.Makaba
{
    /// <summary>
    /// Сетевой движок "Makaba".
    /// </summary>
    public sealed class MakabaEngine : ServiceBase, INetworkEngine
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public MakabaEngine(IServiceProvider services) : base(services)
        {
            configuration = new MakabaEngineConfig();
            postCorrection = new MakabaPostCorrectionService(services);
        }

        /// <summary>
        /// Идентификатор движка.
        /// </summary>
        public string EngineId
        {
            get { return CoreConstants.Engine.Makaba; }
        }

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        public string DisplayName
        {
            get { return "2ch.hk (Makaba)"; }
        }

        /// <summary>
        /// Возможности движка.
        /// </summary>
        public EngineCapability Capability
        {
            get
            {
                return 
                    EngineCapability.BoardsListRequest | 
                    EngineCapability.LastModifiedRequest | 
                    EngineCapability.PartialThreadRequest | 
                    EngineCapability.ThreadStatusRequest | 
                    EngineCapability.SearchRequest |
                    EngineCapability.TopPostsRequest;
            }
        }

        private readonly IMakabaEngineConfig configuration;

        /// <summary>
        /// Конфигурация.
        /// </summary>
        public IConfiguration Configuration
        {
            get { return configuration; }
        }

        /// <summary>
        /// URI движка.
        /// </summary>
        public IEngineUriService EngineUriService
        {
            get { return Services.GetServiceOrThrow<IMakabaUriService>(); }
        }

        /// <summary>
        /// Типы капчи.
        /// </summary>
        public CaptchaTypes CaptchaTypes
        {
            get
            {
                return CaptchaTypes.Yandex | CaptchaTypes.Recaptcha;
            }
        }

        private readonly IPostCorrectionService postCorrection;

        /// <summary>
        /// Сервис коррекции постов.
        /// </summary>
        public IPostCorrectionService PostCorrection
        {
            get { return postCorrection; }
        }

        /// <summary>
        /// Корневая ссылка.
        /// </summary>
        public BoardLinkBase RootLink
        {
            get
            {
                return new RootLink() { Engine = CoreConstants.Engine.Makaba };
            }
        }

        /// <summary>
        /// Получить страницу борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Страница борды.</returns>
        public IEngineOperationsWithProgress<IBoardPageResult, EngineProgress> GetBoardPage(BoardLinkBase link)
        {
            return new MakabaGetBoardPageOperation(link, Services);
        }

        /// <summary>
        /// Получить тред.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Тред.</returns>
        public IEngineOperationsWithProgress<IThreadResult, EngineProgress> GetThread(BoardLinkBase link)
        {
            if (link is ThreadPartLink)
            {
                return new MakabaGetThreadPartOperation(link, Services);
            }
            return new MakabaGetThreadOperation(link, Services);
        }

        /// <summary>
        /// Получить медиафайл.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Медиафайл.</returns>
        public IEngineOperationsWithProgress<IMediaResult, EngineProgress> GetMediaFile(BoardLinkBase link)
        {
            return new MakabaGetMediaFileOperation(link, Services);
        }

        /// <summary>
        /// Получить ключи для капчи.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="captchaType">Тип капчи.</param>
        /// <returns>Ключи для капчи.</returns>
        public IEngineOperationsWithProgress<ICaptchaResult, EngineProgress> GetCaptchaKeys(BoardLinkBase link, CaptchaType captchaType)
        {
            return new MakabaGetCaptchaOperation(captchaType, Services);
        }

        /// <summary>
        /// Постинг.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Результат постинга.</returns>
        public IEngineOperationsWithProgress<IPostingResult, EngineProgress> Post(PostEntryData data)
        {
            return new MakabaPostOperation(data, Services);
        }

        /// <summary>
        /// Получить статус треда.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Статус.</returns>
        public IEngineOperationsWithProgress<IThreadStatusResult, EngineProgress> GetThreadStatus(BoardLinkBase link)
        {
            return new MakabaThreadStatusOperation(link, Services);
        }

        /// <summary>
        /// Получить список борд.
        /// </summary>
        /// <returns>Список борд.</returns>
        public IEngineOperationsWithProgress<IBoardListResult, EngineProgress> GetBoardsList()
        {
            return new MakabaGetBoardsOperation(Empty.Value, Services);
        }

        /// <summary>
        /// Получить информацию о борде по умолчанию.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <param name="boardId">Идентификатор борды.</param>
        /// <returns>Результат.</returns>
        public BoardReference GetDefaultBoardData(string category, string boardId)
        {
            return Services.GetServiceOrThrow<IMakabaBoardInfoParser>().Default(category, boardId);
        }

        /// <summary>
        /// Получить информацию о последнем изменении.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Информация о последнем изменении.</returns>
        public IEngineOperationsWithProgress<ILastModifiedCheckResult, EngineProgress> GetResourceLastModified(BoardLinkBase link)
        {
            return new MakabaLastModifiedOperation(link, Services);
        }
    }
}