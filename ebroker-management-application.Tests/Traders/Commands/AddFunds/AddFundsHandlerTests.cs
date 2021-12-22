using MediatR;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using EBroker.Management.Domain.Trading;
using Moq;
using EBroker.Management.Application.Traders.Commands.AddFunds;
using Xunit;
using EBroker.Management.Application.Traders.Models;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;

namespace EBroker.Management.Application.Tests.Traders.Commands.AddFunds{
    public class AddFundsHandlerTests
    {
        private MockRepository mockRepository;
        private readonly Mock<IUnitOfWork<IEBrokerDbContext>> mockUnitOfWork;

        public AddFundsHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockUnitOfWork = new Mock<IUnitOfWork<IEBrokerDbContext>>();
        }

        private AddFundsHandler CreateAddFundsHandler()
        {
            return new AddFundsHandler(
                this.mockUnitOfWork.Object
            );
        }

        //When Funds Added less than 1 Lakh to valid vendor account
        [Fact]
        public async Task Handle_WhenFundsAdded_Lessthan1Lakh_ShouldAdd_InTraderAccountWithoutCharges()
        {
            // Arrange
            var AddFundsHandler = this.CreateAddFundsHandler();
            var request = new AddFundsCommand
            {
                TraderCode = "Test",
                Funds = 1000
            };

            var TraderProfiles = new List<TraderProfile>
            {
                new TraderProfile
                {
                    TraderId = "sys_guid",
                    TraderCode = "Test",
                    TraderName = "Test",
                    Funds = 100
                }
            };

            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfiles.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await AddFundsHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(1100, result.Funds);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        //When Funds Added more than 1 lakh to valid vendor account, the fund should be added after deducting charges (0.05%)
        [Fact]
        public async Task Handle_WhenFundsAdded_Morethan1Lakh_ShouldAdd_InTraderAccountWithCharges()
        {
            // Arrange

            var AddFundsHandler = this.CreateAddFundsHandler();
            var request = new AddFundsCommand
            {
                TraderCode = "Test",
                Funds = 105000
            };

            var TraderProfiles = new List<TraderProfile>
            {
                new TraderProfile
                {
                    TraderId = "sys_guid",
                    TraderCode = "Test",
                    TraderName = "Test",
                    Funds = 100
                }
            };

            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfiles.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await AddFundsHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(105047.5, result.Funds);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        //When Funds Added to Invalid Vendor Account
        [Fact]
        public async Task Handle_WhenFundsAdded_WithInvalidTraderCode_ShouldReturn_ResultWithErrorMessage()
        {
            // Arrange

            var AddFundsHandler = this.CreateAddFundsHandler();
            var request = new AddFundsCommand
            {
                TraderCode = "Test1",
                Funds = 1000
            };

            var TraderProfiles = new List<TraderProfile>
            {
                new TraderProfile
                {
                    TraderId = "sys_guid",
                    TraderCode = "Test",
                    TraderName = "Test",
                    Funds = 100
                }
            };

            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfiles.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await AddFundsHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(FundsStatus.INVALID.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        //When Funds Added Without Initializing Traders List
        [Fact]
        public async Task Handle_WhenFundsAdded_WithoutInitializing_TradersList_ShouldThrowException()
        {
            // Arrange
            var AddFundsHandler = this.CreateAddFundsHandler();
            var request = new AddFundsCommand
            {
                TraderCode = "Test",
                Funds = 1000
            };

            CancellationToken cancellationToken = default;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => AddFundsHandler.Handle(request, cancellationToken));
        }

    }
}
