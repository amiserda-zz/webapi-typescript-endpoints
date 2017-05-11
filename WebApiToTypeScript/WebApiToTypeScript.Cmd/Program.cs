using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApiToTypeScript.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = @"C:\source\gen-endpoints\WebApiToTypeScript\WebApiToTypeScript.TestWebApp\bin\WebApiToTypeScript.TestWebApp.dll";
            var assembly = GetAssembly(filename);
            var types = GetTypes(assembly);

            new CodeGenerator().Generate(types);
        }

        private static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Controller"));
        }

        private static Assembly GetAssembly(string filename)
        {
            return Assembly.LoadFrom(filename);
        }
    }
}
