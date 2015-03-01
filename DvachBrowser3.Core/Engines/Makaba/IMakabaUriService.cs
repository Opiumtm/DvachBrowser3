using System;
using System.Text.RegularExpressions;
using DvachBrowser3.Links;

namespace DvachBrowser3.Engines.Makaba
{
    /// <summary>
    /// Сервис URI для макабы.
    /// </summary>
    public interface IMakabaUriService
    {
        /// <summary>
        /// Получить URI страницы борды.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <param name="isReferer">Реферер.</param>
        /// <returns>URI.</returns>
        Uri GetBoardPageUri(BoardPageLink link, bool isReferer);

        /// <summary>
        /// Попробовать распарсить ссылку.
        /// </summary>
        /// <param name="uri">URI.</param>
        /// <returns>Ссылка.</returns>
        BoardLinkBase TryParsePostLink(string uri);

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