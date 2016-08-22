using System;
using System.Text.RegularExpressions;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines.Makaba
{
    /// <summary>
    /// Сервис URI для макабы.
    /// </summary>
    public interface IMakabaUriService : IEngineUriService
    {
        /// <summary>
        /// Получить URI страницы борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="isReferer">Реферер.</param>
        /// <returns>URI.</returns>
        Uri GetBoardPageUri(BoardPageLink link, bool isReferer);

        /// <summary>
        /// Получить URI треда.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="isReferer">Реферер.</param>
        /// <returns>URI.</returns>
        Uri GetThreadUri(ThreadLink link, bool isReferer);

        /// <summary>
        /// Получить URI части треда.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="isReferer">Реферер.</param>
        /// <returns>URI.</returns>
        Uri GetThreadPartUri(ThreadPartLink link, bool isReferer);

        /// <summary>
        /// Ссылка JSON.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        Uri GetJsonLink(BoardLinkBase link);

        /// <summary>
        /// Ссылка HTML.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        Uri GetHtmlLink(BoardLinkBase link);

        /// <summary>
        /// Ссылка на медиа.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Ссылка.</returns>
        Uri GetMediaLink(BoardMediaLink link);

        /// <summary>
        /// Ссылка на медиа.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Ссылка.</returns>
        Uri GetMediaLink(MediaLink link);

        /// <summary>
        /// Ссылка на медиа.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Ссылка.</returns>
        Uri GetMediaLink(YoutubeLink link);

        /// <summary>
        /// Получить URI капчи.
        /// </summary>
        /// <param name="captchaType">Тип капчи.</param>
        /// <param name="forThread">Для треда.</param>
        /// <returns>URI капчи.</returns>
        Uri GetCaptchaUri(CaptchaType captchaType, bool forThread);

        /// <summary>
        /// Получить URI капчи (V2).
        /// </summary>
        /// <param name="captchaType">Тип капчи.</param>
        /// <param name="board">Борда.</param>
        /// <param name="thread">Тред.</param>
        /// <returns>URI капчи.</returns>
        Uri GetCaptchaUriV2(CaptchaType captchaType, string board, int? thread);

        /// <summary>
        /// Получить URI изображения капчи V2.
        /// </summary>
        /// <param name="captchaType">Тип капчи.</param>
        /// <param name="id">Идентификатор.</param>
        /// <returns>URI изображения.</returns>
        Uri GetCaptchaV2ImageUri(CaptchaType captchaType, string id);

        /// <summary>
        /// Получить URI для постинга.
        /// </summary>
        /// <returns>URI для постинга.</returns>
        Uri GetPostingUri();

        /// <summary>
        /// Получить информацию о треде.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Информация.</returns>
        Uri GetLastThreadInfoUri(ThreadLink link);

        /// <summary>
        /// Получить URI списка борд.
        /// </summary>
        /// <returns>Ссылка.</returns>
        Uri GetBoardsListUri();

        /// <summary>
        /// Регулярное выражение URI поста.
        /// </summary>
        Regex PostLinkRegex { get; }

        /// <summary>
        /// Регулярное выражение URI поста.
        /// </summary>
        Regex PostLinkRegex2 { get; }

        /// <summary>
        /// Получить URI для постинга без капчи.
        /// </summary>
        /// <param name="check">Проверить.</param>
        /// <param name="appId">ID приложения.</param>
        /// <returns>Ссылка.</returns>
        Uri GetNocaptchaUri(bool check, string appId);

        /// <summary>
        /// Получить URI каталога.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="html">Для браузера.</param>
        /// <returns>URI каталога.</returns>
        Uri GetCatalogUri(BoardCatalogLink link, bool html);
    }
}