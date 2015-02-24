using System.Collections.Generic;
using DvachBrowser3.Board;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Результат списка борд.
    /// </summary>
    public interface IBoardListResult
    {
        /// <summary>
        /// Список борд.
        /// </summary>
        List<BoardReference> Boards { get; }
    }
}