namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Запрос на поиск постов в коллекции.
    /// </summary>
    public interface IPostCollectionSearchQuery
    {
        /// <summary>
        /// Фильтровать пост.
        /// </summary>
        /// <param name="post">Пост.</param>
        /// <returns>Результат фильтрации.</returns>
        bool Filter(IPostViewModel post);        
    }
}