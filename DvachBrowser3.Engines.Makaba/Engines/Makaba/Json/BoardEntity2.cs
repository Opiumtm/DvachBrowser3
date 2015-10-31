using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Сущность "борда или тред".
    /// </summary>
    public class BoardEntity2
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
        /// Скорость борды.
        /// </summary>
        [JsonProperty("board_speed")]
        public string BoardSpeed { get; set; }

        /// <summary>
        /// Текущая страница.
        /// </summary>
        [JsonProperty("current_page")]
        public string CurrentPage { get; set; }

        /// <summary>
        /// Текущий тред.
        /// </summary>
        [JsonProperty("current_thread")]
        public string CurrentThread { get; set; }

        /// <summary>
        /// Разрешить аудио.
        /// </summary>
        [JsonProperty("enable_audio")]
        public string EnableAudio { get; set; }

        /// <summary>
        /// Разрешить кубики (?)
        /// </summary>
        [JsonProperty("enable_dices")]
        public string EnableDices { get; set; }

        /// <summary>
        /// Разрешить иконки.
        /// </summary>
        [JsonProperty("enable_icons")]
        public string EnableIcons { get; set; }

        /// <summary>
        /// Разрешить иконки.
        /// </summary>
        [JsonProperty("enable_flags")]
        public string EnableFlags { get; set; }

        /// <summary>
        /// Разрешить изображения.
        /// </summary>
        [JsonProperty("enable_images")]
        public string EnableImages { get; set; }

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
        /// Разрешить заголово.
        /// </summary>
        [JsonProperty("enable_subject")]
        public string EnableSubject { get; set; }

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
        /// Иконки.
        /// </summary>
        [JsonProperty("icons")]
        public BoardIcon2[] Icons { get; set; }

        /// <summary>
        /// Объект является бордой.
        /// </summary>
        [JsonProperty("is_board")]
        public string IsBoard { get; set; }

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
        /// Страницы.
        /// </summary>
        [JsonProperty("pages")]
        public string[] Pages { get; set; }

        /// <summary>
        /// Треды.
        /// </summary>
        [JsonProperty("threads")]
        public BoardThread2[] Threads { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}