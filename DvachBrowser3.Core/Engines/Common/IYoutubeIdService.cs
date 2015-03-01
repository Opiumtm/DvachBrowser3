namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Сервис получения ID ютубы.
    /// </summary>
    public interface IYoutubeIdService
    {
        /// <summary>
        /// Получить идентификатор ютубы.
        /// </summary>
        /// <param name="uri">URI.</param>
        /// <returns>Идентификатор.</returns>
        string GetYoutubeIdFromUri(string uri);         
    }
}