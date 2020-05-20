S3 FileUpload

To perform a request to these two endpoints:
 - Each of these API calls require an API key. 

1) Generate post pre-signed URL 
Endpoint used to generate a post pre-signed URL- URL, which will allow a customer to upload a file. 

        Request
        GET api/generate-post-presigned-url/{Filename}
        
        url --location --request GET '/API URL/{Filename}'
        
        Response
        {"PresignedUrl": "https://presignedurl",
            "ObjectKey": "your object key",
            "Region": "us-east-2",
            "Region": "us-east-2",
            "Expiry": "4/7/20 3:06:21 PM"}

2) Generate pre-signed URL 
Endpoint used to generate a pre-signed URL- URL, which will allow a customer to download a given file. 
        Request GET /generate-presigned-url
        
        curl --location --request GET '/API URL?query string parameters'
        
        Response
        {"PresignedUrl":"https://presignedurl",
            "Bucket": "your bucket",
            "ObjectKey": "your object key",
            "Region": "us-east-2",
            "Expiry": "4/7/20 3:06:21 PM"}
