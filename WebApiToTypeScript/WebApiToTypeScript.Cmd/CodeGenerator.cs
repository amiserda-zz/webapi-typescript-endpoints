using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebApiToTypeScript.Cmd
{
    class CodeGenerator
    {
        public void Generate(IEnumerable<Type> controllerTypes)
        {
            foreach (var controllerType in controllerTypes)
            {
                var codeFile = new StringBuilder();

                codeFile.AppendStartOfModuleBlock()
                    .AppendIndentation().AppendStartOfClassBlock(BuildServiceTypeName(controllerType.Name));

                foreach (var publicMethodInfo in GetPublicMethods(controllerType))
                {
                    codeFile.AppendIndentation().AppendIndentation().AppendFunctionName(BuildFunctionName(publicMethodInfo.Name)).AppendReturnType(ConvertType(publicMethodInfo.ReturnType)).AppendFunctionBlockStart()
                    .AppendIndentation().AppendIndentation().AppendIndentation().AppendAjaxRequestWithPromiseResolver()
                    .AppendIndentation().AppendIndentation().AppendBlockEnd();
                }

                codeFile.AppendIndentation().AppendBlockEnd()
                    .AppendBlockEnd();

                File.WriteAllText("Api.ts", codeFile.ToString());
            }
        }

        private static IEnumerable<MethodInfo> GetPublicMethods(Type controllerType)
        {
            return controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod).Where(x => x.Name == "Get");
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

        private static string ConvertType(Type type)
        {
            if (type == typeof(string)) return "string";

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
        internal static StringBuilder AppendAjaxRequestWithPromiseResolver(this StringBuilder codeFile) => codeFile.AppendLine("return new Promise<string>((resolve, reject) => resolve('test'));");
        internal static StringBuilder AppendFunctionBlockEnd(this StringBuilder codeFile) => codeFile.AppendLine("});");
    }
}