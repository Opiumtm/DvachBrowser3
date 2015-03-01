using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Проверка на апдейты.
    /// </summary>
    public class CheckUpdatesData
    {
        /// <summary>
        /// Последний номер поста.
        /// </summary>
        [JsonProperty("num")]
        public int LastNum { get; set; }

        /// <summary>
        /// Количество постов.
        /// </summary>
        [JsonProperty("posts")]
        public int Posts { get; set; }

        /// <summary>
        /// Временная метка.
        /// </summary>
        [JsonProperty("timestamp")]
        public int TimeStamp { get; set; }

        /// <summary>
        /// Код ошибки.
        /// </summary>
        [JsonProperty("Code")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        [JsonProperty("Error")]
        public string ErrorMessage { get; set; }
    }
}