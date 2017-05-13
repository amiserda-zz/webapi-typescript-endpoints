using System.Collections.Generic;

namespace WebApiToTypeScript.Cmd.Mapper
{
    internal class TypeScriptArtefacts
    {
        public string Module { get; set; }
        public IEnumerable<ServiceClassInfo> Services { get; set; }
    }
}