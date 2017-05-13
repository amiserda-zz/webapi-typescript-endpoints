using System.Collections.Generic;
using System.Linq;

namespace WebApiToTypeScript.Cmd
{
    internal class DotNetEndpointsToTypeScriptArtefactsMapper
    {
        public static TypeScriptArtefacts Map(DotNetEndpointsTypeInfoExtractor.DotNetEndpointsTypeInfo dotNetEndpointsTypeInfo)
        {
            var typeScriptArtefacts = new TypeScriptArtefacts
            {
                Module = "Api",
                Services = BuildServiceClassesInfo(dotNetEndpointsTypeInfo.ControllerTypeInfos)
            };
            return typeScriptArtefacts;
        }

        private static IEnumerable<TypeScriptArtefacts.ServiceClassInfo> BuildServiceClassesInfo(IEnumerable<DotNetEndpointsTypeInfoExtractor.DotNetEndpointsTypeInfo.ControllerTypeInfo> controllerTypeInfos)
        {
            return 
                controllerTypeInfos.Select(controllerTypeInfo => new TypeScriptArtefacts.ServiceClassInfo
                {
                    Name = BuildServiceTypeName(controllerTypeInfo.TypeName),
                    Functions = BuildFunctionsInfo(controllerTypeInfo.EndpointTypeInfos)
                });
        }

        private static IEnumerable<TypeScriptArtefacts.ServiceClassInfo.FunctionInfo> BuildFunctionsInfo(IEnumerable<DotNetEndpointsTypeInfoExtractor.DotNetEndpointsTypeInfo.ControllerTypeInfo.EndpointTypeInfo> endpointTypeInfos)
        {
            return
                endpointTypeInfos.Select(endpointTypeInfo => new TypeScriptArtefacts.ServiceClassInfo.FunctionInfo
                {
                    Name = ToCamelCase(endpointTypeInfo.Name),
                    ReturnType = MapDotNetToTypeScriptType(endpointTypeInfo.ReturnType),
                    Parameters = BuildParametersInfo(endpointTypeInfo.Parameters)
                });
        }

        private static IEnumerable<TypeScriptArtefacts.ServiceClassInfo.FunctionInfo.ParameterInfo> BuildParametersInfo(IEnumerable<DotNetEndpointsTypeInfoExtractor.DotNetEndpointsTypeInfo.ControllerTypeInfo.EndpointTypeInfo.ParameterInfo> parameters)
        {
            return
                parameters.Select(parameter => new TypeScriptArtefacts.ServiceClassInfo.FunctionInfo.ParameterInfo
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

        internal class TypeScriptArtefacts
        {
            public string Module { get; set; }
            public IEnumerable<ServiceClassInfo> Services { get; set; }

            internal class ServiceClassInfo
            {
                public string Name { get; set; }
                public IEnumerable<FunctionInfo> Functions { get; set; }

                internal class FunctionInfo
                {
                    public string Name { get; set; }
                    public string ReturnType { get; set; }
                    public IEnumerable<ParameterInfo> Parameters { get; set; }

                    internal class ParameterInfo
                    {
                        public string Name { get; set; }
                        public string Type { get; set; }
                    }
                }
            }
        }
    }
}