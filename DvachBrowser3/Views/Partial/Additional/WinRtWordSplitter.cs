using System.Collections.Generic;
using DvachBrowser3.TextRender;
using System.Linq;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Разбивка на слова с помощью встроенных средств.
    /// </summary>
    public sealed class WinRtWordSplitter : IWordSplitter
    {
        /// <summary>
        /// Разбить на слова.
        /// </summary>
        /// <param name="src">Исходная строка.</param>
        /// <returns>Разбитая на слова строка.</returns>
        public IEnumerable<string> Split(string src)
        {
            var ws = new Windows.Data.Text.WordsSegmenter("ru-RU");
            var tokens = ws.GetTokens(src ?? "");
            foreach (var token in tokens)
            {
                yield return token.Text;
            }
        }
    }
}