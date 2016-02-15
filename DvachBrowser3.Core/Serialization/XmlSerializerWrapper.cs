using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage.Streams;

namespace DvachBrowser3
{
    /// <summary>
    /// Враппер над XML-сериализатором.
    /// </summary>
    public sealed class XmlSerializerWrapper<T> : DataContractSerializerWrapperBase<T>
    {
        /// <summary>
        /// Создать средство записи.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство записи.</returns>
        protected override XmlWriter CreateWriter(IOutputStream str)
        {
            return XmlDictionaryWriter.CreateTextWriter(str.AsStreamForWrite(), Encoding.UTF8, false);
        }

        /// <summary>
        /// Создать средство чтения.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство чтения.</returns>
        protected override XmlReader CreateReader(IInputStream str)
        {
            return XmlDictionaryReader.CreateTextReader(str.AsStreamForRead(), XmlDictionaryReaderQuotas.Max);
        }

        /// <summary>
        /// Создать средство записи.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство записи.</returns>
        protected override XmlWriter CreateWriter(Stream str)
        {
            return XmlDictionaryWriter.CreateTextWriter(str, Encoding.UTF8, false);
        }

        /// <summary>
        /// Создать средство чтения.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Средство чтения.</returns>
        protected override XmlReader CreateReader(Stream str)
        {
            return XmlDictionaryReader.CreateTextReader(str, XmlDictionaryReaderQuotas.Max);
        }
    }
}