using ClosedXML;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;

namespace Tictactoe.Service
{
    public class SwaggerEnpointDescriptionFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttr = context.MethodInfo.GetAttributes<AuthorizedAttribute>();
            var authDesciption = authAttr.Any() ? "(Auth)" : "";
            var descAtth = context.MethodInfo.GetAttributes<DescriptionAttribute>().FirstOrDefault();
            var desc = descAtth?.Description;

            operation.Summary = $"{context.MethodInfo.Name} {authDesciption} {desc}";
        }
    }
}
