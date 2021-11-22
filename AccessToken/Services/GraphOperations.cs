using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AccessToken.Services
{
    public class GraphOperations
    {
        protected HttpClient HttpClient { get; private set; }
        private string TenantId = ConfigurationManager.AppSettings["TenantId"];
        private string ClientId = ConfigurationManager.AppSettings["ClientId"];
        private string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        private string Authority = ConfigurationManager.AppSettings["Authority"];
        private string ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        public GraphOperations(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task DelegatedAuth()
        {
            IEnumerable<string> scopes = new string[] { $"{ApiUrl}.default" };
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var clientSecretCredential = new ClientSecretCredential(TenantId, ClientId, ClientSecret, options);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
            var messages = await graphClient.Me.Messages
                .Request()
                .Select(m => new {
                    m.Subject,
                    m.Sender
                }).GetAsync();
        }

        public async Task<AuthenticationResult> AcquireAccessToken()
        {
            IConfidentialClientApplication app;
            app = ConfidentialClientApplicationBuilder.Create(ClientId)
                    .WithClientSecret(ClientSecret)
                    .WithAuthority(new Uri(Authority))
                    .Build();

            AuthenticationResult result;
            IEnumerable<string> scopes = new string[] { $"{ApiUrl}.default" };

            try
            {
                result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public async Task<List<Object>> ReadEmailAsync(string accessToken)
        {
            List<Object> listObject = new List<object>();
            HttpResponseMessage response = null;
            if (!string.IsNullOrEmpty(accessToken))
            {
                var defaultRequestHeaders = HttpClient.DefaultRequestHeaders;
                if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    response = await HttpClient.GetAsync("https://graph.microsoft.com/v1.0/me/messages");
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JObject result = JsonConvert.DeserializeObject(json) as JObject;
                    listObject = processResult(result);
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                }
            }
            return listObject;
        }

        private List<Object> processResult(JObject result)
        {
            List<Object> oDataCollection = new List<object>();
            foreach (JProperty child in result.Properties().Where(p => !p.Name.StartsWith("@")))
            {
                oDataCollection.Add($"{child.Name} = {child.Value}");
            }
            return oDataCollection;
        }
    }
}