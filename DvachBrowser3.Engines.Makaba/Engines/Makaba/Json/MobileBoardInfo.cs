using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Информация о борде.
    /// </summary>
    public class MobileBoardInfo
    {
        /// <summary>
        /// Бамплимит.
        /// </summary>
        [JsonProperty("bump_limit")]
        public int BumpLimit { get; set; }

        /// <summary>
        /// Категория.
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Имя постера по умолчанию.
        /// </summary>
        [JsonProperty("default_name")]
        public string DefaultName { get; set; }

        /// <summary>
        /// Разрешить лайки.
        /// </summary>
        [JsonProperty("enable_likes")]
        public int EnableLikes { get; set; }

        /// <summary>
        /// Разрешён постинг.
        /// </summary>
        [JsonProperty("enable_posting")]
        public int EnablePosting { get; set; }

        /// <summary>
        /// Разрешить тэги тредов.
        /// </summary>
        [JsonProperty("enable_thread_tags")]
        public int EnableThreadTags { get; set; }

        /// <summary>
        /// Иконки.
        /// </summary>
        [JsonProperty("icons")]
        public BoardIcon2[] Icons { get; set; }

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Количество страниц.
        /// </summary>
        [JsonProperty("pages")]
        public int Pages { get; set; }

        /// <summary>
        /// Сажа.
        /// </summary>
        [JsonProperty("sage")]
        public int Sage { get; set; }

        /// <summary>
        /// Трипкоды.
        /// </summary>
        [JsonProperty("tripcodes")]
        public int Tripcodes { get; set; }
    }
}