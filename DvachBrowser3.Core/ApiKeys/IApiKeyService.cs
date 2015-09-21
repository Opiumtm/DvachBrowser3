namespace DvachBrowser3.ApiKeys
{
    /// <summary>
    /// Сервис ключей API.
    /// </summary>
    public interface IApiKeyService
    {
        /// <summary>
        /// Найти контейнер.
        /// </summary>
        /// <param name="name">Имя.</param>
        /// <returns>Контейнер.</returns>
        IApiKeyContainer Find(string name);

        /// <summary>
        /// Добавить.
        /// </summary>
        /// <param name="name">Имя.</param>
        /// <param name="container">Контейнер.</param>
        void Add(string name, IApiKeyContainer container);
    }
}