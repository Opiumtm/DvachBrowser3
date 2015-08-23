using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace DvachBrowser3.Lifecycle
{
    /// <summary>
    /// Класс-помощник навигации.
    /// </summary>
    public interface INavigationHelper
    {
        /// <summary>
        /// Команда навигации назад.
        /// </summary>
        ICommand GoBackCommand { get; }

        /// <summary>
        /// Команда навигации вперёд.
        /// </summary>
        ICommand GoForwardCommand { get; }

        /// <summary>
        /// Возможность навигации назад.
        /// </summary>
        /// <returns>Результат.</returns>
        bool CanGoBack();

        /// <summary>
        /// Возможность навигации вперёд.
        /// </summary>
        /// <returns>Результат.</returns>
        bool CanGoForward();

        /// <summary>
        /// Навигация назад.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Навигация вперёд.
        /// </summary>
        void GoForward();

        /// <summary>
        /// Загрузка состояния.
        /// </summary>
        event LoadStateEventHandler LoadState;

        /// <summary>
        /// Сохранение состояния.
        /// </summary>
        event SaveStateEventHandler SaveState;

        /// <summary>
        /// Навигация на страницу.
        /// </summary>
        /// <param name="e">Событие.</param>
        void OnNavigatedTo(NavigationEventArgs e);

        /// <summary>
        /// Навигация со страницы.
        /// </summary>
        /// <param name="e">Событие.</param>
        void OnNavigatedFrom(NavigationEventArgs e);
    }
}