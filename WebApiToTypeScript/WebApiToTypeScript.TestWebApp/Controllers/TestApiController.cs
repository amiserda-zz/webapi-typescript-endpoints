using System.Web.Http;

namespace WebApiToTypeScript.TestWebApp.Controllers
{
    public class TestApiController : ApiController
    {
        public int Get2(int i, string s)
        {
            return -2;
        }

        [HttpGet]
        public string Get1()
        {
            return "value";
        }

        [HttpGet]
        public string One()
        {
            return "value1";
        }
    }
}
