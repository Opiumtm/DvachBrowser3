using DvachBrowser3.ViewModels;

namespace DvachBrowser3.Views.Partial
{
    /// <summary>
    /// Хранилище индекса визуального показа коллекции постов.
    /// </summary>
    public interface IPostCollectionVisualIndexStore
    {
        /// <summary>
        /// Получить индекс видимого элемента.
        /// </summary>
        /// <returns>Индекс.</returns>
        IPostViewModel GetTopViewIndex();

        /// <summary>
        /// Показать видимый элемент.
        /// </summary>
        /// <param name="post">Пост.</param>
        void ScrollIntoView(IPostViewModel post);
    }
}