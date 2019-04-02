using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Net.Http.Headers;
using System.IO;

namespace ImageTesting
{
    public class PersonManager
    {
        /// <summary>
        /// Create a new person and ad him to a PersonGroup
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="name"></param>
        /// <param name="key">Subscription Key</param>
        /// <returns>return the person ID in string </returns>
        public async static Task<string> Creatperson(string groupID, string name, string key)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            var uri = $"{SharedData.ServerLocation}{groupID}/persons";

            HttpResponseMessage response;
            var requestBody = "{\"name\": \"" + name + "\"}";
            byte[] byteData = Encoding.UTF8.GetBytes(requestBody);

            using(var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result)["personId"];
        }


        public async static Task<bool> AddpersonFace(string groupID, string personID, string imagePath, string subscriptionKey, string targetFace = null)
        {
            var client = new HttpClient();
            var queryString = targetFace != null ? HttpUtility.ParseQueryString(targetFace) : HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            if(targetFace != null)
                queryString["targetFace"] = targetFace;

            var uri = $"{SharedData.ServerLocation}{groupID}/persons/{personID}/persistedFaces/" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData =  File.ReadAllBytes(imagePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
            }

            return response.IsSuccessStatusCode;
        }

        public async static Task<List<Person>> GetAllPersons()
        {
            s
        }

        
    }
}