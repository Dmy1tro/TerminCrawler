using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Services.Helpers
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> Post<TResponse>(
            this HttpClient httpClient,
            Uri uri,
            object request,
            JsonSerializerSettings? serializerSettings = default) where TResponse : class
        {
            serializerSettings ??= GetDefaultSerializerSettings();
            var jsonData = JsonConvert.SerializeObject(request, serializerSettings);
            var rawData = await Post(httpClient, uri, jsonData);

            if (string.IsNullOrEmpty(rawData))
                return null;

            var data = JsonConvert.DeserializeObject<TResponse>(rawData);

            return data;
        }

        private static async Task<string> Post(this HttpClient httpClient, Uri uri, string jsonData)
        {
            using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync(uri, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                DebugHelper.LogError($"Failed to execute http request to uri: {uri}. HttpCode: {response.StatusCode}. Details: {result}.");
                return null;
            }

            return result;
        }

        private static JsonSerializerSettings GetDefaultSerializerSettings() => new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
