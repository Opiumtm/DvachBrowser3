using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DvachBrowser3
{
    /// <summary>
    /// Сервис кэша сериализаторов.
    /// </summary>
    public class SerializerCacheService : ServiceBase, ISerializerCacheService
    {
        private readonly Dictionary<Type, DataContractSerializer> serializers = new Dictionary<Type, DataContractSerializer>();

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
        public DataContractSerializer GetSerializer<T>()
        {
            lock (serializers)
            {
                var t = typeof (T);
                if (!serializers.ContainsKey(t))
                {
                    serializers[t] = new DataContractSerializer(t);
                }
                return serializers[t];
            }
        }
    }
}