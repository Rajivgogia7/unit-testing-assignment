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
        public async Task ShouldThrowException_When_Service_Is_Null()
        {
            //Arrange
            ServiceCollection collection = null;

            //Act and assert
            Assert.Throws<ArgumentNullException>(() => collection.AddSwaggerSetup());
        }

        [Fact]
        public async Task ShouldAdd_SwaggerGen_Service()
        {
            //Arrange
            ServiceCollection collection = new ServiceCollection();

            //Act
            collection.AddSwaggerSetup();

            //Assert
            Assert.Contains(collection, x => x.ServiceType == typeof(ISwaggerProvider));
          
        }

        [Fact]
        public async Task ShouldThrowException_When_App_Is_Null()
        {
            //Arrange
            ApplicationBuilder appBuilder = null;

            //Act and assert
            Assert.Throws<ArgumentNullException>(() => appBuilder.UseSwaggerSetup());
        }
       
    }
}
