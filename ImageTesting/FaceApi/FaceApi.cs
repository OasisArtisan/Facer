

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

        private List<Face> Faces = new List<Face>();
        public FaceApi(ImageReady image)
        {
            Image = image;
            imageByteArray = Image.GetImageAsByteArray();
        }

        public List<Face> DetectFaces()
        {
            client = new HttpClient();
            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SharedData.SubscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true";
        }
    }
}	

