using MediatR;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;
using EBroker.Management.Domain.Trading;
using EBroker.Management.Domain.Equity;
using Moq;
using Xunit;
using EBroker.Management.Application.Traders.Models;
using System.Collections.Generic;
using System.Linq;
using MockQueryable.Moq;
using EBroker.Management.Application.Equities;
using EBroker.Management.Application.Traders.Commands.SellEquity;
using EBroker.Management.Application.Shared;

namespace EBroker.Management.Application.Tests.Traders.Commands.SellEquity{
    public class SellEquityHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private readonly Mock<IUnitOfWork<IEBrokerDbContext>> mockUnitOfWork;
        private readonly Mock<IEquityService> equityService;
        private bool _disposedValue;


        public SellEquityHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockUnitOfWork = new Mock<IUnitOfWork<IEBrokerDbContext>>();
            this.equityService = new Mock<IEquityService>();
        }

        private SellEquityHandler CreateSellEquityHandler()
        {
            return new SellEquityHandler(
                this.mockUnitOfWork.Object, this.equityService.Object
            );
        }

        [Fact]
        public async Task Handle_WhenEquitiesAreSold_WorkingHours_ShouldGetSold()
        {
            // Arrange

            var SellEquityHandler = this.CreateSellEquityHandler();

            DateTimeProvider.SetDateTimeSell(new DateTime(2021, 12, 20, 9, 30, 0));

            var request = new SellEquityCommand
            {
                TraderCode = "Test",
                EquityCode = "Test-Equity",
                Quantity = 9
            };

            var TraderProfile = new List<TraderProfile>
            {
                new TraderProfile
                {
                    TraderId = "sys_guid",
                    TraderCode = "Test",
                    TraderName = "Test",
                    Funds = 100
                }
            };

            var Equity = new List<Equity>
            {
                new Equity
                {
                    EquityId = "sys_guid",
                    EquityCode = "Test-Equity",
                    EquityName = "Test-Equity",
                    Quantity = 100,
                    Price = 10
                }
            };
            var TraderEquity = new List<TraderEquity>
            {
                new TraderEquity
                {
                    TraderEquityId = "sys_guid",
                    TraderId = "sys_guid",
                    EquityId = "sys_guid",
                    Quantity = 10
                }
            };

            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfile.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equity.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderEquity>().GetAll()).Returns(TraderEquity.AsQueryable().BuildMock().Object);


            CancellationToken cancellationToken = default;

            // Act
            var result = await SellEquityHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(TradingStatus.SUCCESS.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenEquitiesAreSoldOutOfWorkingHours_ShouldReturn_ResultWithErrorMessage()
        {
            // Arrange

            var SellEquityHandler = this.CreateSellEquityHandler();

            DateTimeProvider.SetDateTimeSell(new DateTime(2021, 12, 20, 15, 30, 0)); //3.30 PM

            var request = new SellEquityCommand
            {
                TraderCode = "Test",
                EquityCode = "Test-Equity",
                Quantity = 9
            };

            var TraderProfile = new List<TraderProfile>
            {
                new TraderProfile
                {
                    TraderId = "sys_guid",
                    TraderCode = "Test",
                    TraderName = "Test",
                    Funds = 100
                }
            };

            var Equity = new List<Equity>
            {
                new Equity
                {
                    EquityId = "sys_guid",
                    EquityCode = "Test-Equity",
                    EquityName = "Test-Equity",
                    Quantity = 100,
                    Price = 10
                }
            };

            var TraderEquity = new List<TraderEquity>
            {
                new TraderEquity
                {
                    TraderEquityId = "sys_guid",
                    TraderId = "sys_guid",
                    EquityId = "sys_guid",
                    Quantity = 10
                }
            };

            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfile.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equity.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderEquity>().GetAll()).Returns(TraderEquity.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await SellEquityHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(TradingStatus.NOT_A_VALID_TIME_OR_DAY.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenEquitiesAreSold_OnSaturdayOrSunday_ShouldReturn_ResultWithErrorMessage()
        {
            // Arrange

            var SellEquityHandler = this.CreateSellEquityHandler();

            DateTimeProvider.SetDateTimeSell(new DateTime(2021, 12, 19, 9, 30, 0)); //Sunday

            var request = new SellEquityCommand
            {
                TraderCode = "Test",
                EquityCode = "Test-Equity",
                Quantity = 10
            };

            var TraderProfile = new List<TraderProfile>
            {
                new TraderProfile
                {
                    TraderId = "sys_guid",
                    TraderCode = "Test",
                    TraderName = "Test",
                    Funds = 100
                }
            };

            var Equity = new List<Equity>
            {
                new Equity
                {
                    EquityId = "sys_guid",
                    EquityCode = "Test-Equity",
                    EquityName = "Test-Equity",
                    Quantity = 100,
                    Price = 10
                }
            };

            var TraderEquity = new List<TraderEquity>
            {
                 new TraderEquity
                {
                    TraderEquityId = "sys_guid",
                    TraderId = "sys_guid",
                    EquityId = "sys_guid",
                    Quantity = 10
                }
            };


            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfile.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equity.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderEquity>().GetAll()).Returns(TraderEquity.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await SellEquityHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(TradingStatus.NOT_A_VALID_TIME_OR_DAY.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenEquitiesAreSold_WorkingHours_EquityNotSufficient_ShouldReturn_ResultWithErrorMessage()
        {
            // Arrange

            var SellEquityHandler = this.CreateSellEquityHandler();

            DateTimeProvider.SetDateTimeSell(new DateTime(2021, 12, 21, 09, 30, 0)); // Tuesday - 9.30 AM

            var request = new SellEquityCommand
            {
                TraderCode = "Test",
                EquityCode = "Test-Equity",
                Quantity = 105
            };

            var TraderProfile = new List<TraderProfile>
            {
                new TraderProfile
                {
                    TraderId = "sys_guid",
                    TraderCode = "Test",
                    TraderName = "Test",
                    Funds = 10000
                }
            };

            var Equity = new List<Equity>
            {
                new Equity
                {
                    EquityId = "sys_guid",
                    EquityCode = "Test-Equity",
                    EquityName = "Test-Equity",
                    Quantity = 100,
                    Price = 10
                }
            };

            var TraderEquity = new List<TraderEquity>
            {
               new TraderEquity
                {
                    TraderEquityId = "sys_guid",
                    TraderId = "sys_guid",
                    EquityId = "sys_guid",
                    Quantity = 10
                }
            };


            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfile.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equity.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderEquity>().GetAll()).Returns(TraderEquity.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await SellEquityHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(TradingStatus.EQUITY_NOT_SUFFICIENT_TRADER.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenEquitiesAreSold_WorkingHours_FundsNotSufficient_ShouldReturn_ResultWithErrorMessage()
        {
            // Arrange

            var SellEquityHandler = this.CreateSellEquityHandler();

            DateTimeProvider.SetDateTimeSell(new DateTime(2021, 12, 21, 09, 30, 0)); // Tuesday - 9.30 AM

            var request = new SellEquityCommand
            {
                TraderCode = "Test",
                EquityCode = "Test-Equity",
                Quantity = 15
            };

            var TraderProfile = new List<TraderProfile>
            {
                new TraderProfile
                {
                    TraderId = "sys_guid",
                    TraderCode = "Test",
                    TraderName = "Test",
                    Funds = 10
                }
            };

            var Equity = new List<Equity>
            {
                new Equity
                {
                    EquityId = "sys_guid",
                    EquityCode = "Test-Equity",
                    EquityName = "Test-Equity",
                    Quantity = 100,
                    Price = 10
                }
            };

            var TraderEquity = new List<TraderEquity>
            {
                new TraderEquity
                {
                    TraderEquityId = "sys_guid",
                    TraderId = "sys_guid",
                    EquityId= "sys_guid",
                    Quantity = 25
                }
            };


            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfile.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equity.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderEquity>().GetAll()).Returns(TraderEquity.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await SellEquityHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(TradingStatus.FUNDS_NOT_SUFFICIENT_TRADER_SELL.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }
        [Fact]
        public async Task Handle_WhenEquitiesAreSold__WorkingHours_WrongDetailsPassed_ShouldReturn_ResultWithErrorMessage()
        {
            // Arrange

            var SellEquityHandler = this.CreateSellEquityHandler();

            DateTimeProvider.SetDateTimeSell(new DateTime(2021, 12, 21, 9, 30, 0)); // Tuesday - 9.30 AM

            var request = new SellEquityCommand
            {
                TraderCode = "Test1",
                EquityCode = "Test-Equity1",
                Quantity = 5
            };

            var TraderProfile = new List<TraderProfile>
            {
                new TraderProfile
                {
                    TraderId = "sys_guid",
                    TraderCode = "Test",
                    TraderName = "Test",
                    Funds = 100
                }
            };

            var Equity = new List<Equity>
            {
                new Equity
                {
                    EquityId = "sys_guid",
                    EquityCode = "Test-Equity",
                    EquityName = "Test-Equity",
                    Quantity = 100,
                    Price = 10
                }
            };

            var TraderEquity = new List<TraderEquity>
            {
                new TraderEquity
                {
                    TraderEquityId = "sys_guid",
                    TraderId = "sys_guid",
                    EquityId = "sys_guid",
                    Quantity = 10
                }
            };


            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderProfile>().GetAll()).Returns(TraderProfile.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equity.AsQueryable().BuildMock().Object);
            this.mockUnitOfWork.Setup(x => x.GetRepository<TraderEquity>().GetAll()).Returns(TraderEquity.AsQueryable().BuildMock().Object);

            CancellationToken cancellationToken = default;

            // Act
            var result = await SellEquityHandler.Handle(
                request,
                cancellationToken);

            //Assert
            Assert.Equal(TradingStatus.INVALID_DETAILS.Code, result.Status.Code);
            this.mockUnitOfWork.Verify(x => x.BeginTransaction(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Commit(), Times.Never);
            this.mockUnitOfWork.Verify(x => x.Rollback(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenEquities_AreSold_WithoutInitializingList_ShouldThrowException()
        {
            // Arrange

            var SellEquityHandler = this.CreateSellEquityHandler();

            var request = new SellEquityCommand
            {
                TraderCode = "Test1",
                EquityCode = "Test-Equity1",
                Quantity = 5
            };

            CancellationToken cancellationToken = default;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => SellEquityHandler.Handle(request, cancellationToken));
        }

        public void Dispose()
        {
            DateTimeProvider.ResetDateTimeSell();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
            }
        }
    }
}
