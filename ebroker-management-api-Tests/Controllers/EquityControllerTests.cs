using EBroker.Management.Api.Controllers;
using EBroker.Management.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using EBroker.Management.Application.Equities.Commands.CreateEquity;
using EBroker.Management.Application.Equities.Models;
using EBroker.Management.Application.Equities.Queries.GetEquity;
using EBroker.Management.Application.Equities.Commands.AddStock;

namespace EBroker.Management.Api.Tests.Controllers
{
    public class EquityControllerTests
    {
        private MockRepository mockRepository;
        private Mock<IMediator> mockMediator;

        public EquityControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockMediator = this.mockRepository.Create<IMediator>();
        }

        private EquityController CreateEquityController()
        {
            return new EquityController(
                this.mockMediator.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext
                    {}
                }
            };
        }

        [Fact]
        public async Task GetEquitiesAsync_WhenFound_ReturnsListOfEquities()
        {
            // Arrange
            var equityController = this.CreateEquityController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<GetEquityQuery>(), CancellationToken.None))
                .ReturnsAsync(new GetEquityResponse
                {
                    Equities = new List<EquityModel>
                    {
                        new EquityModel
                        {
                            EquityId = "b10f6c33-9e07-4687-aeed-c64369b31b1c",
                            EquityCode = "Test",
                            EquityName = "Test",
                            Quantity = 100,
                            Price = 10.10
                        }
                    }
                }); ;

            // Act
            var result = await equityController.GetEquitiesAsync();

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<GetEquityQuery>(), CancellationToken.None), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetEquitiesAsync_WhenNotFound_Returns_Exception()
        {
            // Arrange
            var equityController = this.CreateEquityController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<GetEquityQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            await Assert.ThrowsAsync<Exception>(() => equityController.GetEquitiesAsync());
        }

        [Fact]
        public async Task GetEquityAsync_WhenFound_ReturnsListOfEquities()
        {
            // Arrange
            var equityController = this.CreateEquityController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<GetEquityQuery>(), CancellationToken.None))
                .ReturnsAsync(new GetEquityResponse
                {
                    Equities = new List<EquityModel>
                    {
                        new EquityModel
                        {
                            EquityId = "b10f6c33-9e07-4687-aeed-c64369b31b1c",
                            EquityCode = "Test",
                            EquityName = "Test",
                            Quantity = 100,
                            Price = 10.10
                        }
                    }
                }); ;

            // Act
            var result = await equityController.GetEquityAsync("Test");

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<GetEquityQuery>(), CancellationToken.None), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetEquityAsync_WhenNotFound_Returns_Exception()
        {
            // Arrange
            var equityController = this.CreateEquityController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<GetEquityQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            await Assert.ThrowsAsync<Exception>(() => equityController.GetEquityAsync("Test"));
        }

        [Fact]
        public async Task AddEquityAsync_WhenCreatedSuccessfully_ReturnsEquityDetails()
        {
            // Arrange
            var equityController = this.CreateEquityController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<CreateEquityCommand>(), CancellationToken.None))
                .ReturnsAsync(new CreateEquityResponse
                {
                    EquityId = "test",
                    Status = EquityCreationStatus.VALID
                });

            // Act
            var result = await equityController.AddEquityAsync(new CreateEquityRequest());

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<CreateEquityCommand>(), CancellationToken.None), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddEquityAsync_WhenException_ThrowsException()
        {
            // Arrange
            var equityController = this.CreateEquityController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<CreateEquityCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            await Assert.ThrowsAsync<Exception>(() => equityController.AddEquityAsync(new CreateEquityRequest()));
        }

        [Fact]
        public async Task AddEquityStock_WhenCreatedSuccessfully_ReturnsEquityDetails()
        {
            // Arrange
            var equityController = this.CreateEquityController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<AddStockCommand>(), CancellationToken.None))
                .ReturnsAsync(new AddStockResponse
                {
                    EquityCode = "Test",
                    Quantity = 1,
                    Status = EquityCreationStatus.VALID
                });

            // Act
            var result = await equityController.AddStock("Test", new AddStockRequest());

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<AddStockCommand>(), CancellationToken.None), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddEquityStock_WhenException_ThrowsException()
        {
            // Arrange
            var equityController = this.CreateEquityController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<AddStockCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            await Assert.ThrowsAsync<Exception>(() => equityController.AddStock("Test",new AddStockRequest()));
        }
    }
}
