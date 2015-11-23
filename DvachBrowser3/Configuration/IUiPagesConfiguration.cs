namespace DvachBrowser3.Configuration
{
    /// <summary>
    /// Конфигурация страниц UI.
    /// </summary>
    public interface IUiPagesConfiguration : IConfiguration
    {
        /// <summary>
        /// Показывать баннеры.
        /// </summary>
        bool ShowBanners { get; set; }

        /// <summary>
        /// Дата, специфичная для борды.
        /// </summary>
        bool BoardSpecificDate { get; set; }
    }
}