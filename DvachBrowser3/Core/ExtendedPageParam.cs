using DvachBrowser3.Links;
using Newtonsoft.Json;
using Template10.Services.SerializationService;

namespace DvachBrowser3
{
    /// <summary>
    /// Расширенный параметр страницы.
    /// </summary>
    public class ExtendedPageParam
    {
        /// <summary>
        /// Параметр ссылки.
        /// </summary>
        [JsonProperty("LinkParam")]
        public string LinkParam { get; set; }

        /// <summary>
        /// Идентификатор дополнительных данных.
        /// </summary>
        [JsonProperty("CustomDataId")]
        public string CustomDataId { get; set; }

        /// <summary>
        /// Перевести в JSON.
        /// </summary>
        /// <returns>JSON.</returns>
        public string ToJson()
        {
            return SerializationService.Json.Serialize(this);
        }

        /// <summary>
        /// Перевести из JSON.
        /// </summary>
        /// <param name="json">Представление JSON.</param>
        /// <returns>Объект.</returns>
        public static ExtendedPageParam FromJson(string json)
        {
            return SerializationService.Json.Deserialize<ExtendedPageParam>(json);
        }
    }

    /// <summary>
    /// Расширенный параметр страницы.
    /// </summary>
    public class ExtendedPageParam2
    {
        /// <summary>
        /// Параметр ссылки.
        /// </summary>
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Идентификатор дополнительных данных.
        /// </summary>
        public string CustomDataId { get; set; }
    }
}