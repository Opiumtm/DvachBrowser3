namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Страница с ролью навигации.
    /// </summary>
    public interface INavigationRolePage
    {
        /// <summary>
        /// Получить роль навигации.
        /// </summary>
        NavigationRole? NavigationRole { get; } 
    }
}