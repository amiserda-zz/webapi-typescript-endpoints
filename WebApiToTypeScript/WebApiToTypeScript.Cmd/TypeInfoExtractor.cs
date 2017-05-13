using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApiToTypeScript.Cmd
{
    internal class TypeInfoExtractor
    {
        internal TypeInfo Extract(string assemblyFilename)
        {
            var typeInfo = new TypeInfo();

            var assembly = GetAssembly(assemblyFilename);
            var controllerTypes = GetControllerTypes(assembly);

            typeInfo.ControllerTypeInfos = MapToControllerTypeInfos(controllerTypes);

            return typeInfo;
        }

        private static Assembly GetAssembly(string filename) => Assembly.LoadFrom(filename);
        private static IEnumerable<Type> GetControllerTypes(Assembly assembly) => assembly.GetTypes().Where(t => t.Name.EndsWith("ApiController"));

        private static IEnumerable<TypeInfo.ControllerTypeInfo> MapToControllerTypeInfos(IEnumerable<Type> controllerTypes)
        {
            var controllerTypeInfos = new List<TypeInfo.ControllerTypeInfo>();

            foreach (var controllerType in controllerTypes)
            {
                var publicMethodInfos = GetPublicMethods(controllerType);

                controllerTypeInfos.Add(new TypeInfo.ControllerTypeInfo
                {
                    TypeName = controllerType.Name,
                    EndpointTypeInfos = MapToEndpointTypeInfos(publicMethodInfos)
                });
            }
            return controllerTypeInfos;
        }

        private static IEnumerable<MethodInfo> GetPublicMethods(Type type)
        {
            return type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod)
                .Where(x => x.Name.StartsWith("Get1"));
        }

        private static IEnumerable<TypeInfo.ControllerTypeInfo.EndpointTypeInfo> MapToEndpointTypeInfos(IEnumerable<MethodInfo> methodInfos)
        {
            var endpointTypeInfos = new List<TypeInfo.ControllerTypeInfo.EndpointTypeInfo>();

            foreach (var methodInfo in methodInfos)
            {
                endpointTypeInfos.Add(new TypeInfo.ControllerTypeInfo.EndpointTypeInfo
                {
                    Name = methodInfo.Name,
                    ReturnType = methodInfo.ReturnType.Name,
                    Parameters = MapToParameterInfos(methodInfo.GetParameters())
                });
            }
            return endpointTypeInfos;
        }

        private static IEnumerable<TypeInfo.ControllerTypeInfo.EndpointTypeInfo.ParameterInfo> MapToParameterInfos(ParameterInfo[] methodParameters)
        {
            return methodParameters.Select(x => new TypeInfo.ControllerTypeInfo.EndpointTypeInfo.ParameterInfo
            {
                Name = x.Name,
                Type = x.ParameterType.Name
            });
        }

        internal class TypeInfo
        {
            public IEnumerable<ControllerTypeInfo> ControllerTypeInfos { get; set; }

            internal class ControllerTypeInfo
            {
                public string TypeName { get; set; }
                public IEnumerable<EndpointTypeInfo> EndpointTypeInfos { get; set; }

                internal class EndpointTypeInfo
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