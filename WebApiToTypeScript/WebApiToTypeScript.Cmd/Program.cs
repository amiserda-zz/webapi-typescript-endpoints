namespace WebApiToTypeScript.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyFilename = @"..\..\..\WebApiToTypeScript.TestWebApp\bin\WebApiToTypeScript.TestWebApp.dll";

            var typeInfo = new TypeInfoExtractor().Extract(assemblyFilename);

            new CodeGenerator().Generate(typeInfo);
        }
    }
}
