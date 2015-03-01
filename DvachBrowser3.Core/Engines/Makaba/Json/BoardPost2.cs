using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Пост на борде (makaba).
    /// </summary>
    public class BoardPost2 : BoardPost
    {
        /// <summary>
        /// MD5-хэш.
        /// </summary>
        [JsonProperty("md5")]
        public string Md5 { get; set; }

        /// <summary>
        /// Трипкод.
        /// </summary>
        [JsonProperty("trip")]
        public string Tripcode { get; set; }

        /// <summary>
        /// Адрес почты.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Редактировался.
        /// </summary>
        [JsonProperty("edited")]
        public string Edited { get; set; }

        /// <summary>
        /// Имя изображения.
        /// </summary>
        [JsonProperty("image_name")]
        public string ImageName { get; set; }

        /// <summary>
        /// Количество изображений.
        /// </summary>
        [JsonProperty("images_count")]
        public string ImagesCount { get; set; }

        /// <summary>
        /// Иконка.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Файлы.
        /// </summary>
        [JsonProperty("files")]
        public BoardPostFile2[] Files { get; set; }
    }
}