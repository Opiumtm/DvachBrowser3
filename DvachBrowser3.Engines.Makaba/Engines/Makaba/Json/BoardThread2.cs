using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Тред борды.
    /// </summary>
    public class BoardThread2
    {
        /// <summary>
        /// Количество изображений.
        /// </summary>
        [JsonProperty("files_count")]
        public string ImagesCount { get; set; }

        /// <summary>
        /// Посты.
        /// </summary>
        [JsonProperty("posts")]
        public BoardPost2[] Posts { get; set; }

        /// <summary>
        /// Количество постов.
        /// </summary>
        [JsonProperty("posts_count")]
        public string PostsCount { get; set; }

        /// <summary>
        /// Номер треда.
        /// </summary>
        [JsonProperty("thread_num")]
        public string ThreadNumber { get; set; }
    }
}