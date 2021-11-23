using AccessToken.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
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
        private string ClientId = ConfigurationManager.AppSettings["ClientId"];
        private string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        private string Authority = ConfigurationManager.AppSettings["Authority"];
        private string ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        public GraphOperations(HttpClient httpClient)
        {
            HttpClient = httpClient;
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

        public async Task<HttpResponseMessage> ReadEmailAsync(string accessToken, string mailBoxName)
        {
            HttpResponseMessage response = null;
            var defaultRequestHeaders = HttpClient.DefaultRequestHeaders;
            if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
            {
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                response = await HttpClient.GetAsync($"https://graph.microsoft.com/v1.0/users/{mailBoxName}@7cwr2b.onmicrosoft.com/messages");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return response;
        }
    }
}