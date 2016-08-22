using Newtonsoft.Json;

namespace DvachBrowser3.Engines.Makaba.Json
{
    public class CaptchaV2IdResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("result")]
        public int Result { get; set; } 

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("error")]
        public int Error { get; set; }

        [JsonProperty("description")]
        public string ErrorDescription { get; set; }
    }
}