﻿using System;
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
        /// Получить URI капчи.
        /// </summary>
        /// <param name="captchaType">Тип капчи.</param>
        /// <returns>URI капчи.</returns>
        Uri GetCaptchaUri(CaptchaType captchaType);

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
        /// Регулярное выражение URI поста.
        /// </summary>
        Regex PostLinkRegex { get; }

        /// <summary>
        /// Регулярное выражение URI поста.
        /// </summary>
        Regex PostLinkRegex2 { get; }
    }
}