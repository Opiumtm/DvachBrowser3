using System;
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
    }
}