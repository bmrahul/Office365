using AccessToken.Models;
using AccessToken.Services;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AccessToken.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        private string ClientId = ConfigurationManager.AppSettings["ClientId"];

        [Route("application/{mailBoxName}")]
        [HttpGet]
        public async Task<IHttpActionResult> application(string mailBoxName)
        {
            OData data;
            var httpClient = new HttpClient();
            HttpResponseMessage result;
            GraphOperations graphOperations = new GraphOperations(httpClient);

            AuthenticationResult token = await graphOperations.AcquireAccessToken();

            if (!string.IsNullOrEmpty(token.AccessToken))
            {
                result = await graphOperations.ReadEmailAsync(token.AccessToken, mailBoxName);
                if (result.IsSuccessStatusCode)
                {
                    string json = await result.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<OData>(json);
                    return Json<OData>(data);
                }
                else
                {
                    return Json<HttpResponseMessage>(result);
                }
            }
            else
            {
                return Json<string>("Unauthorized");
            }
        }

        [Route("delegated/{username}/{secret}")]
        [HttpGet]
        public async Task<IHttpActionResult> Delegated(string username, string secret)
        {
            AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile();
            var appConfig = config.PublicClientApplicationOptions;
            var app = PublicClientApplicationBuilder.CreateWithApplicationOptions(appConfig).Build();
            GraphAuthenticationService graphAuthenticationService = new GraphAuthenticationService(app);

            OData data;
            var httpClient = new HttpClient();
            HttpResponseMessage result;
            AuthenticationResult token = await graphAuthenticationService.AquireToken(username, secret);

            MiningMails miningMails = new MiningMails(httpClient);

            if (token != null)
            {
                result = await miningMails.ReadEmailAsync(token.AccessToken);
                if (result.IsSuccessStatusCode)
                {
                    string json = await result.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<OData>(json);
                    return Json<OData>(data);
                }
                else
                {
                    return Json<HttpResponseMessage>(result);
                }
            }
            else
            {
                return Json<string>("Unauthorized");
            }
        }
    }
}
