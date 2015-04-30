using System.IO;
using System.Text;
using Windows.Storage.Streams;

namespace DvachBrowser3.Common
{
    /// <summary>
    /// Сервис десериализации JSON.
    /// </summary>
    public interface IJsonService
    {
        /// <summary>
        /// Десериализовать из формата JSON.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="str">Строка.</param>
        /// <returns>Объект.</returns>
        T Deserialize<T>(string str);

        /// <summary>
        /// Десериализовать из формата JSON.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="str">Входной поток.</param>
        /// <param name="encoding">Кодировка.</param>
        /// <returns>Объект.</returns>
        T Deserialize<T>(Stream str, Encoding encoding);
    }
}