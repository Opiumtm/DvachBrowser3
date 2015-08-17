using System.Collections.Generic;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Средство разбивки на слова.
    /// </summary>
    public interface IWordSplitter
    {
        /// <summary>
        /// Разбить на слова.
        /// </summary>
        /// <param name="src">Исходная строка.</param>
        /// <returns>Разбитая на слова строка.</returns>
        IEnumerable<string> Split(string src);
    }
}