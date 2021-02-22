using GraphiAPI.Helpers;
using GraphiAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GraphiAPI.Services
{
    public class GraphService : IGraphService
    {
        private readonly GraphApiConfig _graphApiConfig;

        public GraphService(IOptions<GraphApiConfig> graphApiConfig)
        {
            _graphApiConfig = graphApiConfig.Value;
        }

        public async Task<string> GetAccessToken()
        {
            AuthenticationResult result = null;
            try
            {
                IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(_graphApiConfig.ClientId)
                        .WithClientSecret(_graphApiConfig.ClientSecret)
                        .WithAuthority(new Uri(_graphApiConfig.Authority))
                        .Build();

                result = await app.AcquireTokenForClient(GraphConstants.DefaultScopes).ExecuteAsync();
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                Console.WriteLine("Scope provided is not supported");
            }
            return result.AccessToken;
        }

        public async Task<GraphUser> GetUserById(string id)
        {
            GraphUser result = null;
            var httpClient = new HttpClient();
            var apiCaller = new WebApiCallHelper(httpClient);

            string accessToken = await GetAccessToken();

            if (accessToken != null)
            {
                var url = $"{_graphApiConfig.ApiUrl}/users/{id}{GraphConstants.UserQueryParameter}";
                var jObjectResult = await apiCaller.CallWebApiGetAsync(url, accessToken);

                result = new GraphUser
                {
                    Id = jObjectResult.GetValue("id").ToString(),
                    DisplayName = jObjectResult.GetValue("displayName").ToString(),
                    Mail = jObjectResult.GetValue("mail").ToString()
                };
            }
            return result;
        }

        public async Task<Chat> CreateOneOnOneChat()
        {
            Chat result = null;
            var membersList = new Dictionary<string, object>
            {
                { "@odata.type", "#microsoft.graph.aadUserConversationMember" },
                { "roles", new string[] { "owner" } },
                { "user@odata.bind", $"https://graph.microsoft.com/beta/users/5069e042-a8dd-42db-8271-d8309ba205dc" }
            };
            var body = JsonConvert.SerializeObject(new { chatType = "oneOnOne", members = new[] { membersList } });
            var httpClient = new HttpClient();
            var apiCaller = new WebApiCallHelper(httpClient);

            string accessToken = await GetAccessToken();

            if (accessToken != null)
            {
                var url = $"{_graphApiConfig.ApiUrl}/chats";
                var jObjectResult = await apiCaller.CallWebApiPostAsync(url, body, accessToken);

                result = new Chat
                {
                    Id = jObjectResult.GetValue("id").ToString(),
                    Topic = jObjectResult.GetValue("topic").ToString(),
                    CreatedDateTime = jObjectResult.GetValue("createdDateTime").ToString(),
                    LastUpdatedDateTime = jObjectResult.GetValue("lastUpdatedDateTime").ToString(),
                    ChatType = jObjectResult.GetValue("chatType").ToString()
                };
            }
            return result;
        }
    }
}
