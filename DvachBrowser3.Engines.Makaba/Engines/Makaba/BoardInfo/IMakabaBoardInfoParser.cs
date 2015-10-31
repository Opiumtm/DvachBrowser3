using DvachBrowser3.Board;
using DvachBrowser3.Engines.Makaba.Json;

namespace DvachBrowser3.Engines.Makaba.BoardInfo
{
    /// <summary>
    /// Парсер информации о борде.
    /// </summary>
    public interface IMakabaBoardInfoParser
    {
        /// <summary>
        /// Парсить информацию о борде.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <param name="b">Информация.</param>
        /// <returns>Ссылка на борду.</returns>
        BoardReference Parse(string category, MobileBoardInfo b);

        /// <summary>
        /// Парсить информацию о борде.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <param name="boardId">Идентификатор борды.</param>
        /// <returns>Ссылка на борду.</returns>
        BoardReference Default(string category, string boardId);
    }
}