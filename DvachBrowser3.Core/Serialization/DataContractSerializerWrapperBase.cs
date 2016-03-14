using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage.Streams;
using Windows.System.Threading;

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
            return ThreadPool.RunAsync(a =>
            {
                var wr = CreateWriter(str);
                serializer.WriteObject(wr, obj);
                wr.Flush();
            }).AsTask();
        }

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Объект.</returns>
        public async Task<object> ReadObjectAsync(IInputStream str)
        {
            var result = new ObjectWrapper();
            await ThreadPool.RunAsync(a =>
            {
                var rd = CreateReader(str);
                result.Obj = serializer.ReadObject(rd);
            });
            return result.Obj;
        }

        private class ObjectWrapper
        {
            private object obj;

            public object Obj
            {
                get { return Interlocked.CompareExchange(ref obj, null, null); }
                set { Interlocked.Exchange(ref obj, value); }
            }
        }


        /// <summary>
        /// Клонировать объект.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Клон объекта.</returns>
        public async Task<object> DeepClone(object obj)
        {
            var result = new ObjectWrapper();
            await ThreadPool.RunAsync(a =>
            {
                using (var str = new MemoryStream())
                {
                    var wr = CreateWriter(str);
                    serializer.WriteObject(wr, obj);
                    wr.Flush();
                    str.Seek(0, SeekOrigin.Begin);
                    var rd = CreateReader(str);
                    result.Obj = serializer.ReadObject(rd);
                }
            });
            return result.Obj;
        }
    }
}