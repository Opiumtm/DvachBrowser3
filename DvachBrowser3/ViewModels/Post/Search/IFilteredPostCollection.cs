namespace DvachBrowser3.ViewModels
{
    /// <summary>
    /// Фильтрованная коллекция постов.
    /// </summary>
    public interface IFilteredPostCollection : IPostCollectionViewModel
    {
        /// <summary>
        /// Исходная коллекция.
        /// </summary>
        IPostCollectionViewModel Source { get; }         

        /// <summary>
        /// Запрос.
        /// </summary>
        IPostCollectionSearchQuery Query { get; }
    }
}