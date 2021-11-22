using AccessToken.Services;
using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AccessToken.Controllers
{
    [RoutePrefix("api/Home")]
    public class HomeController : ApiController
    {
        [Route("Index")]
        [HttpGet]
        public async Task<IHttpActionResult> Index()
        {
            var httpClient = new HttpClient();
            GraphOperations graphOperations = new GraphOperations(httpClient);

            AuthenticationResult token = await graphOperations.AcquireAccessToken();

            var result = await graphOperations.ReadEmailAsync(token.AccessToken);

            return Json<Object>(result);
        }
    }
}
