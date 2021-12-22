using EBroker.Management.Api.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Xunit;

namespace EBroker.Management.Api.Tests.Swagger
{
    public class RemoveVersionFromParameterTests
    {
        private readonly RemoveVersionFromParameter _filter;

        public RemoveVersionFromParameterTests()
        {
            _filter = new RemoveVersionFromParameter();
        }

        [Fact]
        public void ShouldRemoveVersionParameterFromOperationOnApply()
        {
            var operation = new OpenApiOperation()
            {
                Parameters = new List<OpenApiParameter>
            {
                new OpenApiParameter { Name="version"} }
            };
            var context = new OperationFilterContext(new ApiDescription(), new Mock<ISchemaGenerator>().Object, new SchemaRepository(), null);

            _filter.Apply(operation, context);

            Assert.Empty(operation.Parameters);
        }
    }
}
