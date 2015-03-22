using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using DvachBrowser3.Board;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;
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
        /// URI движка.
        /// </summary>
        IEngineUriService EngineUriService { get; }

        /// <summary>
        /// Типы капчи.
        /// </summary>
        CaptchaTypes CaptchaTypes { get; }

        /// <summary>
        /// Сервис коррекции постов.
        /// </summary>
        IPostCorrectionService PostCorrection { get; }

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
        /// Получить ключи для капчи.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="captchaType">Тип капчи.</param>
        /// <returns>Ключи для капчи.</returns>
        IEngineOperationsWithProgress<ICaptchaResult, EngineProgress> GetCaptchaKeys(BoardLinkBase link, CaptchaType captchaType);
        
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
        /// Получить расширенную информацию о борде.
        /// </summary>
        /// <param name="reference">Ссылка на борду.</param>
        /// <returns>Расширенная информация.</returns>
        IEngineOperationsWithProgress<IBoardExtendedDataResult, EngineProgress> GetExtendedBoardData(BoardReference reference);

        /// <summary>
        /// Получить информацию о последнем изменении.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Информация о последнем изменении.</returns>
        IEngineOperationsWithProgress<ILastModifiedCheckResult, EngineProgress> GetResourceLastModified(BoardLinkBase link);
    }
}