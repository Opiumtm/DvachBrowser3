namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Сервис коррекции постов.
    /// </summary>
    public interface IPostCorrectionService
    {
        /// <summary>
        /// Конфигурация.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Корректировать текст.
        /// </summary>
        /// <param name="text">Текст.</param>
        /// <returns>Результат.</returns>
        string CorrectPostText(string text);
    }
}