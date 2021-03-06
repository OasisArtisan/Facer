﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;

namespace Facer.ImageAnalyzer
{
    class FaceDetectionRequest
    {
        private const string _subscriptionKey = "98766ba89df84f7d93d7bfab99585679";
        private const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";

        private ImageReady image { get; set; }
        private byte[] imageByteArray;
        HttpClient client;
        string uri;

        public FaceDetectionRequest(ImageReady image)
        {
            this.image = image;
            imageByteArray = image.GetImageAsByteArray();

            client = new HttpClient();
            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true";

            // Assemble the URI for the REST API Call.
            uri = uriBase + "?" + requestParameters;
        }

        private async Task<string> MakeAnalysisRequest()
        {
            HttpResponseMessage response;

            using (ByteArrayContent content = new ByteArrayContent(imageByteArray))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                return contentString;
            }
        }

        
    }
}
