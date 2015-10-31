using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Новости.
    /// </summary>
    public class BoardEntityNewsReference
    {
        /// <summary>
        /// Дата.
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// Номер.
        /// </summary>
        [JsonProperty("num")]
        public string Number { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }
    }
}