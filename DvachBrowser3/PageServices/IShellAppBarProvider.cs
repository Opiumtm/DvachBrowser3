using Windows.UI.Xaml.Controls;

namespace DvachBrowser3.PageServices
{
    /// <summary>
    /// Источник строки команд.
    /// </summary>
    public interface IShellAppBarProvider
    {
        /// <summary>
        /// Получить нижнюю строку команд.
        /// </summary>
        /// <returns>Строка команд.</returns>
        AppBar GetBottomAppBar();
    }
}