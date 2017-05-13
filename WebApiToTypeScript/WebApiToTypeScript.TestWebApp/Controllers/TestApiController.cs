using System.Web.Http;

namespace WebApiToTypeScript.TestWebApp.Controllers
{
    public class TestApiController : ApiController
    {
        public string Get1()
        {
            return "value";
        }

        public int Get2(int i, string s)
        {
            return -2;
        }
    }
}
