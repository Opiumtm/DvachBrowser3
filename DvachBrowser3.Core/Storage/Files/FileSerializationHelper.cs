using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage.Streams;

namespace DvachBrowser3.Storage.Files
{
    /// <summary>
    /// Класс-помощник сериализации.
    /// </summary>
    public static class FileSerializationHelper
    {
        /// <summary>
        /// Сохранить объект асинхронно.
        /// </summary>
        /// <param name="serializer">Сериализатор.</param>
        /// <param name="wr">Средство записи XML.</param>
        /// <param name="graph">Объект.</param>
        /// <returns>Таск.</returns>
        public static Task WriteObjectAsync(this DataContractSerializer serializer, XmlWriter wr, object graph)
        {
            return Task.Factory.StartNew(() => serializer.WriteObject(wr, graph));
        }

        /// <summary>
        /// Прочитать объект асинхронно.
        /// </summary>
        /// <param name="serializer">Сериализатор.</param>
        /// <param name="rd">Средство чтения XML.</param>
        /// <returns>Таск.</returns>
        public static Task<object> ReadObjectAsync(this DataContractSerializer serializer, XmlReader rd)
        {
            return Task.Factory.StartNew(() => serializer.ReadObject(rd));
        }
    }
}