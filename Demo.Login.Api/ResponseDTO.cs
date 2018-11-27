using Newtonsoft.Json;

namespace Demo.Login.Api
{
    public class ResponseDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public ResponseDTO()
        {
        }

        public bool Success { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    }
}
