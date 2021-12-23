using EBroker.Management.Api.Configurations;
using EBroker.Management.Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EBroker.Management.Api.Tests.Configurations
{
    public class DatabaseSetupTests
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IServiceCollection> _serviceCollection;
        public DatabaseSetupTests()
        {
            _configuration = new Mock<IConfiguration>();
            _serviceCollection = new Mock<IServiceCollection>();
        }

        [Fact]
        public async Task ShouldThrowException_When_Service_Is_Null()
        {
            //Arrange
            ServiceCollection collection = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => collection.AddDatabaseSetup(_configuration.Object));
        }

        [Fact]
        public async Task ShouldAdd_DbContext_Service()
        {
            //Arrange
            ServiceCollection collection = new ServiceCollection();

            //Act
            collection.AddDatabaseSetup(_configuration.Object);

            //Assert
            Assert.Contains(collection, x => x.ServiceType == typeof(EBrokerDbContext));
        }
    }
}
