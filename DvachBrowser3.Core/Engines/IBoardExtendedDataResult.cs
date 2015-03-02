using System.Collections.Generic;
using DvachBrowser3.Board;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Дополнительная информация о борде.
    /// </summary>
    public interface IBoardExtendedDataResult
    {
        /// <summary>
        /// Расширения.
        /// </summary>
        ICollection<BoardReferenceExtension> Extensions { get; }
    }
}