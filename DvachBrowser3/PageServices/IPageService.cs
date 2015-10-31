using Windows.UI.Xaml.Controls;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Сервис страницы.
    /// </summary>
    public interface IPageService
    {
        /// <summary>
        /// Прикрепиться к странице.
        /// </summary>
        /// <param name="page">Страница.</param>
        void Attach(Page page);
    }
}