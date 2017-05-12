using System.Web.Http;

namespace WebApiToTypeScript.TestWebApp.Controllers
{
    public class TestApiController : ApiController
    {
        public string Get()
        {
            return "value";
        }
    }
}
