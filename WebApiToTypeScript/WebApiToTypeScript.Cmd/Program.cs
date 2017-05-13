using System.IO;

namespace WebApiToTypeScript.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyFilename = @"..\..\..\WebApiToTypeScript.TestWebApp\bin\WebApiToTypeScript.TestWebApp.dll";

            var typeInfo = new DotNetEndpointsTypeInfoExtractor().Extract(assemblyFilename);

            var typeScriptCode = new CodeGenerator().Generate(typeInfo);

            File.WriteAllText(@"..\..\..\WebApiToTypeScript.TestWebApp\Scripts\GeneratedTypeScript\Api.ts", typeScriptCode);
        }
    }
}
