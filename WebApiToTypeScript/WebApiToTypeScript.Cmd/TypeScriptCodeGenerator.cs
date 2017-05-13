using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApiToTypeScript.Cmd
{
    internal class TypeScriptCodeGenerator
    {
        public static string Generate(DotNetEndpointsToTypeScriptArtefactsMapper.TypeScriptArtefacts typeScriptArtefacts)
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
                        .Append($" (")
                        .Append(RemoveTrailingParametersDelimiter(parametersCode))
                        .Append(")")
                        .AppendReturnType(function.ReturnType)
                        .AppendFunctionBlockStart()
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendAjaxRequestWithPromiseResolver(function.Name, function.ReturnType, function.Parameters)
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

    static class CodeFormattingExtensions
    {
        internal static StringBuilder AppendIndentation(this StringBuilder codeFile) => codeFile.Append("    ");
        internal static StringBuilder AppendLineEnd(this StringBuilder codeFile) => codeFile.Append(Environment.NewLine);
        internal static StringBuilder AppendBlockEnd(this StringBuilder codeFile) => codeFile.AppendLine("}");

        internal static StringBuilder AppendFunctionBlockStart(this StringBuilder codeFile) => codeFile.AppendLine(" => {");
        internal static StringBuilder AppendFunctionName(this StringBuilder codeFile, string functionName) => codeFile.Append($"{functionName} =");
        internal static StringBuilder AppendReturnType(this StringBuilder codeFile, string typeName) => codeFile.Append($" : Promise<{typeName}>");
        internal static StringBuilder AppendFunctionBlockEnd(this StringBuilder codeFile) => codeFile.AppendLine("});");
        internal static StringBuilder AppendFunctionParameter(this StringBuilder codeFile, string name, string type) => codeFile.Append($"{name}: {type}, ");

        internal static StringBuilder AppendStartOfModuleBlock(this StringBuilder codeFile, string module) => codeFile.AppendLine($"module {module} {{");
        internal static StringBuilder AppendStartOfClassBlock(this StringBuilder codeFile, string className) => codeFile.AppendLine($"export class {className} {{{Environment.NewLine}");

        internal static StringBuilder AppendAjaxRequestWithPromiseResolver(this StringBuilder codeFile, 
            string endpointName,
            string returnType,
            IEnumerable<DotNetEndpointsToTypeScriptArtefactsMapper.TypeScriptArtefacts.ServiceClassInfo.FunctionInfo.ParameterInfo> parameters)
        {
            var ajaxRequestParams = string.Join(", ", parameters.Select(p => p.Name));

            return codeFile.AppendLine($"return new Promise<{returnType}>((resolve) => resolve($.get('/api/testapi/{endpointName}', {{{ajaxRequestParams}}})));");
        }
    }
}