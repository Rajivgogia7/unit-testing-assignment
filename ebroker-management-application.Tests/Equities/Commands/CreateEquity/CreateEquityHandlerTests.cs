using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using EBroker.Management.Application.Equities.Commands.CreateEquity;
using Xunit;
using EBroker.Management.Domain.Equity;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;
using EBroker.Management.Application.Equities.Models;
using System;

namespace EBroker.Management.Application.Tests.Equities.Command.CreateEquity{
    public class CreateEquityHandlerTests
    {
        private MockRepository mockRepository;
        private readonly Mock<IUnitOfWork<IEBrokerDbContext>> mockUnitOfWork;

        public CreateEquityHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockUnitOfWork = new Mock<IUnitOfWork<IEBrokerDbContext>>();
        }

        private CreateEquityHandler CreateEquityHandler()
        {
            return new CreateEquityHandler(
                this.mockUnitOfWork.Object
            );
        }

        //When New Equity Is Added With Valid Equity Code
        [Fact]
        public async Task Handle_WhenNewEquityIsAdded_ShouldAdd_InExistingEquityList()
        {
            // Arrange

            var CreateEquityHandler = this.CreateEquityHandler();
            var request = new CreateEquityCommand
            {
                EquityCode = "Test1",
                EquityName = "Test1",
                Quantity = 10,
                Price = 10
            };

            var Equities = new List<Equity>
            {
                new Equity
                {
                    EquityId = "sys_guid",
                    EquityCode = "Test",
                    EquityName = "Test",
                    Quantity = 10,
                    Price = 10
                }
            };

            this.mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equities.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await CreateEquityHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.NotNull(result.EquityId);
            Assert.Equal(AddStockStatus.VALID.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        //When New Equity Is Added With Existing Equity Code
        [Fact]
        public async Task Handle_WhenNewEquityIsAdded_WithExistingEquityCode_ShouldReturn_ResultWithErrorMessage()
        {
            // Arrange

            var CreateEquityHandler = this.CreateEquityHandler();
            var request = new CreateEquityCommand
            {
                EquityCode = "Test",
                EquityName = "Test",
                Quantity = 10,
                Price = 10
            };

            var Equities = new List<Equity>
            {
                new Equity
                {
                    EquityId = "sys_guid",
                    EquityCode = "Test",
                    EquityName = "Test",
                    Quantity = 10,
                    Price = 10
                }
            };

            this.mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equities.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await CreateEquityHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Null(result.EquityId);
            Assert.Equal(AddStockStatus.INVALID.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        //When New Equity Is Added With Valid Equity Code Without Initializing Equity List
        [Fact]
        public async Task Handle_WhenNewEquityIsAdded_WithoutInitializing_EquityList_ShouldThrow_Exception()
        {
            // Arrange

            var CreateEquityHandler = this.CreateEquityHandler();
            var request = new CreateEquityCommand
            {
                EquityCode = "Test",
                Quantity = 100
            };

            CancellationToken cancellationToken = default;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => CreateEquityHandler.Handle(request, cancellationToken));
        }
    }
}
