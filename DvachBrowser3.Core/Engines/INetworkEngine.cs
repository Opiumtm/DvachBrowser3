﻿using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using DvachBrowser3.Board;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Сетевой движок.
    /// </summary>
    public interface INetworkEngine
    {
        /// <summary>
        /// Идентификатор движка.
        /// </summary>
        string EngineId { get; }

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Возможности движка.
        /// </summary>
        EngineCapability Capability { get; }

        /// <summary>
        /// Конфигурация.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Получить страницу борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Страница борды.</returns>
        IEngineOperationsWithProgress<IBoardPageResult, EngineProgress> GetBoardPage(BoardLinkBase link);

        /// <summary>
        /// Получить тред.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Тред.</returns>
        IEngineOperationsWithProgress<IThreadResult, EngineProgress> GetThread(BoardLinkBase link);

        /// <summary>
        /// Получить медиафайл.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Медиафайл.</returns>
        IEngineOperationsWithProgress<IMediaResult, EngineProgress> GetMediaFile(BoardLinkBase link);

        /// <summary>
        /// Постинг.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Результат постинга.</returns>
        IEngineOperationsWithProgress<IPostingResult, EngineProgress> Post(PostEntryData data);
            
        /// <summary>
        /// Получить статус треда.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Статус.</returns>
        IEngineOperationsWithProgress<IThreadStatusResult, EngineProgress> GetThreadStatus(BoardLinkBase link);
            
        /// <summary>
        /// Получить список борд.
        /// </summary>
        /// <returns>Список борд.</returns>
        IEngineOperationsWithProgress<IBoardListResult, EngineProgress> GetBoardsList();

        /// <summary>
        /// Получить дефолтные настройки для борды.
        /// </summary>
        /// <returns>Дефолтные настройки.</returns>
        BoardReference GetDefaultOptionsForBoard(BoardLinkBase link);
    }
}