using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Shared.Helpers
{
    public class HttpHelper
    {
        public static string Post(Uri uri, string jsonData)
        {
            using var httpClient = new HttpClient();
            using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            using var httpResponse = httpClient.PostAsync(uri, content).GetAwaiter().GetResult();

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                return response;
            }

            return null;
        }

        public static T Post<T>(Uri uri, string jsonData) where T : class
        {
            var rawData = Post(uri, jsonData);

            if (string.IsNullOrEmpty(rawData))
                return null;

            var data = JsonConvert.DeserializeObject<T>(rawData);

            return data;
        }
    }
}
