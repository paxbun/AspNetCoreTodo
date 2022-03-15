using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCoreTodo;

public class RequiredPropertiesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        NullabilityInfoContext nullability = new();
        foreach (var (propertyName, propertySchema) in schema.Properties)
        {
            if (propertyName is null)
                continue;

            var pascalPropertyName = char.ToUpper(propertyName[0]) + propertyName[1..];
            var property = context.Type.GetProperty(pascalPropertyName);
            if (property is null)
                continue;

            if (nullability.Create(property).ReadState != NullabilityState.Nullable)
            {
                schema.Required.Add(propertyName);
                propertySchema.Nullable = false;
            }
            else
            {
                propertySchema.Nullable = true;
            }
        }
    }
}