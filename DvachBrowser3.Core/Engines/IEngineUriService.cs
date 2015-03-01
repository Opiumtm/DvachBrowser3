using System;
using DvachBrowser3.Links;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Сервис URI движка.
    /// </summary>
    public interface IEngineUriService
    {
        /// <summary>
        /// Ссылка для браузера.
        /// </summary>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        Uri GetBrowserLink(BoardLinkBase link);

        /// <summary>
        /// Попробовать распарсить ссылку на пост.
        /// </summary>
        /// <param name="uri">URI.</param>
        /// <returns>Ссылка.</returns>
        BoardLinkBase TryParsePostLink(string uri);
    }
}