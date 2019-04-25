using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace Facer.FaceApi
{
    public class FaceApi
    {
        #region Class Data

        private ImageReady Image {get;set;}

        private byte[] imageByteArray;

        private HttpClient client;

        string uriBase = SharedData.ServerLocationForServices;
        private string uri { get; set; }

        private List<Face> Faces = new List<Face>();

        #endregion

        #region Constructor
        public FaceApi(ImageReady image)
        {
            Image = image;
            imageByteArray = Image.GetImageAsByteArray();
        }
        #endregion

        #region Main Methods
        public async Task<List<Face>> DetectFaces()
        {
            MakeHttpRequest(Service.Detection);

            HttpResponseMessage response;
            using(var content = new ByteArrayContent(imageByteArray))
            {
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                response = await client.PostAsync(uri, content);
            }
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();

            return ExtractFacesFromJSON(jsonResponse);
        }

        public async Task<List<FaceIdentifyResult>> IdentifyPerson(string groupID, int maxCandidateNumber = 3, double threshold = 0.7, params string[] facesID)
        {
            MakeHttpRequest(Service.Identification);

            var x = JSONIdentificationBodyWriter(groupID, maxCandidateNumber, threshold, facesID);
            App.Reference.Printer.PrintLine(x);
            var body = Encoding.UTF8.GetBytes(x);

            HttpResponseMessage response;
            using(var content = new ByteArrayContent(body))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }
            var y = await response.Content.ReadAsStringAsync();
            App.Reference.Printer.PrintLine(y);
            return JsonConvert.DeserializeObject<List<FaceIdentifyResult>>(y);
        }

        
        #endregion



        #region Helper Functions
        // It takes either "identify" or "request" depending on the type of function needed
        public void MakeHttpRequest(Service service)
        {
            client = new HttpClient();
            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SharedData.SubscriptionKey);

            switch (service)
            {
                case Service.Detection:
                    uri = uriBase + "/" + "detect" + "?";
                    break;
                case Service.Identification:
                    uri = uriBase + "/" + "identify" + "?";
                    break;
                default:
                    uri = uriBase;
                    break;
            }
            
        }

        public List<Face> ExtractFacesFromJSON(string json)
        {
            return JsonConvert.DeserializeObject<List<Face>>(json);
        }

        public string JSONIdentificationBodyWriter(string groupID, int maxCandidateNumber, double confidenceThreshold, params string[] facesID)
        {
            StringBuilder body = new StringBuilder();
            body.AppendLine("{");
            body.AppendLine($"\"PersonGroupId\": \"{groupID}\",");
            body.AppendLine("\"faceIds\": [");
            for(var i = 0; i < facesID.Length; i++)
            {
                body.Append("\"" + facesID[i] + "\"");
                body.Append((i + 1) == facesID.Length ? "\n" : ",\n");
            }
            body.AppendLine("],");
            body.AppendLine($"\"maxNumOfCandidatesReturned\": {maxCandidateNumber},");
            body.AppendLine($"\"confidenceThreshold\": {confidenceThreshold}");
            body.AppendLine("}");

            return body.ToString();
        }

        #endregion
    }
}	

