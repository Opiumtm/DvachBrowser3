namespace DvachBrowser3.Engines.Makaba
{
    /// <summary>
    /// Конфигурация постинга.
    /// </summary>
    public interface IMakabaPostConfig : IConfiguration
    {
        /// <summary>
        /// Смайл-разметка.
        /// </summary>
        IMakabaSmileConfig SmileConfig { get; }

        /// <summary>
        /// Корректировать разметку.
        /// </summary>
        bool CorrectWakaba { get; set; }

        /// <summary>
        /// Использовать смайл-разметку.
        /// </summary>
        bool UseSmileMarkup { get; set; }
    }
}