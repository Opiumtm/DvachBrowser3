using System;
using Windows.Storage;
using DvachBrowser3.Engines;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Logic
{
    /// <summary>
    /// Сетевая логика.
    /// </summary>
    public interface INetworkLogic
    {
        /// <summary>
        /// Проверить тред на наличие изменений.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат (null - невозможно проверить или ошибка проверки).</returns>
        IEngineOperationsWithProgress<bool?, EngineProgress> CheckThread(BoardLinkBase link);

        /// <summary>
        /// Проверить страницу борды на наличие изменений.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат (null - невозможно проверить или ошибка проверки).</returns>
        IEngineOperationsWithProgress<bool?, EngineProgress> CheckBoardPage(BoardLinkBase link);

        /// <summary>
        /// Загрузить тред.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="mode">Режим обновления данных.</param>
        /// <returns>Результат.</returns>
        IEngineOperationsWithProgress<ThreadTree, EngineProgress> LoadThread(BoardLinkBase link, UpdateThreadMode mode = UpdateThreadMode.Default);

        /// <summary>
        /// Загрузить страницу борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="mode">Режим обновления данных.</param>
        /// <returns>Результат.</returns>
        IEngineOperationsWithProgress<BoardPageTree, EngineProgress> LoadBoardPage(BoardLinkBase link, UpdateBoardPageMode mode = UpdateBoardPageMode.Default);

        /// <summary>
        /// Загрузить маленький медиафайл.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="mode">Режим загрузки.</param>
        /// <returns>Результат.</returns>
        IEngineOperationsWithProgress<StorageFile, EngineProgress> LoadMediaFile(BoardLinkBase link, LoadMediaFileMode mode);

        /// <summary>
        /// Отправить пост.
        /// </summary>
        /// <param name="data">Данные поста.</param>
        /// <param name="captcha">Данные капчи.</param>
        /// <param name="mode">Режим постинга.</param>
        /// <returns>Ссылка-результат (null, если нет такой ссылки).</returns>
        IEngineOperationsWithProgress<BoardLinkBase, EngineProgress> Post(PostingData data, CaptchaPostingData captcha, PostingMode mode = PostingMode.Default);
        
        /// <summary>
        /// Загрузить тред в архив.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Идентификатор архива.</returns>
        IEngineOperationsWithProgress<Guid, EngineProgress> DownloadToArchive(BoardLinkBase link);

        /// <summary>
        /// Проверить избранные треды на обновления.
        /// </summary>
        /// <param name="mode">Режим проверки.</param>
        /// <returns>Данные избранных тредов (null - нет данных).</returns>
        IEngineOperationsWithProgress<LinkCollection, EngineProgress> CheckFavoriteThreadsForUpdates(CheckFavoriteThreadsMode mode = CheckFavoriteThreadsMode.Default);
    }
}