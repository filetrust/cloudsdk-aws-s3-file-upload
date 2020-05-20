using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using S3FileUpload.Models;
using System;

namespace S3FileUpload.Logic
{
    public class GeneratePreSignedUrl
    {
        private static IAmazonS3 s3Client;
        S3LookupModel bucketInfo;

        public GeneratePreSignedUrl(S3LookupModel _bucketInfo)
        {
            bucketInfo = _bucketInfo;
            s3Client = new AmazonS3Client(RegionEndpoint.GetBySystemName(bucketInfo.RegionEndpoint));
        }

        public string GenerateURL()
        {
            string urlString = "";
            try
            {
                GetPreSignedUrlRequest urlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketInfo.BucketName,
                    Key = bucketInfo.ObjectKey,
                    Verb = bucketInfo.Verb,
                    Expires = DateTime.Now.AddHours(1)
                };
                urlString = s3Client.GetPreSignedURL(urlRequest);                
            }
            catch (AmazonS3Exception e)
            {
                LambdaLogger.Log("S3 Exception: " + e.InnerException + " " + e.Message);
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Exception has occurred: " + e.InnerException + " " + e.Message);
            }
            LambdaLogger.Log("Pre-signed URL: " + urlString);
            return urlString;
        }       
    }
}

