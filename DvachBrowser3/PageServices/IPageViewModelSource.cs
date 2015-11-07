namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Источник модели представления.
    /// </summary>
    public interface IPageViewModelSource
    {
        /// <summary>
        /// Получить модель представления.
        /// </summary>
        /// <returns>Модель представления.</returns>
        object GetViewModel();
    }
}