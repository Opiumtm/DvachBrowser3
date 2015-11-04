using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
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
        /// Имя ресурса.
        /// </summary>
        string ResourceName { get; }

        /// <summary>
        /// Цвет плитки для UI.
        /// </summary>
        Color TileBackgroundColor { get; }

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
        /// Корневая ссылка.
        /// </summary>
        BoardLinkBase RootLink { get; }

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
        /// Получить информацию о борде по умолчанию.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <param name="boardId">Идентификатор борды.</param>
        /// <returns>Результат.</returns>
        BoardReference GetDefaultBoardData(string category, string boardId);

        /// <summary>
        /// Получить информацию о последнем изменении.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Информация о последнем изменении.</returns>
        IEngineOperationsWithProgress<ILastModifiedCheckResult, EngineProgress> GetResourceLastModified(BoardLinkBase link);

        /// <summary>
        /// Проверить возможность постить без капчи.
        /// </summary>
        /// <param name="postLink">Ссылка для постинга (корневая ссылка - для проверки возможности в целом).</param>
        /// <returns>Результат.</returns>
        IEngineOperationsWithProgress<INoCaptchaCheckResult, EngineProgress> CheckNoCaptchaAbility(BoardLinkBase postLink);

        /// <summary>
        /// Получить ссылку на борду.
        /// </summary>
        /// <param name="shortName">Короткое имя борды.</param>
        /// <returns>Ссылка на борду.</returns>
        BoardLinkBase CreateBoardLink(string shortName);
    }
}