using EBroker.Management.Api.Controllers;
using EBroker.Management.Api.Models;
using EBroker.Management.Application.Traders.Commands.AddFunds;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EBroker.Management.Api.Tests.Controllers
{
    public class FundsControllerTests
    {
        private MockRepository mockRepository;
        private Mock<IMediator> mockMediator;
        private FundsController _controller;

        public FundsControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockMediator = this.mockRepository.Create<IMediator>();
        }

        private FundsController CreateFundsController()
        {
            _controller =  new FundsController(
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
        public async Task AddFundsAsync_WhenAddedSuccessfully_ReturnsUpdatedBalance()
        {
            // Arrange
            var traderProfilesController = this.CreateFundsController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<AddFundsCommand>(), CancellationToken.None))
                .ReturnsAsync(new AddFundsResponse
                {
                    TraderCode = "test",
                    Funds = 10
                });

            var addTransferFundsRequest = new AddTraderFundsRequest { Funds = 10 };
            var expectedResult = new AddFundsResponse
            {
                TraderCode = "test",
                Funds = 10
            };

            // Act
            var result = await traderProfilesController.AddFundsAsync("test", addTransferFundsRequest);

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<AddFundsCommand>(), CancellationToken.None), Times.Once);
            Assert.Equal(expectedResult.Funds, result.Funds);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddFundsAsync_WhenAddedMoreThanOneLakh_FundsAddedAfterDeductingCharges()
        {
            // Arrange
            var traderProfilesController = this.CreateFundsController();
            this.mockMediator.Setup(x => x.Send(It.IsAny<AddFundsCommand>(), CancellationToken.None))
                .ReturnsAsync(new AddFundsResponse
                {
                    TraderCode = "test",
                    Funds = 99950
                });

            var addTransferFundsRequest = new AddTraderFundsRequest { Funds = 100000 };

            var expectedResult = new AddFundsResponse
            {
                TraderCode = "test",
                Funds = 99950
            };

            // Act
            var result = await traderProfilesController.AddFundsAsync("test", addTransferFundsRequest);

            // Assert
            this.mockMediator.Verify(x => x.Send(It.IsAny<AddFundsCommand>(), CancellationToken.None), Times.Once);
            Assert.Equal(expectedResult.Funds, result.Funds);
            Assert.NotNull(result);
        }
    }
}
