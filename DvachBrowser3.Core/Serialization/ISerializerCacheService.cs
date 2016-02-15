using System;
using System.Runtime.Serialization;

namespace DvachBrowser3
{
    /// <summary>
    /// Сервис кэша сериализаторов.
    /// </summary>
    public interface ISerializerCacheService
    {
        /// <summary>
        /// Получить сериализатор.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <returns>Сериализатор.</returns>
        IObjectSerializer GetSerializer<T>();
    }
}