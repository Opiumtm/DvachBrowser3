using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    /// <summary>
    /// Иконка борды.
    /// </summary>
    public class BoardIcon2
    {
        /// <summary>
        /// Имя.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Номер.
        /// </summary>
        [JsonProperty("num")]
        public string Number { get; set; }

        /// <summary>
        /// URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public int NumberInt
        {
            get
            {
                int r;
                if (int.TryParse(Number, out r))
                {
                    return r;
                }
                return 0;
            }
        }
    }
}