using DvachBrowser3.Posts;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Результат запроса к странице борды.
    /// </summary>
    public interface IBoardPageResult
    {
        /// <summary>
        /// Результат.
        /// </summary>
        BoardPageTree Result { get; } 
    }
}