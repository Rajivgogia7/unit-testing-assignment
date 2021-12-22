using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using EBroker.Management.Application.Equities.Commands.AddStock;
using Xunit;
using EBroker.Management.Domain.Equity;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;
using EBroker.Management.Application.Equities.Models;
using System;

namespace EBroker.Management.Application.Tests.Equities.Command.AddStock{
    public class AddStockHandlerTests
    {
        private MockRepository mockRepository;
        private readonly Mock<IUnitOfWork<IEBrokerDbContext>> mockUnitOfWork;

        public AddStockHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockUnitOfWork = new Mock<IUnitOfWork<IEBrokerDbContext>>();
        }

        private AddStockHandler CreateAddStockHandler()
        {
            return new AddStockHandler(
                this.mockUnitOfWork.Object
            );
        }

        //When New Stock Is Added with valid Equity Code
        [Fact]
        public async Task Handle_WhenNewStockIsAdded_ShouldAdd_InExistingEquityRecord()
        {
            // Arrange

            var AddStockandler = this.CreateAddStockHandler();
            var request = new AddStockCommand
            {
                EquityCode = "Test",
                Quantity = 100
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
            var result = await AddStockandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(110, result.Quantity);
            Assert.Equal(AddStockStatus.VALID.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        //When New Stock Is Added with Invalid Equity Code
        [Fact]
        public async Task Handle_WhenNewStockIsAdded_WithInvalidEquityCode_ShouldReturn_ResultWithErrorMessage()
        {
            // Arrange

            var AddStockHandler = this.CreateAddStockHandler();
            var request = new AddStockCommand
            {
                EquityCode = "Test1",
                Quantity = 100
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
            var result = await AddStockHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(AddStockStatus.INVALID.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        //When New Stock Is Added without initializing Equity List
        [Fact]
        public async Task Handle_WhenNewStockIsAdded_WithoutInitializing_EquityList_ShouldThrow_Exception()
        {
            // Arrange
            var AddStockHandler = this.CreateAddStockHandler();
            var request = new AddStockCommand
            {
                EquityCode = "Test",
                Quantity = 100
            };

            CancellationToken cancellationToken = default;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => AddStockHandler.Handle(request, cancellationToken));
        }
    }
}
