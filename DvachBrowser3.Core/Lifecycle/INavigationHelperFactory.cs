using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DvachBrowser3.Lifecycle
{
    /// <summary>
    /// Фабрика создания помощников навигации.
    /// </summary>
    public interface INavigationHelperFactory
    {
        /// <summary>
        /// Создать.
        /// </summary>
        /// <param name="page">Страница.</param>
        /// <returns>Помощник навигации.</returns>
        INavigationHelper Create(Page page);

        /// <summary>
        /// Инициализировать.
        /// </summary>
        /// <param name="frame">Фрейм.</param>
        void Initialize(Frame frame);
    }
}