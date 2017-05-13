using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApiToTypeScript.Cmd.Mapper;

namespace WebApiToTypeScript.Cmd.Generator
{
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
            IEnumerable<ParameterInfo> parameters)
        {
            var ajaxRequestParams = string.Join(", ", parameters.Select(p => p.Name));

            return codeFile.AppendLine($"return new Promise<{returnType}>((resolve) => resolve($.get('/api/testapi/{endpointName}', {{{ajaxRequestParams}}})));");
        }
    }
}