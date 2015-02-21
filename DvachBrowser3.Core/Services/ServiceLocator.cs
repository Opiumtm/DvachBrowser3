using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Локатор сервисов (текущий набор сервисов, инициализируется в основном приложении).
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// Текущий экземпляр.
        /// </summary>
        public static IServiceProvider Current { get; set; }
    }
}