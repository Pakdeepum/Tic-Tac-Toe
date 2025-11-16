using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tictactoe.Service
{
    public class SwaggerModelFormatFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if(context.Type == typeof(TimeSpan) || context.Type == typeof(TimeSpan?))
            {
                schema.Example = new OpenApiString(new TimeSpan().ToString());
            }
            else if(context.Type == typeof(object))
            {
                schema.Example = new OpenApiNull();
            }
        }
    }
}
