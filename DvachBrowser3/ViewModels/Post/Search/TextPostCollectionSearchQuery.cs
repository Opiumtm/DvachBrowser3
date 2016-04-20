using System;
using System.Linq;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Поиск по тексту поста.
    /// </summary>
    public sealed class TextPostCollectionSearchQuery : IPostCollectionSearchQuery
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="text">Текст.</param>
        public TextPostCollectionSearchQuery(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Текст.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Результат фильтрации.</returns>
        public bool Filter(IPostViewModel post)
        {
            var postText = post?.Text?.GetPlainText();
            if (postText == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(Text))
            {
                return true;
            }
            var text = Text.Trim();
            if (text.StartsWith("/") && text.EndsWith("/") && text.Length > 2)
            {
                text = text.Remove(0, 1);
                text = text.Remove(text.Length - 1, 1);
                return post.Tags.Tags.Any(t => text.Equals(t, StringComparison.CurrentCultureIgnoreCase));
            }
            return 
                (post.Subject ?? "").IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                postText.Any(t => t.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                post.Tags.Tags.Any(t => text.Equals(t, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}