using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.Net.Http.Headers;

namespace Facer.FaceApi
{
    public class PersonGroupManager
    {
        private string subscriptionKey;
        #region Static Methods
        public async static Task<bool> CreatPersongroup(string name, string groupID, string key)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
            var uri = SharedData.ServerLocation + "/" + groupID + "?" + queryString;

            HttpResponseMessage response;
            byte[] bytedata = Encoding.UTF8.GetBytes("{\"name\" : \"" + name + "\"}");
            using(var content = new ByteArrayContent(bytedata))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PutAsync(uri, content);
            }

            return response.IsSuccessStatusCode;
        }

        public async static Task<bool> DeletePersongroup(string groupID, string key)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            var uri = SharedData.ServerLocation + "/" + groupID + "?" + queryString;

            var response = await client.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Train the specified Person Group
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async static Task<bool> TrainPersongroup(string groupID, string key)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            var uri = SharedData.ServerLocation + "/" + groupID + "/train" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("");

            using(var content = new ByteArrayContent(byteData))
            {
                //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Retun all the Person Groups for a specified Subscription Key
        /// </summary>
        /// <param name="key">Subscription Key</param>
        /// <returns></returns>
        public async static Task<List<Group>> GetPersongroups(string key)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            var uri = SharedData.ServerLocation;

            var response = await client.GetAsync(uri);

            return JsonConvert.DeserializeObject<List<Group>>(response.Content.ReadAsStringAsync().Result);


        }

        /// <summary>
        /// Get the PersonGroup training status
        /// </summary>
        /// <param name="key">Subscription Key</param>
        /// <returns>"notstarted" for untrained group, "running" for groups being trained, "suceeded" for trained groups, "failed" if somthing went wrong</returns>
        public async static Task<string> GetTrainingstatus(string groupID, string key)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

            var uri = SharedData.ServerLocation + "/" + groupID + "/training" + queryString;

            App.Reference.Printer.PrintLine(uri);
            var response = await client.GetAsync(uri);
            App.Reference.Printer.PrintLine(await response.Content.ReadAsStringAsync());

            // $$ Example of returned data $$$

            /*{
                 "status": "succeeded",
                 "createdDateTime": "2018-10-15T11:51:27.6872495Z",
                 "lastActionDateTime": "2018-10-15T11:51:27.8705696Z",
                 "message": null
              }*/

            // In this function "status" only is returned...
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync())["status"];
        }

        #endregion

        #region Subscription Key and Constructor
        public string SubscriptionKey { get; }
        public PersonGroupManager(string subscriptionKey)
        {
            SubscriptionKey = subscriptionKey;
        }
        #endregion

        #region Instance Methods
        public async Task<bool> CreatPersonGroup(string name, string groupID)
        {
            return await CreatPersongroup(name, groupID, SubscriptionKey);
        }

        public async Task<bool> DeletePersonGroup(string groupID)
        {
            return await DeletePersongroup(groupID, SubscriptionKey);
        }

        public async Task<bool> TrainPersonGroup(string groupID)
        {
            return await TrainPersongroup(groupID, SubscriptionKey);
        }

        public async Task<List<Group>> GetPersonGroups()
        {
            return await GetPersongroups(SubscriptionKey);
        }

        public async Task<string> GetTainingStatus(string groupID)
        {
            return await GetTrainingstatus(groupID, SubscriptionKey);
        }

        #endregion
    }
}
