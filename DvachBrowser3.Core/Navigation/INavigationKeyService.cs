namespace DvachBrowser3.Navigation
{
    /// <summary>
    /// Сервис ключей навигации.
    /// </summary>
    public interface INavigationKeyService
    {
        /// <summary>
        /// Сериализовать в строку.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Строка.</returns>
        string Serialize(INavigationKey key);

        /// <summary>
        /// Десериализовать.
        /// </summary>
        /// <param name="data">Строка.</param>
        /// <returns>Ключ.</returns>
        INavigationKey Deserialize(string data);

        /// <summary>
        /// Фабрика ключей навигации.
        /// </summary>
        /// <param name="factory">Фабрика.</param>
        void AddNavigationKeyFactory(INavigationKeyFactory factory);
    }
}