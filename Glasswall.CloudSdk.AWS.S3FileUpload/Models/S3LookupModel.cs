using Amazon.S3;
using System;

namespace S3FileUpload.Models
{
    public class S3LookupModel
    {
        public string BucketName { get; set; }
        public string ObjectKey { get; set; }
        public string RegionEndpoint {get;set;}
        public HttpVerb Verb { get; set; }

        public S3LookupModel(string bucketName, string objectPath, string region, string fileName, HttpVerb verb)
        {
            BucketName = bucketName;
            ObjectKey =  verb == HttpVerb.PUT ?
                objectPath + "/" + Guid.NewGuid() + "/" + DateTime.Now.AddHours(1).ToString("dd-MM-yyyy hh:mm:ss") + "/" + fileName : objectPath + "/" + fileName;
            RegionEndpoint = region;
            Verb = verb;
        }
    }
}
