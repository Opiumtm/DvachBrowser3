using System.Collections.Generic;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Markup
{
    /// <summary>
    /// Провайдер разметки.
    /// </summary>
    public interface IMarkupProvider
    {
        /// <summary>
        /// Тип разметки.
        /// </summary>
        PostingMarkupType MarkupType { get; }

        /// <summary>
        /// Поддерживаемые тэги.
        /// </summary>
        ISet<MarkupTag> SupportedTags { get; }

        /// <summary>
        /// Установить разметку.
        /// </summary>
        /// <param name="src">Исходная строка.</param>
        /// <param name="tag">Тэг.</param>
        /// <param name="startIndex">Начальный индекс.</param>
        /// <param name="length">Длина.</param>
        /// <returns></returns>
        string SetMarkup(string src, MarkupTag tag, int startIndex, int length);
    }
}