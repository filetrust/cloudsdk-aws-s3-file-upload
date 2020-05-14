using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
      
        public APIGatewayProxyResponse GenPostPresignedURLHandler(APIGatewayProxyRequest request)
        {
            var requestValidator = new PostUrlRequestValidator();
            LambdaLogger.Log("Environmental Variables:" + JsonConvert.SerializeObject(System.Environment.GetEnvironmentVariables()));

            var requestValidatorResult = requestValidator.Validate(request);

            LambdaLogger.Log("Request validation response: " + requestValidatorResult.IsValid);

            // validate request
            if (!requestValidatorResult.IsValid)
            {
                var badRequestResponse = new APIGatewayProxyResponse
                {
                    StatusCode = 400,
                    Body = JsonConvert.SerializeObject(requestValidatorResult.Errors),
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" }
                    }
                };
                LambdaLogger.Log(JsonConvert.SerializeObject(requestValidatorResult.Errors));
                return badRequestResponse;
            }

            LambdaLogger.Log("Input filename: " + request.PathParameters["Filename"]);

            //create S3 bucket information model class
            var bucketInfo = new S3LookupModel(Environment.GetEnvironmentVariable("BucketName"),
                                               Environment.GetEnvironmentVariable("ObjectKey"),
                                               Environment.GetEnvironmentVariable("RegionEndpoint"),
                                               request.PathParameters["Filename"],
                                               Amazon.S3.HttpVerb.PUT);

            //generate pre-signed URL
            GeneratePreSignedUrl generatePresignedUrl = new GeneratePreSignedUrl(bucketInfo);
            
            var response = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(new ResponseBoundary{
                    PresignedUrl = generatePresignedUrl.GenerateURL(),
                    Bucket = bucketInfo.BucketName,
                    ObjectKey = bucketInfo.ObjectKey, 
                    Region = bucketInfo.RegionEndpoint,
                    Expiry = DateTime.Now.AddHours(2).ToString() //I am adding 2 hours for the JSON response to say the correct time as AWS has an ongoing issue of working with GMT+00 which does not take into consideration daylight saving hours. 
                }),
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };
            return response;
        }

        public APIGatewayProxyResponse GenPresignedURLHandler(APIGatewayProxyRequest request)
        {
            var requestValidator = new GetUrlRequestValidator();
            var requestValidatorResult = requestValidator.Validate(request);

            LambdaLogger.Log("Request validation response: " + requestValidatorResult.IsValid);

            // validate request
            if (!requestValidatorResult.IsValid)
            {
                var badRequestResponse = new APIGatewayProxyResponse
                {
                    StatusCode = 400,
                    Body = JsonConvert.SerializeObject(requestValidatorResult.Errors),
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" }
                    }
                };
                LambdaLogger.Log(JsonConvert.SerializeObject(requestValidatorResult.Errors));
                return badRequestResponse;
            }

            LambdaLogger.Log("Input filename: " + request.QueryStringParameters["fileName"]);

            //create S3 bucket information model class
            var bucketInfo = new S3LookupModel(request.QueryStringParameters["bucketName"],
                                               request.QueryStringParameters["objectPath"],
                                               request.QueryStringParameters["region"],
                                               request.QueryStringParameters["fileName"],
                                               Amazon.S3.HttpVerb.GET);

            //generate pre-signed URL
            GeneratePreSignedUrl generatePresignedUrl = new GeneratePreSignedUrl(bucketInfo);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(new ResponseBoundary
                {
                    PresignedUrl = generatePresignedUrl.GenerateURL(),
                    Bucket = bucketInfo.BucketName,
                    ObjectKey = bucketInfo.ObjectKey,
                    Region = bucketInfo.RegionEndpoint,
                    Expiry = DateTime.Now.AddHours(2).ToString() //I am adding 2 hours for the JSON response to say the correct time as AWS has an ongoing issue of working with GMT+00 which does not take into consideration daylight saving hours. 
                }),
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };
            return response;
        }

    }
}
