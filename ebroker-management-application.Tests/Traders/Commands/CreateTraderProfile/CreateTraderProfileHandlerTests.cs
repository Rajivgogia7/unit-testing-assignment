using MediatR;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using EBroker.Management.Domain.Trading;
using Moq;
using EBroker.Management.Application.Traders.Commands.CreateTraderProfile;
using Xunit;
using EBroker.Management.Application.Traders.Models;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;

namespace EBroker.Management.Application.Tests.Traders.Commands.CreateTraderProfile{
    public class CreateTraderProfileHandlerTests
    {
        private MockRepository mockRepository;
        private readonly Mock<IUnitOfWork<IEBrokerDbContext>> mockUnitOfWork;

        public CreateTraderProfileHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockUnitOfWork = new Mock<IUnitOfWork<IEBrokerDbContext>>();
        }

        private CreateTraderProfileHandler CreateTraderProfileHandler()
        {
            return new CreateTraderProfileHandler(
                this.mockUnitOfWork.Object
            );
        }

        //When Trader Added to the system
        [Fact]
        public async Task Handle_WhenFirstTraderAdded_ShouldGetAdded_InTheTradersList()
        {
            // Arrange
            var CreateTraderProfileHandler = this.CreateTraderProfileHandler();
            var request = new CreateTraderProfileCommand
            {
                TraderCode = "Test1",
                TraderName = "Test1",
                Funds = 1000
            };

            var TraderProfiles = new List<TraderProfile>
            {
            };

            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfiles.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await CreateTraderProfileHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.NotNull(result.TraderId);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        //When Trader Added to the system and code is already existing
        [Fact]
        public async Task Handle_WhenTraderAdded_IsAlreadyExisting_ShouldGetResponse_WithErrorMessage()
        {
            // Arrange
            var CreateTraderProfileHandler = this.CreateTraderProfileHandler();
            var request = new CreateTraderProfileCommand
            {
                TraderCode = "Test",
                TraderName = "Test",
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
            var result = await CreateTraderProfileHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(TraderAccountCreationStatus.INVALID.Code,result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        //When Trader Added Without Initializing Traders List
        [Fact]
        public async Task Handle_WhenNewTraderAdded_WithoutInitializing_TradersList_ShouldThrowException()
        {
            // Arrange
            var CreateTraderProfileHandler = this.CreateTraderProfileHandler();
            var request = new CreateTraderProfileCommand
            {
                TraderCode = "Test",
                Funds = 1000
            };

            CancellationToken cancellationToken = default;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => CreateTraderProfileHandler.Handle(request, cancellationToken));
        }
    }
}
