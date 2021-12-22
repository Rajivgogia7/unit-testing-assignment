using EBroker.Management.Api.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Xunit;

namespace EBroker.Management.Api.Tests.Swagger
{
    public class ReplaceVersionWithExactValueInPathTests
    {
        private readonly Mock<ISchemaGenerator> _mockSchemaGen;
        private readonly ReplaceVersionWithExactValueInPath _filter;

        public ReplaceVersionWithExactValueInPathTests()
        {
            _mockSchemaGen = new Mock<ISchemaGenerator>();
            _filter = new ReplaceVersionWithExactValueInPath();
        }

        [Fact]
        public void ShouldReplaceVerionFromAllPathsOnApply()
        {
            var repo = new SchemaRepository();
            var context = new DocumentFilterContext(Enumerable.Empty<ApiDescription>(), _mockSchemaGen.Object, repo);
            var doc = new OpenApiDocument
            {
                Info = new OpenApiInfo { Version = "v1" },
                Paths = new OpenApiPaths()
            };

            doc.Paths.Add("/v{version}/aa", new OpenApiPathItem());
            doc.Paths.Add("/v{version}/bb", new OpenApiPathItem());

            _filter.Apply(doc, context);

            Assert.Equal(2, doc.Paths.Count);
            Assert.NotNull(doc.Paths["/v1/aa"]);
            Assert.NotNull(doc.Paths["/v1/bb"]);
        }
    }
}
