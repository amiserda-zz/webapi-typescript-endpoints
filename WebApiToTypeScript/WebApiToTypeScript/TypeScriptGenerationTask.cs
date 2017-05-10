using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
