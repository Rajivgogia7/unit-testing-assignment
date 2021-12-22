using EBroker.Management.Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EBroker.Management.Api.Tests.Configurations
{
    public class SwaggerSetupTests
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IServiceCollection> _serviceCollection;
        public SwaggerSetupTests()
        {
            _configuration = new Mock<IConfiguration>();
            _serviceCollection = new Mock<IServiceCollection>();
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Service_Is_Null()
        {
            ServiceCollection collection = null;
            Assert.Throws<ArgumentNullException>(() => collection.AddSwaggerSetup());
        }

        [Fact]
        public async Task Should_Add_SwaggerGen_Service()
        {
            ServiceCollection collection = new ServiceCollection();
            collection.AddSwaggerSetup();

            Assert.Contains(collection, x => x.ServiceType == typeof(ISwaggerProvider));
          
        }

        [Fact]
        public async Task Should_Throw_Exception_When_App_Is_Null()
        {
            ApplicationBuilder appBuilder = null;
            Assert.Throws<ArgumentNullException>(() => appBuilder.UseSwaggerSetup());
        }
       
    }
}
