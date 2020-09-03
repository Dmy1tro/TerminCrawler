using Newtonsoft.Json;

namespace Anticaptcha.Response
{
    internal class CreateTaskResponse
    {
        [JsonProperty("errorId")]
        public int? ErrorId { get; private set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; private set; }

        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; private set; }

        [JsonProperty("taskId")]
        public int? TaskId { get; private set; }
    }
}
