using System;
using System.IO;
using System.Net;
using System.Text;
using Windows.Storage.Streams;
using Newtonsoft.Json;

namespace DvachBrowser3.Common
{
    /// <summary>
    /// Сервис десериализации JSON.
    /// </summary>
    public sealed class JsonService : ServiceBase, IJsonService
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public JsonService(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// Десериализовать из формата JSON.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="str">Строка.</param>
        /// <returns>Объект.</returns>
        public T Deserialize<T>(string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (JsonException)
            {
                throw new WebException("Неправильный формат ответа сервера");
            }
        }

        /// <summary>
        /// Десериализовать из формата JSON.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="str">Входной поток.</param>
        /// <param name="encoding">Кодировка.</param>
        /// <returns>Объект.</returns>
        public T Deserialize<T>(Stream str, Encoding encoding)
        {
            try
            {
                var serializer = new JsonSerializer();
                var rd = new StreamReader(str, encoding);
                var json = new JsonTextReader(rd);
                return serializer.Deserialize<T>(json);
            }
            catch (JsonException)
            {
                throw new WebException("Неправильный формат ответа сервера");
            }
        }
    }
}