using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Результат постинга JSON.
    /// </summary>
    public class PostingJsonResult
    {
        /// <summary>
        /// Статус.
        /// </summary>
        [JsonProperty("Reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Ошибка.
        /// </summary>
        [JsonProperty("Error")]
        public string Error { get; set; }

        /// <summary>
        /// Статус.
        /// </summary>
        [JsonProperty("Status")]
        public string Status { get; set; }

        /// <summary>
        /// Номер поста.
        /// </summary>
        [JsonProperty("Num")]
        public string Num { get; set; }

        /// <summary>
        /// Номер треда.
        /// </summary>
        [JsonProperty("Target")]
        public string Target { get; set; }
    }
}