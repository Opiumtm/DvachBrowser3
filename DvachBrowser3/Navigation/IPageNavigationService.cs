namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Сервис навигации по страницам.
    /// </summary>
    public interface IPageNavigationService
    {
        /// <summary>
        /// Произвести навигацию.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="reportError">Показать ошибку.</param>
        void Navigate(PageNavigationTargetBase target, bool reportError = true);
    }
}