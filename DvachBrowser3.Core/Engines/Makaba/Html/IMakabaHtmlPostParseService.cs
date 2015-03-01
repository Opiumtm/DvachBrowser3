using System.Collections.Generic;
using DvachBrowser3.Posts;

namespace DvachBrowser3.Engines.Makaba.Html
{
    /// <summary>
    /// Сервис парсинга поста.
    /// </summary>
    public interface IMakabaHtmlPostParseService
    {
        /// <summary>
        /// Парсить html поста.
        /// </summary>
        /// <param name="comment">Текст поста.</param>
        /// <returns>Узлы дерева поста.</returns>
        ICollection<PostNodeBase> GetPostNodes(string comment);
    }
}