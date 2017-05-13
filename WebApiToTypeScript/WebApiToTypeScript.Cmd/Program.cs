using System.IO;
using WebApiToTypeScript.Cmd.Generator;
using WebApiToTypeScript.Cmd.Mapper;
using WebApiToTypeScript.Cmd.Reflector;

namespace WebApiToTypeScript.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyFilename = @"..\..\..\WebApiToTypeScript.TestWebApp\bin\WebApiToTypeScript.TestWebApp.dll";

            var endpointsTypeInfo = EndpointsInfoReflector.Reflect(assemblyFilename);
            var typeScriptArtefacts = EndpointsToTypeScriptMapper.Map(endpointsTypeInfo);
            var typeScriptCode = CodeGenerator.Generate(typeScriptArtefacts);

            File.WriteAllText(@"..\..\..\WebApiToTypeScript.TestWebApp\Scripts\GeneratedTypeScript\Api.ts", typeScriptCode);
        }
    }
}
