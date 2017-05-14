using System;
using System.Collections.Generic;
using System.Linq;
using WebApiToTypeScript.Cmd.Reflector;

namespace WebApiToTypeScript.Cmd.Mapper
{
    internal class EndpointsToTypeScriptMapper
    {
        public static TypeScriptArtefacts Map(IEnumerable<ControllerTypeInfo> controllerTypeInfos)
        {
            var typeScriptArtefacts = new TypeScriptArtefacts
            {
                Module = "Api",
                Services = BuildServiceClassesInfo(controllerTypeInfos)
            };
            return typeScriptArtefacts;
        }

        private static IEnumerable<ServiceClassInfo> BuildServiceClassesInfo(IEnumerable<ControllerTypeInfo> controllerTypeInfos)
        {
            return 
                controllerTypeInfos.Select(controllerTypeInfo => new ServiceClassInfo
                {
                    Name = BuildServiceTypeName(controllerTypeInfo.TypeName),
                    Functions = BuildFunctionsInfo(controllerTypeInfo.EndpointTypeInfos)
                });
        }

        private static IEnumerable<FunctionInfo> BuildFunctionsInfo(IEnumerable<EndpointTypeInfo> endpointTypeInfos)
        {
            return
                endpointTypeInfos.Select(endpointTypeInfo => new FunctionInfo
                {
                    Name = ToCamelCase(endpointTypeInfo.Name),
                    ReturnType = MapDotNetToTypeScriptType(endpointTypeInfo.ReturnType),
                    HttpVerb = DetectHttpVerb(endpointTypeInfo),
                    Parameters = BuildParametersInfo(endpointTypeInfo.Parameters)
                });
        }

        private static string DetectHttpVerb(EndpointTypeInfo endpointTypeInfo)
        {
            if (endpointTypeInfo.Name.StartsWith("Get") || endpointTypeInfo.Attributes.Contains("HttpGet"))
                return "GET";

            return "GET";
        }

        private static IEnumerable<ParameterInfo> BuildParametersInfo(IEnumerable<Reflector.ParameterInfo> parameters)
        {
            return
                parameters.Select(parameter => new ParameterInfo
                {
                    Name = ToCamelCase(parameter.Name),
                    Type = MapDotNetToTypeScriptType(parameter.Type)
                });
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
}