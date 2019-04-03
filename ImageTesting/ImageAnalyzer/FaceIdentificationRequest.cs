using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;

namespace ImageTesting
{
    public class FaceIdentificationRequest
    {
        private ImageReady Image {get;set;}
        private byte[] imageByteArray;
        HttpClient client;
        string uri;

        public FaceIdentificationRequest(ImageReady image)
        {
            Image = image;

            imageByteArray = image.GetImageAsByteArray();

            client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SharedData.SubscriptionKey);

            uri = SharedData.ServiceLocationForServices + "/identify";
            
        }

        public async string SendIdentifyRequest()
        {
            string response = await client.PostAsync(uri)
        }
    }    
}
