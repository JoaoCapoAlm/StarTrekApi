using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Configurations
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            string[] supportedCultures = Infra.SupportedCultures();
            var listLanguage = new List<IOpenApiAny>();
            foreach (var lang in supportedCultures)
            {
                listLanguage.Add(new OpenApiString(lang));
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Required = true,
                Description = "Translation language",
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                    Enum = listLanguage
                }
            });
        }
    }
}