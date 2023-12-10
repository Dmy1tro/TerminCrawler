using System.Collections.Generic;
using Newtonsoft.Json;

namespace Crawler.Anticaptcha.Models
{
    internal class CreateTaskRequest
    {
        public static CreateTaskRequest From(ImageToTextInfo model, string clientKey)
        {
            return new CreateTaskRequest(model, clientKey);
        }

        public CreateTaskRequest(ImageToTextInfo model, string clientKey)
        {
            ClientKey = clientKey;
            Task = new Dictionary<string, object>
            {
                { "body", model.BodyBase64.Replace("\r", "").Replace("\n", "") },
                { "type", "ImageToTextTask" },
                { "phrase", false },
                { "case", false },
                { "numeric", 0 },
                { "math", 0 },
                { "minLength", 0 },
                { "maxLength", 0 }
            };
        }

        [JsonProperty("clientKey")]
        public string ClientKey { get; set; }

        [JsonProperty("task")]
        public Dictionary<string, object> Task { get; set; }
    }
}
