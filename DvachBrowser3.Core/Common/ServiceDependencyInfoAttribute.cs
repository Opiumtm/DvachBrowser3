using System;

namespace DvachBrowser3
{
    /// <summary>
    /// Информационный атрибут зависимости от сервиса. Нужен только для пометки участков кода, зависящих от сервисов для удобства перехода к указанному типу.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ServiceDependencyInfoAttribute : Attribute
    {
        /// <summary>
        /// Тип сервиса.
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="serviceType">Тип сервиса.</param>
        public ServiceDependencyInfoAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}