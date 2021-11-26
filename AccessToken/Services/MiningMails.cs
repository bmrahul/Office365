using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AccessToken.Services
{
    public class MiningMails
    {
        protected HttpClient HttpClient { get; private set; }
        public MiningMails(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<HttpResponseMessage> ReadEmailAsync(string accessToken)
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
                response = await HttpClient.GetAsync($"https://graph.microsoft.com/v1.0/me/messages");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return response;
        }
    }
}