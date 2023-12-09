using Crawler.Anticaptcha.Enums;
using Newtonsoft.Json;

namespace Crawler.Anticaptcha.Response
{
    internal class TaskResultResponse
    {
        [JsonProperty("errorId")]
        public int? ErrorId { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; set; }

        [JsonProperty("status")]
        public StatusType? Status { get; set; }

        [JsonProperty("solution")]
        public SolutionData Solution { get; set; }

        [JsonProperty("cost")]
        public double? Cost { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        /// <summary>
        ///     Task create time in UTC
        /// </summary>
        [JsonProperty("createTime")]
        public string CreateTime { get; set; }

        /// <summary>
        ///     Task end time in UTC
        /// </summary>
        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("solveCount")]
        public int? SolveCount { get; set; }
    }
}
