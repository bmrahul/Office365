using AccessToken.Models;
using AccessToken.Services;
using Microsoft.Identity.Client;
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
            var httpClient = new HttpClient();
            GraphOperations graphOperations = new GraphOperations(httpClient);

            AuthenticationResult token = await graphOperations.AcquireAccessToken();

            var result = await graphOperations.ReadEmailAsync(token.AccessToken, mailBoxName);

            return Json<OData>(result);
        }
    }
}
