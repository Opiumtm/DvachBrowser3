using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3
{
    /// <summary>
    /// Сервис кэша сериализаторов.
    /// </summary>
    public sealed class SerializerCacheService : ServiceBase, ISerializerCacheService
    {
        private readonly Dictionary<Type, IObjectSerializer> serializers = new Dictionary<Type, IObjectSerializer>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public SerializerCacheService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Получить сериализатор.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <returns>Сериализатор.</returns>
        public IObjectSerializer GetSerializer<T>()
        {
            lock (serializers)
            {
                var t = typeof (T);
                if (!serializers.ContainsKey(t))
                {
                    serializers[t] = new BinarySerializerWrapper<T>();
                }
                return serializers[t];
            }
        }
    }
}