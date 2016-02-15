using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace DvachBrowser3
{
    /// <summary>
    /// ������� ��� ������������ JSON.NET � ������� BSON
    /// </summary>
    public sealed class BsonNetSerializerWrapper<T> : IObjectSerializer
    {
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings()
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
        /// ������������� ������.
        /// </summary>
        /// <param name="str">�����.</param>
        /// <param name="obj">������.</param>
        /// <returns>����.</returns>
        public Task WriteObjectAsync(IOutputStream str, object obj)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var str2 = str.AsStreamForWrite())
                {
                    using (var wr = new BinaryWriter(str2, Encoding.UTF8))
                    {
                        using (var json = new BsonWriter(wr))
                        {
                            var serializer = JsonSerializer.Create(settings);
                            serializer.Serialize(json, obj);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// ��������������� ������.
        /// </summary>
        /// <param name="str">�����.</param>
        /// <returns>������.</returns>
        public Task<object> ReadObjectAsync(IInputStream str)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var str2 = str.AsStreamForRead())
                {
                    using (var rd = new BinaryReader(str2, Encoding.UTF8))
                    {
                        using (var json = new BsonReader(rd))
                        {
                            var serialier = JsonSerializer.Create(settings);
                            return serialier.Deserialize(json);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// ����������� ������.
        /// </summary>
        /// <param name="obj">������.</param>
        /// <returns>���� �������.</returns>
        public Task<object> DeepClone(object obj)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var str = new MemoryStream())
                {
                    var serialier = JsonSerializer.Create(settings);
                    var wr = new BinaryWriter(str, Encoding.UTF8);
                    var jsonWr = new BsonWriter(wr);
                    serialier.Serialize(jsonWr, obj);
                    jsonWr.Flush();
                    wr.Flush();
                    str.Position = 0;
                    var rd = new BinaryReader(str);
                    var jsonRd = new BsonReader(rd);
                    return serialier.Deserialize(jsonRd);
                }
            });
        }
    }
}