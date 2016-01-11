using System.Text;
using DvachBrowser3.TextRender;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Помощник получения ключа.
    /// </summary>
    public static class TextRenderCommandKeyHelper
    {
        /// <summary>
        /// Получение ключа кэширования.
        /// </summary>
        /// <param name="command">Команда.</param>
        /// <param name="isNarrow">Узкое представление.</param>
        /// <returns></returns>
        public static string GetCacheKey(this ITextRenderCommand command, bool isNarrow)
        {
            var sb = new StringBuilder();
            var isnk = isNarrow ? "1" : "0";
            sb.Append($"SK{isnk}:[");
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Bold))
            {
                sb.Append("b");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Italic))
            {
                sb.Append("i");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Fixed))
            {
                sb.Append("f");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Spoiler))
            {
                sb.Append("s");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Quote))
            {
                sb.Append("q");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Link))
            {
                sb.Append("l");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Undeline))
            {
                sb.Append("_");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Overline))
            {
                sb.Append("^");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Strikethrough))
            {
                sb.Append("-");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Subscript))
            {
                sb.Append(".");
            }
            if (command.Attributes.Attributes.ContainsKey(CommonTextRenderAttributes.Superscript))
            {
                sb.Append("`");
            }
            sb.Append("]:");

            string text;
            var textCnt = command.Content as ITextRenderTextContent;
            if (textCnt != null)
            {
                text = textCnt.Text ?? "";
            }
            else
            {
                text = "";
            }
            var sha = UniqueIdHelper.CreateIdString(text);
            sb.Append(sha);
            return sb.ToString();
        }
    }
}