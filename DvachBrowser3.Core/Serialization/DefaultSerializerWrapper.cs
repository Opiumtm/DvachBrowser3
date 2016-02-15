using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace DvachBrowser3
{
    /// <summary>
    /// Враппер для сериализации по умолчанию.
    /// </summary>
    public sealed class DefaultSerializerWrapper<T> : IObjectSerializer
    {
        private readonly DataContractSerializer serializer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DefaultSerializerWrapper()
        {
            serializer = new DataContractSerializer(typeof(T));
        }


        /// <summary>
        /// Сериализовать объект.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <param name="obj">Объект.</param>
        /// <returns>Таск.</returns>
        public Task WriteObjectAsync(IOutputStream str, object obj)
        {
            return Task.Factory.StartNew(() =>
            {
                serializer.WriteObject(str.AsStreamForWrite(), obj);
            });
        }

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Объект.</returns>
        public Task<object> ReadObjectAsync(IInputStream str)
        {
            return Task.Factory.StartNew(() => serializer.ReadObject(str.AsStreamForRead()));
        }

        /// <summary>
        /// Клонировать объект.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Клон объекта.</returns>
        public Task<object> DeepClone(object obj)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var str = new MemoryStream())
                {
                    serializer.WriteObject(str, obj);
                    str.Position = 0;
                    return serializer.ReadObject(str);
                }
            });
        }
    }
}