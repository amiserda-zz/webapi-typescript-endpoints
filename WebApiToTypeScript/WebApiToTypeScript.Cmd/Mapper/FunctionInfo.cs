using System.Collections.Generic;

namespace WebApiToTypeScript.Cmd.Mapper
{
    internal class FunctionInfo
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public IEnumerable<ParameterInfo> Parameters { get; set; }
    }
}