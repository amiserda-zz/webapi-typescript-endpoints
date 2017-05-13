using System.Collections.Generic;

namespace WebApiToTypeScript.Cmd.Mapper
{
    internal class ServiceClassInfo
    {
        public string Name { get; set; }
        public IEnumerable<FunctionInfo> Functions { get; set; }
    }
}