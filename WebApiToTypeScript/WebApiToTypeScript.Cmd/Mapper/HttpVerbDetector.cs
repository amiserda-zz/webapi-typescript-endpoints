using System;
using System.Collections.Generic;
using System.Linq;
using WebApiToTypeScript.Cmd.Reflector;

namespace WebApiToTypeScript.Cmd.Mapper
{
    internal class HttpVerbDetector
    {
        public static string Detect(EndpointTypeInfo endpointTypeInfo)
        {
            var attributeVerbMappings = new Dictionary<string, string>
            {
                {"HttpGet", HttpVerbs.Get},
                {"HttpPost", HttpVerbs.Post},
                {"HttpPut", HttpVerbs.Put},
                {"HttpPatch", HttpVerbs.Patch},
                {"HttpDelete", HttpVerbs.Delete}
            };

            var endpointNamePrefixes = new Dictionary<string, string>
            {
                {"Get", HttpVerbs.Get},
                {"Post", HttpVerbs.Post},
                {"Put", HttpVerbs.Put},
                {"Patch", HttpVerbs.Patch},
                {"Delete", HttpVerbs.Delete}
            };

            var containsMappingAttribute = attributeVerbMappings.Keys.Intersect(endpointTypeInfo.Attributes).Any();
            var hasConventionalNamePrefix = endpointNamePrefixes.Any(x => endpointTypeInfo.Name.StartsWith(x.Key));

            if (containsMappingAttribute)
            {
                var attribute = attributeVerbMappings.Keys.Intersect(endpointTypeInfo.Attributes).First();
                return attributeVerbMappings[attribute];
            }
            if (hasConventionalNamePrefix)
            {
                var matchingPrefix = endpointNamePrefixes.First(x => endpointTypeInfo.Name.StartsWith(x.Key)).Key;
                return endpointNamePrefixes[matchingPrefix];
            }

            throw new ApplicationException($"Can't detect Http verb on endpoint: {endpointTypeInfo.Name}");
        }
    }
}