using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Windows.Storage.Streams;

namespace DvachBrowser3
{
    /// <summary>
    /// ������� �������� ������������.
    /// </summary>
    public sealed class BinarySerializerWrapper<T> : DataContractSerializerWrapperBase<T>
    {
        /// <summary>
        /// ������� �������� ������.
        /// </summary>
        /// <param name="str">�����.</param>
        /// <returns>�������� ������.</returns>
        protected override XmlWriter CreateWriter(IOutputStream str)
        {
            return XmlDictionaryWriter.CreateBinaryWriter(str.AsStreamForWrite(64*1024));
        }

        /// <summary>
        /// ������� �������� ������.
        /// </summary>
        /// <param name="str">�����.</param>
        /// <returns>�������� ������.</returns>
        protected override XmlReader CreateReader(IInputStream str)
        {
            return XmlDictionaryReader.CreateBinaryReader(str.AsStreamForRead(64*1024), XmlDictionaryReaderQuotas.Max);
        }

        /// <summary>
        /// ������� �������� ������.
        /// </summary>
        /// <param name="str">�����.</param>
        /// <returns>�������� ������.</returns>
        protected override XmlWriter CreateWriter(Stream str)
        {
            return XmlDictionaryWriter.CreateBinaryWriter(str);
        }

        /// <summary>
        /// ������� �������� ������.
        /// </summary>
        /// <param name="str">�����.</param>
        /// <returns>�������� ������.</returns>
        protected override XmlReader CreateReader(Stream str)
        {
            return XmlDictionaryReader.CreateBinaryReader(str, XmlDictionaryReaderQuotas.Max);
        }
    }
}