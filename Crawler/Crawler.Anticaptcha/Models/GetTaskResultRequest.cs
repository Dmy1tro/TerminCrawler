using Newtonsoft.Json;

namespace Crawler.Anticaptcha.Models
{
    internal class GetTaskResultRequest
    {
        [JsonProperty("taskId")]
        public int TaskId { get; set; }

        [JsonProperty("clientKey")]
        public string ClientKey { get; set; }
    }
}
