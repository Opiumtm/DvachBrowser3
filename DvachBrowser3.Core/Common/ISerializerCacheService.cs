﻿using System;
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
        DataContractSerializer GetSerializer<T>();

        /// <summary>
        /// Зарегистрировать тип.
        /// </summary>
        /// <param name="type">Тип.</param>
        void RegisterCustomDataType(Type type);
    }
}