using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Tictactoe.Service
{
    public class SwaggerExcludeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters()
           .SelectMany(p => p.ParameterType.GetProperties()
                            .Where(prop => prop.GetCustomAttribute<JsonIgnoreAttribute>() != null || prop.GetCustomAttribute<SwaggerIgnoreAttribute>() != null));
            if (ignoredProperties.Any())
            {
                foreach (var property in ignoredProperties)
                {
                    if (property.PropertyType.IsClass)
                    {
                        operation.Parameters = operation.Parameters
                            .Where(p => !Regex.IsMatch(p.Name, $"{property.Name}\\."))
                            .ToList();
                    }
                    else
                    {
                        operation.Parameters = operation.Parameters
                            .Where(p => !p.Name.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase))
                            .ToList();
                    }

                }
            }
        }
    }
}
