using System.Collections.Generic;
using System.Text;

namespace DvachBrowser3.TextRender
{
    /// <summary>
    /// Средство разбивки на слова.
    /// </summary>
    public sealed class WordSplitter : IWordSplitter
    {
        /// <summary>
        /// Разбить на слова.
        /// </summary>
        /// <param name="src">Исходная строка.</param>
        /// <returns>Разбитая на слова строка.</returns>
        public IEnumerable<string> Split(string src)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < src.Length; i++)
            {
                var ch = src[i];
                if (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n')
                {
                    sb.Append(' ');
                    yield return sb.ToString();
                    sb.Clear();
                }
                else
                {
                    sb.Append(ch);
                    //if (ch == '.' || ch == ',' || ch == ':' || ch == '/' || ch == '\\' || ch == '#' || ch =='?' || ch == '!' || ch == '*' || ch == '-')
                    if (char.IsPunctuation(ch) || ch == '.' || ch == ',' || ch == ':' || ch == '/' || ch == '\\' || ch == '#' || ch == '?' || ch == '!' || ch == '*' || ch == '-')
                    {
                        yield return sb.ToString();
                        sb.Clear();
                    }
                }
            }
            if (sb.Length > 0)
            {
                yield return sb.ToString();
            }
        }
    }
}