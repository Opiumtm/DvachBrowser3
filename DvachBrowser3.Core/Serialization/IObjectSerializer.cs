using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace DvachBrowser3
{
    /// <summary>
    /// Сериализатор объектов.
    /// </summary>
    public interface IObjectSerializer
    {
        /// <summary>
        /// Сериализовать объект.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <param name="obj">Объект.</param>
        /// <returns>Таск.</returns>
        Task WriteObjectAsync(IOutputStream str, object obj);

        /// <summary>
        /// Десериализовать объект.
        /// </summary>
        /// <param name="str">Поток.</param>
        /// <returns>Объект.</returns>
        Task<object> ReadObjectAsync(IInputStream str);

        /// <summary>
        /// Клонировать объект.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Клон объекта.</returns>
        Task<object> DeepClone(object obj);
    }
}