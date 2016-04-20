namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Фабрика менеджера стилей.
    /// </summary>
    public interface IStyleManagerFactory
    {
        /// <summary>
        /// Получить менеджер.
        /// </summary>
        /// <returns></returns>
        IStyleManager GetManager();
    }
}