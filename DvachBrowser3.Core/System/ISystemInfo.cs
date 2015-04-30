namespace DvachBrowser3.SystemInformation
{
    /// <summary>
    /// Информация о системе.
    /// </summary>
    public interface ISystemInfo
    {
        /// <summary>
        /// Платформа.
        /// </summary>
        AppPlatform Platform { get; }

        /// <summary>
        /// Иконка приложения.
        /// </summary>
        string AppIcon { get; }

        /// <summary>
        /// Маленькая иконка приложения.
        /// </summary>
        string SmallAppIcon { get; }
    }
}