using System.Web.Http;

namespace WebApiToTypeScript.TestWebApp.Controllers
{
    public class TestApiController : ApiController
    {
        public string Get1()
        {
            return "value";
        }

        public int Get12(int i, string s)
        {
            return -2;
        }
    }
}
