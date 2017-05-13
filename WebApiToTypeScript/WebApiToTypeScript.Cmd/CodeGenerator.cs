using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApiToTypeScript.Cmd
{
    internal class CodeGenerator
    {
        public string Generate(TypeInfoExtractor.TypeInfo typeInfo)
        {
            var codeFile = new StringBuilder();

            foreach (var controllerTypeInfo in typeInfo.ControllerTypeInfos)
            {
                var className = BuildServiceTypeName(controllerTypeInfo.TypeName);

                codeFile
                    .AppendStartOfModuleBlock()
                    .AppendIndentation()
                    .AppendStartOfClassBlock(className);

                foreach (var endpointTypeInfo in controllerTypeInfo.EndpointTypeInfos)
                {
                    var functionName = ToCamelCase(endpointTypeInfo.Name);
                    var returnType = MapDotNetToTypeScriptType(endpointTypeInfo.ReturnType);

                    var parametersCode = new StringBuilder();
                    foreach (var parameterInfo in endpointTypeInfo.Parameters)
                    {
                        var parameterName = ToCamelCase(parameterInfo.Name);
                        var parameterType = MapDotNetToTypeScriptType(parameterInfo.Type);

                        parametersCode.AppendFunctionParameter(parameterName, parameterType);
                    }

                    codeFile
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendFunctionName(functionName)
                        .Append($" (")
                        .Append(RemoveTrailingParametersDelimiter(parametersCode))
                        .Append(")")
                        .AppendReturnType(returnType)
                        .AppendFunctionBlockStart()
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendAjaxRequestWithPromiseResolver(endpointTypeInfo.Name, MapDotNetToTypeScriptType(endpointTypeInfo.ReturnType), endpointTypeInfo.Parameters)
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendBlockEnd();
                }

                codeFile
                    .AppendIndentation()
                    .AppendBlockEnd()
                    .AppendBlockEnd();
            }
            return codeFile.ToString();
        }

        private static string RemoveTrailingParametersDelimiter(StringBuilder parametersCode)
        {
            var s = parametersCode.ToString();
            var lastIndexOfParametersDelimiter = s.LastIndexOf(", ");

            return lastIndexOfParametersDelimiter == -1
                ? s
                : s.Remove(lastIndexOfParametersDelimiter);
        }

        private static string BuildServiceTypeName(string typeName)
        {
            var typeNameWithoutControllerPostfix = typeName.EndsWith("Controller")
                ? typeName.Remove(typeName.Length - "Controller".Length)
                : typeName;

            return $"{typeNameWithoutControllerPostfix}Service";
        }

        private static string ToCamelCase(string s) => char.ToLowerInvariant(s[0]) + s.Substring(1);

        private static string MapDotNetToTypeScriptType(string type)
        {
            if (type == "Boolean") return "boolean";
            if (type == "Byte") return "number";
            if (type == "Char") return "string";
            if (type == "Decimal") return "number";
            if (type == "Double") return "number";
            if (type == "Float") return "number";
            if (type == "Int16") return "number";
            if (type == "Int32") return "number";
            if (type == "Int64") return "number";
            if (type == "SByte") return "number";
            if (type == "UInt16") return "number";
            if (type == "UInt32") return "number";
            if (type == "UInt64") return "number";
            if (type == "String") return "string";
            if (type == "DateTime") return "Date";

            return "any";
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

        internal static StringBuilder AppendStartOfModuleBlock(this StringBuilder codeFile) => codeFile.AppendLine("module Api {");
        internal static StringBuilder AppendStartOfClassBlock(this StringBuilder codeFile, string className) => codeFile.AppendLine($"export class {className} {{{Environment.NewLine}");

        internal static StringBuilder AppendAjaxRequestWithPromiseResolver(this StringBuilder codeFile, 
            string endpointName,
            string returnType,
            IEnumerable<TypeInfoExtractor.TypeInfo.ControllerTypeInfo.EndpointTypeInfo.ParameterInfo> parameters)
        {
            var ajaxRequestParams = string.Join(", ", parameters.Select(p => p.Name));

            return codeFile.AppendLine($"return new Promise<{returnType}>((resolve) => resolve($.get('/api/testapi/{endpointName}', {{{ajaxRequestParams}}})));");
        }
    }
}