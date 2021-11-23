using AccessToken.Models;
using AccessToken.Services;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AccessToken.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        [Route("index/{mailBoxName}")]
        [HttpGet]
        public async Task<IHttpActionResult> Index(string mailBoxName)
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
    }
}
