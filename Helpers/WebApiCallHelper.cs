using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GraphiAPI.Helpers
{
    /// <summary>
    /// Helper class to call a protected API and process its result
    /// </summary>
    public class WebApiCallHelper
    {
        protected HttpClient HttpClient { get; private set; }

        public WebApiCallHelper(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<JObject> CallWebApiGetAsync(string webApiUrl, string accessToken)
        {
            JObject result = null;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var defaultRequestHeaders = HttpClient.DefaultRequestHeaders;
                if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await HttpClient.GetAsync(webApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    result = JObject.Parse(json);
                }
                else
                {
                    Console.WriteLine($"Failed to call the web API: {response.StatusCode}");
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Content: {content}");
                }
            }
            return result;
        }

        public async Task<JObject> CallWebApiPostAsync(string webApiUrl, string body, string accessToken)
        {
            JObject result = null;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var defaultRequestHeaders = HttpClient.DefaultRequestHeaders;
                if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                JsonContent jsonContent = JsonContent.Create(body);

                HttpResponseMessage response = await HttpClient.PostAsync(webApiUrl, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    result = JObject.Parse(json);
                }
                else
                {
                    Console.WriteLine($"Failed to call the web API: {response.StatusCode}");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Content: {responseContent}");
                }
            }
            return result;
        }
    }
}
