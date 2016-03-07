namespace DvachBrowser3.Styles
{
    /// <summary>
    /// Фабрика менеджера стилей.
    /// </summary>
    public sealed class StyleManagerFactory : IStyleManagerFactory
    {
        /// <summary>
        /// Экземпляр фабрики.
        /// </summary>
        public static readonly IStyleManagerFactory Current = new StyleManagerFactory();

        /// <summary>
        /// Получить менеджер.
        /// </summary>
        /// <returns></returns>
        public IStyleManager GetManager()
        {
            return new StyleManager();
        }
    }
}