using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApiToTypeScript.Cmd.Reflector
{
    internal class EndpointsInfoReflector
    {
        internal static IEnumerable<ControllerTypeInfo> Reflect(string assemblyFilename)
        {
            var assembly = GetAssembly(assemblyFilename);
            var controllerTypes = GetControllerTypes(assembly);

            return MapToControllerTypeInfos(controllerTypes);
        }

        private static Assembly GetAssembly(string filename) => Assembly.LoadFrom(filename);
        private static IEnumerable<Type> GetControllerTypes(Assembly assembly) => assembly.GetTypes().Where(t => t.Name.EndsWith("ApiController"));

        private static IEnumerable<ControllerTypeInfo> MapToControllerTypeInfos(IEnumerable<Type> controllerTypes)
        {
            var controllerTypeInfos = new List<ControllerTypeInfo>();

            foreach (var controllerType in controllerTypes)
            {
                var publicMethodInfos = GetPublicMethods(controllerType);

                controllerTypeInfos.Add(new ControllerTypeInfo
                {
                    TypeName = controllerType.Name,
                    EndpointTypeInfos = MapToEndpointTypeInfos(publicMethodInfos)
                });
            }
            return controllerTypeInfos;
        }

        private static IEnumerable<EndpointTypeInfo> MapToEndpointTypeInfos(IEnumerable<MethodInfo> methodInfos)
        {
            var endpointTypeInfos = new List<EndpointTypeInfo>();

            foreach (var methodInfo in methodInfos)
            {
                endpointTypeInfos.Add(new EndpointTypeInfo
                {
                    Name = methodInfo.Name,
                    ReturnType = methodInfo.ReturnType.Name,
                    Parameters = MapToParameterInfos(methodInfo.GetParameters())
                });
            }
            return endpointTypeInfos;
        }

        private static IEnumerable<ParameterInfo> MapToParameterInfos(System.Reflection.ParameterInfo[] methodParameters)
        {
            return methodParameters.Select(x => new ParameterInfo
            {
                Name = x.Name,
                Type = x.ParameterType.Name
            });
        }

        private static IEnumerable<MethodInfo> GetPublicMethods(Type type)
        {
            return type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly)
                .Where(x => x.Name.StartsWith("Get"));
        }
    }
}