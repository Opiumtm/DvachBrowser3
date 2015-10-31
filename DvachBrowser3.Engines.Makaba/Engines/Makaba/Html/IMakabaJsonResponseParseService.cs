using DvachBrowser3.Engines.Makaba.Json;
using DvachBrowser3.Links;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Engines.Makaba.Html
{
    /// <summary>
    /// Сервис парсинга ответов JSON.
    /// </summary>
    public interface IMakabaJsonResponseParseService
    {
        /// <summary>
        /// Парсить данные страницы борды.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        BoardPageTree ParseBoardPage(BoardEntity2 data, BoardPageLink link);

        /// <summary>
        /// Парсить данные треда.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        ThreadTree ParseThread(BoardEntity2 data, ThreadLink link);

        /// <summary>
        /// Парсить частичные данные треда.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <param name="link">Ссылка.</param>
        /// <returns>Результат.</returns>
        ThreadTreePartial ParseThreadPartial(BoardPost2[] data, ThreadPartLink link);
    }
}