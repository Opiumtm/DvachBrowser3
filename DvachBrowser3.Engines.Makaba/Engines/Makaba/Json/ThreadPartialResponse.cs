using System;
using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Результат получения части треда.
    /// </summary>
    [JsonConverter(typeof(ThreadPartialResponse.Coverter))]
    public sealed class ThreadPartialResponse
    {
        /// <summary>
        /// Ошибка.
        /// </summary>
        public ThreadPartialError Error { get; set; }

        /// <summary>
        /// Посты.
        /// </summary>
        public BoardPost2[] Posts { get; set; }

        /// <summary>
        /// Конвертер.
        /// </summary>
        public class Coverter : JsonConverter
        {
            /// <summary>
            /// Writes the JSON representation of the object.
            /// </summary>
            /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
            /// <exception cref="NotImplementedException">Метод не поддерживается.</exception>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param><param name="objectType">Type of the object.</param><param name="existingValue">The existing value of object being read.</param><param name="serializer">The calling serializer.</param>
            /// <returns>
            /// The object value.
            /// </returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.StartArray)
                {
                    return new ThreadPartialResponse()
                    {
                        Error = null,
                        Posts = serializer.Deserialize<BoardPost2[]>(reader)
                    };
                }
                if (reader.TokenType == JsonToken.StartObject)
                {
                    return new ThreadPartialResponse()
                    {
                        Error = serializer.Deserialize<ThreadPartialError>(reader),
                        Posts = null
                    };                    
                }
                throw new JsonReaderException();
            }

            /// <summary>
            /// Determines whether this instance can convert the specified object type.
            /// </summary>
            /// <param name="objectType">Type of the object.</param>
            /// <returns>
            /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
            /// </returns>
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof (ThreadPartialResponse);
            }
        }
    }
}