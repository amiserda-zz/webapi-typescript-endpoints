using System.Text;
using WebApiToTypeScript.Cmd.Mapper;

namespace WebApiToTypeScript.Cmd.Generator
{
    internal class CodeGenerator
    {
        public static string Generate(TypeScriptArtefacts typeScriptArtefacts)
        {
            var codeFile = new StringBuilder().AppendStartOfModuleBlock(typeScriptArtefacts.Module);

            foreach (var service in typeScriptArtefacts.Services)
            {
                codeFile
                    .AppendIndentation()
                    .AppendStartOfClassBlock(service.Name);

                foreach (var function in service.Functions)
                {
                    var parametersCode = new StringBuilder();
                    foreach (var parameter in function.Parameters)
                    {
                        parametersCode.AppendFunctionParameter(parameter.Name, parameter.Type);
                    }

                    codeFile
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendFunctionName(function.Name)
                        .Append(" (")
                        .Append(RemoveTrailingParametersDelimiter(parametersCode))
                        .Append(")")
                        .AppendReturnType(function.ReturnType)
                        .AppendFunctionBlockStart()
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendAjaxRequestWithPromiseResolver(function.ReturnType, function.HttpVerb, function.Parameters)
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendBlockEnd();
                }

                codeFile
                    .AppendIndentation()
                    .AppendBlockEnd();
            }
            return 
                codeFile
                .AppendBlockEnd()
                .ToString();
        }

        private static string RemoveTrailingParametersDelimiter(StringBuilder parametersCode)
        {
            var s = parametersCode.ToString();
            var lastIndexOfParametersDelimiter = s.LastIndexOf(", ");

            return lastIndexOfParametersDelimiter == -1
                ? s
                : s.Remove(lastIndexOfParametersDelimiter);
        }
    }
}