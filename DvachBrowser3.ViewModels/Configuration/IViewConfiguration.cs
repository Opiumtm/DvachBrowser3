namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Конфигурация отображения.
    /// </summary>
    public interface IViewConfiguration : IConfiguration
    {
        /// <summary>
        /// Отображать даты в формате борды.
        /// </summary>
        bool BoardNativeDates { get; set; }
    }
}