using Newtonsoft.Json;
using System.Collections.Generic;

namespace Anticaptcha.Response
{
    internal class SolutionData
    {
        [JsonProperty("answers")]
        public string Answers { get; set; } // Will be available for CustomCaptcha tasks only!

        [JsonProperty("gRecaptchaResponse")]
        public string GRecaptchaResponse { get; set; } // Will be available for Recaptcha tasks only!

        [JsonProperty("gRecaptchaResponseMd5")]
        public string GRecaptchaResponseMd5 { get; set; } // for Recaptcha with isExtended=true property

        [JsonProperty("text")]
        public string Text { get; set; } // Will be available for ImageToText tasks only!

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; } // Will be available for FunCaptcha tasks only!

        [JsonProperty("challenge")]
        public string Challenge { get; set; } // Will be available for GeeTest tasks only

        [JsonProperty("seccode")]
        public string Seccode { get; set; } // Will be available for GeeTest tasks only

        [JsonProperty("validate")]
        public string Validate { get; set; } // Will be available for GeeTest tasks only

        [JsonProperty("cellNumbers")]
        public List<int> CellNumbers { get; set; } = new List<int>(); // Will be available for Square tasks only
    }
}
