using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Пост на борде.
    /// </summary>
    public class BoardPost
    {
        /// <summary>
        /// Ширина.
        /// </summary>
        [JsonProperty("width")]
        public string Width { get; set; }

        /// <summary>
        /// Последнее попадание.
        /// </summary>
        [JsonProperty("lasthit")]
        public string LastHit { get; set; }

        /// <summary>
        /// Номер поста.
        /// </summary>
        [JsonProperty("num")]
        public string Number { get; set; }

        /// <summary>
        /// Забанен.
        /// </summary>
        [JsonProperty("banned")]
        public string Banned { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// Размер.
        /// </summary>
        [JsonProperty("size")]
        public string Size { get; set; }

        /// <summary>
        /// Временная метка.
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        /// <summary>
        /// Прикреплённый.
        /// </summary>
        [JsonProperty("sticky")]
        public string Sticky { get; set; }

        /// <summary>
        /// Ширина tn (?).
        /// </summary>
        [JsonProperty("tn_width")]
        public string TnWidth { get; set; }

        /// <summary>
        /// Закрыто.
        /// </summary>
        [JsonProperty("closed")]
        public string Closed { get; set; }

        /// <summary>
        /// Относительный URI предпросмотра изображения.
        /// </summary>
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        /// <summary>
        /// Родительский пост.
        /// </summary>
        [JsonProperty("parent")]
        public string Parent { get; set; }

        /// <summary>
        /// Видео.
        /// </summary>
        [JsonProperty("video")]
        public string Video { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Имя постера.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        [JsonProperty("height")]
        public string Height { get; set; }

        /// <summary>
        /// Изображение.
        /// </summary>
        [JsonProperty("image")]
        public string Image { get; set; }

        /// <summary>
        /// Высота tn.
        /// </summary>
        [JsonProperty("tn_height")]
        public string TnHeight { get; set; }

        /// <summary>
        /// Комментарий.
        /// </summary>
        [JsonProperty("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Пост от ОП-а.
        /// </summary>
        [JsonProperty("op")]
        public string Op { get; set; }
    }
}