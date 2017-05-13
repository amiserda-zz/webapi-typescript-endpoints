using System.IO;

namespace WebApiToTypeScript.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyFilename = @"..\..\..\WebApiToTypeScript.TestWebApp\bin\WebApiToTypeScript.TestWebApp.dll";

            var dotNetEndpointsTypeInfo = DotNetEndpointsTypeInfoExtractor.Extract(assemblyFilename);
            var typeScriptArtefacts = DotNetEndpointsToTypeScriptArtefactsMapper.Map(dotNetEndpointsTypeInfo);
            var typeScriptCode = TypeScriptCodeGenerator.Generate(typeScriptArtefacts);

            File.WriteAllText(@"..\..\..\WebApiToTypeScript.TestWebApp\Scripts\GeneratedTypeScript\Api.ts", typeScriptCode);
        }
    }
}
