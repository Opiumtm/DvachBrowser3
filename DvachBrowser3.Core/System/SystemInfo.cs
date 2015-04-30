using System;

namespace DvachBrowser3.SystemInformation
{
    /// <summary>
    /// Информация о системе.
    /// </summary>
    public sealed class SystemInfo : ServiceBase, ISystemInfo
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <param name="param">Параметры системы.</param>
        public SystemInfo(IServiceProvider services, SystemInfoParam param) : base(services)
        {
            Platform = param.Platform;
            AppIcon = param.AppIcon;
            SmallAppIcon = param.SmallAppIcon;
        }

        /// <summary>
        /// Платформа.
        /// </summary>
        public AppPlatform Platform { get; private set; }

        /// <summary>
        /// Иконка приложения.
        /// </summary>
        public string AppIcon { get; private set; }

        /// <summary>
        /// Маленькая иконка приложения.
        /// </summary>
        public string SmallAppIcon { get; private set; }
    }
}