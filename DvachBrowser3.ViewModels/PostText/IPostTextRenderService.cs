using Windows.UI.Xaml.Controls;
using DvachBrowser3.Posts;
using DvachBrowser3.Styles;

namespace DvachBrowser3.PostText
{
    /// <summary>
    /// Сервис рендеринга текста.
    /// </summary>
    public interface IPostTextRenderService
    {
        /// <summary>
        /// Рендеринг.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <param name="target">Куда рендерить.</param>
        /// <param name="styles">Стили.</param>
        void Render(PostTree post, RichTextBlock target, IStyleManager styles);
    }
}