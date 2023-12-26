#region Usings

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

#endregion

namespace Records.Shared.Infra.Swagger.Schemas;

/// <summary>
/// Swagger schama filter.
/// </summary>
public class SwaggerSchemaExampleFilter : ISchemaFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(context);

        if (context.MemberInfo != null)
        {
            SwaggerSchemaExampleAttribute? schemaAttribute = context.MemberInfo.GetCustomAttributes<SwaggerSchemaExampleAttribute>()
                .FirstOrDefault();
            if (schemaAttribute != null)
            {
                ////ApplySchemaAttribute(schema, schemaAttribute);
                schema.Example = new Microsoft.OpenApi.Any.OpenApiString(schemaAttribute.Example);
            }
        }
    }

    ////private void ApplySchemaAttribute(OpenApiSchema schema, SwaggerSchemaExampleAttribute schemaAttribute)
    ////{
    ////    if (schemaAttribute.Example != null)
    ////    {
    ////        schema.Example = new Microsoft.OpenApi.Any.OpenApiString(schemaAttribute.Example);
    ////    }
    ////}
}
