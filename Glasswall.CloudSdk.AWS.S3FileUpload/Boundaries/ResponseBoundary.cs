using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwsDotnetCsharp
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
