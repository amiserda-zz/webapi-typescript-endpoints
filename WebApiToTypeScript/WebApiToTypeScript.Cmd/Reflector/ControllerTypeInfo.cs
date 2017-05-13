using System.Collections.Generic;

namespace WebApiToTypeScript.Cmd.Reflector
{
    internal class ControllerTypeInfo
    {
        public string TypeName { get; set; }
        public IEnumerable<EndpointTypeInfo> EndpointTypeInfos { get; set; }
    }
}