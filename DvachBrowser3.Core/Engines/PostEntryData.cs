using System.Collections.Generic;
using DvachBrowser3.Links;
using DvachBrowser3.Posting;

namespace DvachBrowser3.Engines
{
    /// <summary>
    /// Данные постинга.
    /// </summary>
    public class PostEntryData
    {
        /// <summary>
        /// Ссылка.
        /// </summary>
        public BoardLinkBase Link { get; set; }

        /// <summary>
        /// Общие данные.
        /// </summary>
        public Dictionary<PostingFieldSemanticRole, object> CommonData { get; set; }

        /// <summary>
        /// Медиафайлы.
        /// </summary>
        public MediaFilePostingData[] MediaFiles { get; set; }

        /// <summary>
        /// Капча.
        /// </summary>
        public CaptchaPostingData Captcha { get; set; }
    }
}