using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage.Streams;

namespace DvachBrowser3
{
    /// <summary>
    /// Враппер для сериализатора по контракту данных.
    /// </summary>
    public abstract class DataContractSerializerWrapperBase<T> : IObjectSerializer
    {
        private readonly DataContractSerializer serializer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected DataContractSerializerWrapperBase()
        {
            serializer = new DataContractSerializer(typeof(T));
        }

        /// <summary>
        /// Создать средство записи.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство записи.</returns>
        protected abstract XmlWriter CreateWriter(IOutputStream str);

        /// <summary>
        /// Создать средство чтения.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство чтения.</returns>
        protected abstract XmlReader CreateReader(IInputStream str);

        /// <summary>
        /// Создать средство записи.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство записи.</returns>
        protected abstract XmlWriter CreateWriter(Stream str);

        /// <summary>
        /// Создать средство чтения.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство чтения.</returns>
        protected abstract XmlReader CreateReader(Stream str);

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
                var wr = CreateWriter(str);
                serializer.WriteObject(wr, obj);
                wr.Flush();
            });
        }

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Объект.</returns>
        public Task<object> ReadObjectAsync(IInputStream str)
        {
            return Task.Factory.StartNew(() =>
            {
                var rd = CreateReader(str);
                return serializer.ReadObject(rd);
            });
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
                    var wr = CreateWriter(str);
                    serializer.WriteObject(wr, obj);
                    wr.Flush();
                    str.Seek(0, SeekOrigin.Begin);
                    var rd = CreateReader(str);
                    return serializer.ReadObject(rd);
                }
            });
        }
    }
}