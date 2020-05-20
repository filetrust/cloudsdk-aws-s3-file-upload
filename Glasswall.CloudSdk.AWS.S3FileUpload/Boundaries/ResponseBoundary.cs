using Newtonsoft.Json;

namespace S3FileUpload.Boundaries
{
    public class ResponseBoundary
    {
        [JsonProperty]
        public string PresignedUrl { get; set; }
        [JsonProperty]
        public string Bucket { get; set; }
        [JsonProperty]
        public string ObjectKey { get; set; }
        [JsonProperty]
        public string Region { get; set; }
        [JsonProperty]
        public string Expiry { get; set; }
    }
}
