using System.Web.Http;

namespace WebApiToTypeScript.TestWebApp.Controllers
{
    public class TestApiController : ApiController
    {
        public string Get()
        {
            return "value";
        }

        public int Get(int i, string s)
        {
            return -2;
        }
    }
}
