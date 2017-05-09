using Microsoft.Build.Utilities;

namespace WebApiToTypeScript
{
    public class TypeScriptGenerationTask : AppDomainIsolatedTask
    {
        public override bool Execute()
        {
            return true;
        }
    }
}
