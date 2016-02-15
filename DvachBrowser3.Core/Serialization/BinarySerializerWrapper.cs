using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Windows.Storage.Streams;

namespace DvachBrowser3
{
    /// <summary>
    /// Враппер бинарной сериализации.
    /// </summary>
    public sealed class BinarySerializerWrapper<T> : DataContractSerializerWrapperBase<T>
    {
        /// <summary>
        /// Создать средство записи.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство записи.</returns>
        protected override XmlWriter CreateWriter(IOutputStream str)
        {
            return XmlDictionaryWriter.CreateBinaryWriter(str.AsStreamForWrite());
        }

        /// <summary>
        /// Создать средство чтения.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство чтения.</returns>
        protected override XmlReader CreateReader(IInputStream str)
        {
            return XmlDictionaryReader.CreateBinaryReader(str.AsStreamForRead(), XmlDictionaryReaderQuotas.Max);
        }

        /// <summary>
        /// Создать средство записи.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство записи.</returns>
        protected override XmlWriter CreateWriter(Stream str)
        {
            return XmlDictionaryWriter.CreateBinaryWriter(str);
        }

        /// <summary>
        /// Создать средство чтения.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство чтения.</returns>
        protected override XmlReader CreateReader(Stream str)
        {
            return XmlDictionaryReader.CreateBinaryReader(str, XmlDictionaryReaderQuotas.Max);
        }
    }
}