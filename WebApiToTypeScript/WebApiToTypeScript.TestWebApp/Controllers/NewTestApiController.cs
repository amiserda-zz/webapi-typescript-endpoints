using System;
using System.Web.Http;

namespace WebApiToTypeScript.TestWebApp.Controllers
{
    public class NewTestApiController : ApiController
    {
        public DateTime Get(bool a, byte b, char c, decimal d, double e, float f, int g, long h, sbyte i, short j, uint k, ulong l, ushort m)
        {
            return DateTime.Now;
        }
    }
}