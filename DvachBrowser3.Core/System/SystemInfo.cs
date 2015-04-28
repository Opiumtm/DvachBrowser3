using System;

namespace DvachBrowser3.System
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
        }

        /// <summary>
        /// Платформа.
        /// </summary>
        public AppPlatform Platform { get; private set; }
    }
}