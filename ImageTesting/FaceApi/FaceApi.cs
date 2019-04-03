

using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.Collections.Generic;

namespace ImageTesting
{
    class FaceApi
    {
        private ImageReady Image {get;set;}
        private byte[] imageByteArray;
        private HttpClient client;
        string uriBase = SharedData.ServiceLocationForServices;
        string uri;

        private List<Face> Faces = new List<Face>();
        public FaceApi(ImageReady image)
        {
            Image = image;
            imageByteArray = Image.GetImageAsByteArray();
        }

        public async List<Face> DetectFaces()
        {
            MakeHttpRequest("detect");

            HttpResponseMessage response;
            using(var content = new ByteArrayContent(imageByteArray))
            {
                response = await client.PostAsync(uri, content);
                
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
            }

            
        }

        public async 

        // It takes either "identify" or "request"
        public void MakeHttpRequest(string service)
        {
            client = new HttpClient();
            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SharedData.SubscriptionKey);

            uri = uriBase + "/" + service + "?";
        }
    }
}	

