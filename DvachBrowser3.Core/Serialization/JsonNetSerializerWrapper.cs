using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace DvachBrowser3
{
    /// <summary>
    /// Враппер для сериализации JSON.NET
    /// </summary>
    public sealed class JsonNetSerializerWrapper<T> : IObjectSerializer
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            TypeNameHandling = TypeNameHandling.All,
            DateParseHandling = DateParseHandling.DateTime,
            DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            FloatFormatHandling = FloatFormatHandling.String,
            FloatParseHandling = FloatParseHandling.Double,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            StringEscapeHandling = StringEscapeHandling.Default,
        };

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
                using (var str2 = str.AsStreamForWrite())
                {
                    using (var wr = new StreamWriter(str2, Encoding.UTF8))
                    {
                        using (var json = new JsonTextWriter(wr))
                        {
                            var serializer = JsonSerializer.Create(settings);
                            serializer.Serialize(json, obj);
                        }
                    }
                }
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
                using (var str2 = str.AsStreamForRead())
                {
                    using (var rd = new StreamReader(str2, Encoding.UTF8))
                    {
                        using (var json = new JsonTextReader(rd))
                        {
                            var serialier = JsonSerializer.Create(settings);
                            return serialier.Deserialize(json);
                        }
                    }
                }
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
                    var serialier = JsonSerializer.Create(settings);
                    var wr = new StreamWriter(str, Encoding.UTF8);
                    var jsonWr = new JsonTextWriter(wr);
                    serialier.Serialize(jsonWr, obj);
                    jsonWr.Flush();
                    wr.Flush();
                    str.Position = 0;
                    var rd = new StreamReader(str);
                    var jsonRd = new JsonTextReader(rd);
                    return serialier.Deserialize(jsonRd);
                }
            });
        }
    }
}