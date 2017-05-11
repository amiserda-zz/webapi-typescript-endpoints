using System.Web.Http;

namespace WebApiToTypeScript.TestWebApp.Controllers
{
    public class TestController : ApiController
    {
        public string Get()
        {
            return "value";
        }
    }
}
