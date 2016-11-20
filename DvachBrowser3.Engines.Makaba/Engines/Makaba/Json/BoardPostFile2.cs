using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Файл.
    /// </summary>
    public class BoardPostFile2
    {
        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        [JsonProperty("displayname")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Полное имя.
        /// </summary>
        [JsonProperty("fullname")]
        public string FullName { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        [JsonProperty("height")]
        public int Heigth { get; set; }

        /// <summary>
        /// MD5-хэш.
        /// </summary>
        [JsonProperty("md5")]
        public string Md5 { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Не для работы.
        /// </summary>
        [JsonProperty("nsfw")]
        public int NotSafeForWork { get; set; }

        /// <summary>
        /// Путь.
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// Размер.
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; set; }

        /// <summary>
        /// Идентификатор стикера.
        /// </summary>
        [JsonProperty("sticker")]
        public string Sticker { get; set; }

        /// <summary>
        /// Уменьшенное изображение.
        /// </summary>
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        /// <summary>
        /// Высота превью.
        /// </summary>
        [JsonProperty("tn_height")]
        public int TnHeight { get; set; }

        /// <summary>
        /// Ширина превью.
        /// </summary>
        [JsonProperty("tn_width")]
        public int TnWidth { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// Ширина.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }
    }
}