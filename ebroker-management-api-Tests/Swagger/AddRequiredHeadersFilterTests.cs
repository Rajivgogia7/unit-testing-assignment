using EBroker.Management.Api.RequestHeaderValidation;
using EBroker.Management.Api.Swagger;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EBroker.Management.Api.Tests.Swagger
{
    public class AddRequiredHeadersFilterTests
    {
        private readonly AddRequiredHeadersFilter _filter;

        public AddRequiredHeadersFilterTests()
        {
            _filter = new AddRequiredHeadersFilter();
        }

        [Fact]
        public void ShouldAddHeaderParamsForInternalApi()
        {
            var operation = new OpenApiOperation();
            var apiDescription = new ApiDescription
            {
                ActionDescriptor = new ActionDescriptor
                {
                    Parameters = null,
                    EndpointMetadata = new List<object> { new InternalAPIRequiredHeadersAttribute() }
                }
            };
            apiDescription.ActionDescriptor.Parameters = null;

            var context = new OperationFilterContext(apiDescription, new Mock<ISchemaGenerator>().Object, new SchemaRepository(), null);

            _filter.Apply(operation, context);

            Assert.NotNull(operation.Parameters);
            Assert.Equal(2, operation.Parameters.Count);
            Assert.Contains(operation.Parameters, p => p.Name == "correlationId" && p.Required);
            Assert.Contains(operation.Parameters, p => p.Name == "applicationId" && p.Required);
        }

        [Fact]
        public void ShouldNotAddCorreationIdHeaderParamsIfNotInternalApi()
        {
            var operation = new OpenApiOperation();
            var apiDescription = new ApiDescription
            {
                ActionDescriptor = new ActionDescriptor
                {
                    EndpointMetadata = new List<object>()
                }
            };

            var context = new OperationFilterContext(apiDescription, new Mock<ISchemaGenerator>().Object, new SchemaRepository(), null);

            _filter.Apply(operation, context);

            Assert.NotNull(operation.Parameters);
            Assert.Empty(operation.Parameters.Where(x=>x.Name.Equals("correaltionId",StringComparison.OrdinalIgnoreCase)));
        }
    }
}
