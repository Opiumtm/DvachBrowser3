using System;
using Windows.UI.Xaml.Controls;
using DvachBrowser3.Posts;
using DvachBrowser3.Styles;

namespace DvachBrowser3.PostText
{
    /// <summary>
    /// Сервис рендеринга текста.
    /// </summary>
    public sealed class PostTextRenderService : ServiceBase, IPostTextRenderService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public PostTextRenderService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Рендеринг.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <param name="target">Куда рендерить.</param>
        /// <param name="styles">Стили.</param>
        public void Render(PostTree post, RichTextBlock target, IStyleManager styles)
        {
            target.Blocks.Clear();
        }
    }
}