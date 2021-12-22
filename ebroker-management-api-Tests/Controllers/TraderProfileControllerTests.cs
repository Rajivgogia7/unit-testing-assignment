using EBroker.Management.Api.Controllers;
using EBroker.Management.Api.Models;
using EBroker.Management.Application.Traders.Models;
using EBroker.Management.Application.Traders.Commands.CreateTraderProfile;
using EBroker.Management.Application.Traders.Queries.GetTraders;
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
using EBroker.Management.Application.Traders.Commands.SellEquity;
using EBroker.Management.Application.Traders.Commands.BuyEquity;

namespace EBroker.Management.Api.Tests.Controllers
{
    public class TraderProfileControllerTests
    {
        private MockRepository mockRepository;
        private Mock<IMediator> mockMediator;
        private TraderController _controller;

        public TraderProfileControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockMediator = this.mockRepository.Create<IMediator>();
        }

        private TraderController CreateTraderProfilesController()
        {
            _controller =  new TraderController(
                this.mockMediator.Object)
                    {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext
                    {
                    }
                }
            };

            _controller.Request.Headers.Add("userId", new StringValues("1"));
            _controller.Request.Headers.Add("correlationId", new StringValues("correlationId"));

            return _controller;
        }

        [Fact]
        public async Task GetTraderProfilesAsync_WhenFound_ReturnsListOfProfiles()
        {
            // Arrange
            var traderProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<GetTraderProfilesQuery>(), CancellationToken.None))
                .ReturnsAsync(new GetTraderProfilesResponse
                {
                    Traders = new List<TraderProfileModel>
                    {
                        new TraderProfileModel
                        {
                            TraderId = "b10f6c33-9e07-4687-aeed-c64369b31b1c",
                            TraderCode = "Test",
                            TraderName = "Test",
                            Funds = 100,
                            EquityDetails = new List<TraderEquityDetailsModel>()
                        }
                    }
                }); ;

            // Act
            var result = await traderProfilesController.GetTraderProfilesAsync();

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<GetTraderProfilesQuery>(), CancellationToken.None), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTraderProfilesAsync_WhenNotFound_Throws_Exception()
        {
            // Arrange
            var countryProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<GetTraderProfilesQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            await Assert.ThrowsAsync<Exception>(() => countryProfilesController.GetTraderProfilesAsync());
        }

        [Fact]
        public async Task GetTraderProfilesDetailsAsync_WhenFound_ReturnsListOfProfiles()
        {
            // Arrange
            var traderProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<GetTraderProfilesQuery>(), CancellationToken.None))
                .ReturnsAsync(new GetTraderProfilesResponse
                {
                    Traders = new List<TraderProfileModel>
                    {
                        new TraderProfileModel
                        {
                            TraderId = "b10f6c33-9e07-4687-aeed-c64369b31b1c",
                            TraderCode = "Test",
                            TraderName = "Test",
                            Funds = 100,
                            EquityDetails = new List<TraderEquityDetailsModel>()
                        }
                    }
                }); ;

            // Act
            var result = await traderProfilesController.GetTraderProfilesDetailsAsync("Test");

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<GetTraderProfilesQuery>(), CancellationToken.None), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTraderProfilesDetailsAsync_WhenNotFound_Throws_Exception()
        {
            // Arrange
            var countryProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<GetTraderProfilesQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            await Assert.ThrowsAsync<Exception>(() => countryProfilesController.GetTraderProfilesDetailsAsync("Test"));
        }

        [Fact]
        public async Task CreateTraderProfileAsync_WhenCreatedSuccessfully_ReturnProfileDetails()
        {
            // Arrange
            var traderProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<CreateTraderProfileCommand>(), CancellationToken.None))
                .ReturnsAsync(new CreateTraderProfileResponse
                {
                    TraderId = "test",
                    Status = TraderAccountCreationStatus.VALID
                });

            // Act
            var result = await traderProfilesController.CreateTraderProfileAsync(new CreateTraderProfileRequest());

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<CreateTraderProfileCommand>(), CancellationToken.None), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateTraderProfileAsync_WhenException_ThrowsException()
        {
            // Arrange
            var traderProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<CreateTraderProfileCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            await Assert.ThrowsAsync<Exception>(() => traderProfilesController.CreateTraderProfileAsync(new CreateTraderProfileRequest()));
        }

        [Fact]
        public async Task BuyEquityAsync_WhenPurchasedSuccessfully_ReturnsEquityDetails()
        {
            // Arrange
            var traderProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<BuyEquityCommand>(), CancellationToken.None))
                .ReturnsAsync(new BuyEquityResponse
                {
                    EquityCode = "Test",
                    Quantity = 10,
                    Status = TradingStatus.SUCCESS
                }); ;

            // Act
            var result = await traderProfilesController.BuyEquityAsync("Test",new BuyEquityRequest());

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<BuyEquityCommand>(), CancellationToken.None), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task BuyEquityAsync_WhenException_ThrowsException()
        {
            // Arrange
            var traderProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<BuyEquityCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            await Assert.ThrowsAsync<Exception>(() => traderProfilesController.BuyEquityAsync("Test",new BuyEquityRequest()));
        }

        [Fact]
        public async Task SellEquityAsyncAsync_WhenSoldSuccessfully_ReturnsProfiles()
        {
            // Arrange
            var traderProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<SellEquityCommand>(), CancellationToken.None))
                .ReturnsAsync(new SellEquityResponse
                {
                    EquityCode = "Test",
                    Quantity = 10,
                    Status = TradingStatus.SUCCESS
                });

            // Act
            var result = await traderProfilesController.SellEquityAsync("Test", new SellEquityRequest());

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<SellEquityCommand>(), CancellationToken.None), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SellEquityAsync_WhenException_ThrowsException()
        {
            // Arrange
            var traderProfilesController = this.CreateTraderProfilesController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<SellEquityCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            await Assert.ThrowsAsync<Exception>(() => traderProfilesController.SellEquityAsync("Test",new SellEquityRequest()));
        }
    }
}
