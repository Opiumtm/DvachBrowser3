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
                    if (t == typeof (CustomDictionaryData))
                    {
                        serializers[t] = new DataContractSerializer(t, customDataTypes);
                    }
                    else
                    {
                        serializers[t] = new DataContractSerializer(t);
                    }
                }
                return serializers[t];
            }
        }

        /// <summary>
        /// Любые типы данных.
        /// </summary>
        private readonly HashSet<Type> customDataTypes = new HashSet<Type>();

        /// <summary>
        /// Зарегистрировать тип.
        /// </summary>
        /// <param name="type">Тип.</param>
        public void RegisterCustomDataType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            lock (serializers)
            {
                customDataTypes.Add(type);
                serializers.Remove(typeof (CustomDictionaryData));
            }
        }
    }
}