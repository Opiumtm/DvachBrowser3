using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Класс-помощник для сервисов.
    /// </summary>
    public static class ServiceHelper
    {
        /// <summary>
        /// Получить сервис.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <param name="src">Провайдер сервисов.</param>
        /// <returns>Сервис.</returns>
        public static T GetService<T>(this IServiceProvider src) where T : class
        {
            return src.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Получить сервис или выбросить исключение.
        /// </summary>
        /// <typeparam name="T">Тип сервиса.</typeparam>
        /// <param name="src">Провайдер сервисов.</param>
        /// <returns>Сервис.</returns>
        public static T GetServiceOrThrow<T>(this IServiceProvider src) where T : class
        {
            var result = src.GetService<T>();
            if (result == null)
            {
                throw new NotImplementedException(string.Format("Type {0} is not supported by service provider", typeof(T).FullName));
            }
            return result;
        }
    }
}