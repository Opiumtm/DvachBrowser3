using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Сущность каталога.
    /// </summary>
    public class CatalogEntity
    {
        /// <summary>
        /// Борда.
        /// </summary>
        [JsonProperty("Board")]
        public string Board { get; set; }

        /// <summary>
        /// Информация о борде.
        /// </summary>
        [JsonProperty("BoardInfo")]
        public string BoardInfo { get; set; }

        /// <summary>
        /// Дополнительная информация о борде.
        /// </summary>
        [JsonProperty("BoardInfoOuter")]
        public string BoardInfoOuter { get; set; }

        /// <summary>
        /// Имя борды.
        /// </summary>
        [JsonProperty("BoardName")]
        public string BoardName { get; set; }

        /// <summary>
        /// Баннер.
        /// </summary>
        [JsonProperty("board_banner_image")]
        public string BoardBannerImage { get; set; }

        /// <summary>
        /// Ссылка на борду (короткое имя).
        /// </summary>
        [JsonProperty("board_banner_link")]
        public string BoardBannerLink { get; set; }

        /// <summary>
        /// Бамп лимит.
        /// </summary>
        [JsonProperty("bump_limit")]
        public string BumpLimit { get; set; }

        /// <summary>
        /// Имя по умолчанию.
        /// </summary>
        [JsonProperty("default_name")]
        public string DefaultName { get; set; }

        /// <summary>
        /// Разрешить кубики (?)
        /// </summary>
        [JsonProperty("enable_dices")]
        public string EnableDices { get; set; }

        /// <summary>
        /// Разрешить иконки.
        /// </summary>
        [JsonProperty("enable_flags")]
        public string EnableFlags { get; set; }

        /// <summary>
        /// Разрешить иконки.
        /// </summary>
        [JsonProperty("enable_icons")]
        public string EnableIcons { get; set; }

        /// <summary>
        /// Разрешить изображения.
        /// </summary>
        [JsonProperty("enable_images")]
        public string EnableImages { get; set; }

        /// <summary>
        /// Разрешить лайки.
        /// </summary>
        [JsonProperty("enable_likes")]
        public string EnableLikes { get; set; }

        /// <summary>
        /// Разрешить имена.
        /// </summary>
        [JsonProperty("enable_names")]
        public string EnableNames { get; set; }

        /// <summary>
        /// Разрешить постинг.
        /// </summary>
        [JsonProperty("enable_posting")]
        public string EnablePosting { get; set; }

        /// <summary>
        /// Разрешить сажу.
        /// </summary>
        [JsonProperty("enable_sage")]
        public string EnableSage { get; set; }

        /// <summary>
        /// Разрешить щит (?).
        /// </summary>
        [JsonProperty("enable_shield")]
        public string EnableShield { get; set; }

        /// <summary>
        /// Разрешить заголовок.
        /// </summary>
        [JsonProperty("enable_subject")]
        public string EnableSubject { get; set; }

        /// <summary>
        /// Разрешить тэги тредов.
        /// </summary>
        [JsonProperty("enable_thread_tags")]
        public string EnableThreadTags { get; set; }

        /// <summary>
        /// Разрешить трипкоды.
        /// </summary>
        [JsonProperty("enable_trips")]
        public string EnableTrips { get; set; }

        /// <summary>
        /// Разрешить видео.
        /// </summary>
        [JsonProperty("enable_video")]
        public string EnableVideo { get; set; }

        /// <summary>
        /// Фильтр.
        /// </summary>
        [JsonProperty("filter")]
        public string Filter { get; set; }

        /// <summary>
        /// Максимальный комментарий.
        /// </summary>
        [JsonProperty("max_comment")]
        public string MaxComment { get; set; }

        /// <summary>
        /// Новости.
        /// </summary>
        [JsonProperty("news")]
        public BoardEntityNewsReference[] News { get; set; }

        /// <summary>
        /// Треды.
        /// </summary>
        [JsonProperty("threads")]
        public BoardPost2[] Threads { get; set; }
    }
}