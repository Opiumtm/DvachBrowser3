using DvachBrowser3.Links;

namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Модель представления квоты.
    /// </summary>
    public interface IQuoteViewModel
    {
        /// <summary>
        /// Пост.
        /// </summary>
        IPostViewModel Parent { get; }

        /// <summary>
        /// Ссылка.
        /// </summary>
        BoardLinkBase Link { get; }

        /// <summary>
        /// Заголовок ссылки.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Навигация к квоте.
        /// </summary>
        void NavigateTo();
    }
}