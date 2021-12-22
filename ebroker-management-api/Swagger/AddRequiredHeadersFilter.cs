using EBroker.Management.Api.RequestHeaderValidation;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace EBroker.Management.Api.Swagger
{
    public class AddRequiredHeadersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor;
            if (controllerActionDescriptor.EndpointMetadata.OfType<InternalAPIRequiredHeadersAttribute>().Any())
            {
                AddBaseParams(operation);
            }
        }

        private void AddBaseParams(OpenApiOperation operation)
        {
            operation.Parameters.Add(HeaderParam("correlationId", true, "string", ""));
            operation.Parameters.Add(HeaderParam("applicationId", true, "string", ""));
        }

        public OpenApiParameter HeaderParam(string name, bool required = true, string type = "string", string description = "")
        {
            return new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = type },
                Description = description,
                Required = required
            };
        }
    }
}
