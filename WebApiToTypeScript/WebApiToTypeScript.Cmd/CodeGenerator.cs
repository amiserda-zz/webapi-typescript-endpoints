using System;
using System.IO;
using System.Text;

namespace WebApiToTypeScript.Cmd
{
    internal class CodeGenerator
    {
        public void Generate(TypeInfoExtractor.TypeInfo typeInfo)
        {
            foreach (var controllerTypeInfo in typeInfo.ControllerTypeInfos)
            {
                var codeFile = new StringBuilder();

                codeFile
                    .AppendStartOfModuleBlock()
                    .AppendIndentation()
                    .AppendStartOfClassBlock(BuildServiceTypeName(controllerTypeInfo.TypeName));

                foreach (var endpointTypeInfo in controllerTypeInfo.EndpointTypeInfos)
                {
                    codeFile
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendFunctionName(BuildFunctionName(endpointTypeInfo.Name))
                        .AppendReturnType(ConvertType(endpointTypeInfo.ReturnType))
                        .AppendFunctionBlockStart()
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendAjaxRequestWithPromiseResolver()
                        .AppendIndentation()
                        .AppendIndentation()
                        .AppendBlockEnd();
                }

                codeFile.AppendIndentation().AppendBlockEnd()
                    .AppendBlockEnd();

                File.WriteAllText("Api.ts", codeFile.ToString());
            }
        }

        private static string BuildServiceTypeName(string typeName)
        {
            var typeNameWithoutControllerPostfix = typeName.EndsWith("Controller")
                ? typeName.Remove(typeName.Length - "Controller".Length)
                : typeName;

            return $"{typeNameWithoutControllerPostfix}Service";
        }

        private static string BuildFunctionName(string methodName)
        {
            return methodName.Substring(0,1).ToLower() + methodName.Substring(1, methodName.Length - 1);
        }

        private static string ConvertType(string type)
        {
            if (type == "String") return "string";

            return "any";
        }
    }

    static class CodeFormattingExtensions
    {
        internal static StringBuilder AppendIndentation(this StringBuilder codeFile) => codeFile.Append("    ");
        internal static StringBuilder AppendLineEnd(this StringBuilder codeFile) => codeFile.Append(Environment.NewLine);
        internal static StringBuilder AppendBlockEnd(this StringBuilder codeFile) => codeFile.AppendLine("}");

        internal static StringBuilder AppendStartOfModuleBlock(this StringBuilder codeFile) => codeFile.AppendLine("module Api {");
        internal static StringBuilder AppendStartOfClassBlock(this StringBuilder codeFile, string className) => codeFile.AppendLine($"export class {className} {{{Environment.NewLine}");
        internal static StringBuilder AppendFunctionName(this StringBuilder codeFile, string functionName) => codeFile.Append($"{functionName} = ()");
        internal static StringBuilder AppendReturnType(this StringBuilder codeFile, string typeName) => codeFile.Append($" : Promise<{typeName}>");
        internal static StringBuilder AppendFunctionBlockStart(this StringBuilder codeFile) => codeFile.AppendLine(" => {");
        internal static StringBuilder AppendAjaxRequestWithPromiseResolver(this StringBuilder codeFile) => codeFile.AppendLine("return new Promise<string>((resolve, reject) => resolve($.get('/api/testapi/get'));");
        internal static StringBuilder AppendFunctionBlockEnd(this StringBuilder codeFile) => codeFile.AppendLine("});");
    }
}