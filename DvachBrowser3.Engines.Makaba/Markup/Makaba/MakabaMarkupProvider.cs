using System.Collections.Generic;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Markup.Makaba
{
    /// <summary>
    /// Провайдер разметки Makaba.
    /// </summary>
    public sealed class MakabaMarkupProvider : IMarkupProvider
    {
        /// <summary>
        /// Тип разметки.
        /// </summary>
        public PostingMarkupType MarkupType => PostingMarkupType.Makaba;

        /// <summary>
        /// Поддерживаемые тэги.
        /// </summary>
        public ISet<MarkupTag> SupportedTags => new HashSet<MarkupTag>(ProvidedTags.Keys);

        private static readonly Dictionary<MarkupTag, MarkupTagData> ProvidedTags = new Dictionary<MarkupTag, MarkupTagData>()
        {
            { MarkupTag.Italic, new MarkupTagData() { Begin = "[i]", End = "[/i]"} },
            { MarkupTag.Bold, new MarkupTagData() { Begin = "[b]", End = "[/b]"} },
            { MarkupTag.Monospace, new MarkupTagData() { Begin = "[code]", End = "[/code]"} },
            { MarkupTag.Underline, new MarkupTagData() { Begin = "[u]", End = "[/u]"} },
            { MarkupTag.Overline, new MarkupTagData() { Begin = "[o]", End = "[/o]"} },
            { MarkupTag.Spoiler, new MarkupTagData() { Begin = "[spoiler]", End = "[/spoiler]"} },
            { MarkupTag.Strikeout, new MarkupTagData() { Begin = "[s]", End = "[/s]"} },
            { MarkupTag.Sup, new MarkupTagData() { Begin = "[sup]", End = "[/sup]"} },
            { MarkupTag.Sub, new MarkupTagData() { Begin = "[sub]", End = "[/sub]"} },
        };

        private struct MarkupTagData
        {
            public string Begin;
            public string End;
        }

        /// <summary>
        /// Установить разметку.
        /// </summary>
        /// <param name="src">Исходная строка.</param>
        /// <param name="tag">Тэг.</param>
        /// <param name="startIndex">Начальный индекс.</param>
        /// <param name="length">Длина.</param>
        /// <returns></returns>
        public string SetMarkup(string src, MarkupTag tag, int startIndex, int length)
        {
            if (src == null || !ProvidedTags.ContainsKey(tag))
            {
                return src;
            }
            var mt = ProvidedTags[tag];
            return src.Replace("\r\n", "\n").Insert(startIndex, mt.Begin).Insert(startIndex + mt.Begin.Length + length, mt.End).Replace("\n", "\r\n");
        }
    }
}