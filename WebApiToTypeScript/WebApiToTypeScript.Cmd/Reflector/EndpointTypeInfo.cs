using System.Collections.Generic;

namespace WebApiToTypeScript.Cmd.Reflector
{
    internal class EndpointTypeInfo
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public IEnumerable<ParameterInfo> Parameters { get; set; }
    }
}