using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApiToTypeScript.Cmd
{
    internal class DotNetEndpointsTypeInfoExtractor
    {
        internal DotNetEndpointsTypeInfo Extract(string assemblyFilename)
        {
            var typeInfo = new DotNetEndpointsTypeInfo();

            var assembly = GetAssembly(assemblyFilename);
            var controllerTypes = GetControllerTypes(assembly);

            typeInfo.ControllerTypeInfos = MapToControllerTypeInfos(controllerTypes);

            return typeInfo;
        }

        private static Assembly GetAssembly(string filename) => Assembly.LoadFrom(filename);
        private static IEnumerable<Type> GetControllerTypes(Assembly assembly) => assembly.GetTypes().Where(t => t.Name.EndsWith("ApiController"));

        private static IEnumerable<DotNetEndpointsTypeInfo.ControllerTypeInfo> MapToControllerTypeInfos(IEnumerable<Type> controllerTypes)
        {
            var controllerTypeInfos = new List<DotNetEndpointsTypeInfo.ControllerTypeInfo>();

            foreach (var controllerType in controllerTypes)
            {
                var publicMethodInfos = GetPublicMethods(controllerType);

                controllerTypeInfos.Add(new DotNetEndpointsTypeInfo.ControllerTypeInfo
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
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly)
                .Where(x => x.Name.StartsWith("Get"));
        }

        private static IEnumerable<DotNetEndpointsTypeInfo.ControllerTypeInfo.EndpointTypeInfo> MapToEndpointTypeInfos(IEnumerable<MethodInfo> methodInfos)
        {
            var endpointTypeInfos = new List<DotNetEndpointsTypeInfo.ControllerTypeInfo.EndpointTypeInfo>();

            foreach (var methodInfo in methodInfos)
            {
                endpointTypeInfos.Add(new DotNetEndpointsTypeInfo.ControllerTypeInfo.EndpointTypeInfo
                {
                    Name = methodInfo.Name,
                    ReturnType = methodInfo.ReturnType.Name,
                    Parameters = MapToParameterInfos(methodInfo.GetParameters())
                });
            }
            return endpointTypeInfos;
        }

        private static IEnumerable<DotNetEndpointsTypeInfo.ControllerTypeInfo.EndpointTypeInfo.ParameterInfo> MapToParameterInfos(ParameterInfo[] methodParameters)
        {
            return methodParameters.Select(x => new DotNetEndpointsTypeInfo.ControllerTypeInfo.EndpointTypeInfo.ParameterInfo
            {
                Name = x.Name,
                Type = x.ParameterType.Name
            });
        }

        internal class DotNetEndpointsTypeInfo
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