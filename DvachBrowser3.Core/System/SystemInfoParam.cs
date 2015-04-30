namespace DvachBrowser3.SystemInformation
{
    /// <summary>
    /// Параметр инициализации системной информации.
    /// </summary>
    public struct SystemInfoParam
    {
        /// <summary>
        /// Платформа.
        /// </summary>
        public AppPlatform Platform;

        /// <summary>
        /// Иконка приложения.
        /// </summary>
        public string AppIcon;

        /// <summary>
        /// Маленькая иконка приложения.
        /// </summary>
        public string SmallAppIcon;
    }
}