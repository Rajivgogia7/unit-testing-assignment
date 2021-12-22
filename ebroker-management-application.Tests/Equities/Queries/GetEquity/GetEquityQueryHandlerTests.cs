using EBroker.Management.Application.Equities.Queries.GetEquity;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using EBroker.Management.Domain.Equity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MockQueryable.Moq;
using System;

namespace EBroker.Management.Application.Tests.Equities.Queries.GetEquity
{
    public class GetEquityQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork<IEBrokerDbContext>> _mockUnitOfWork;
        private readonly GetEquityQueryHandler _handler;

        public GetEquityQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<IEBrokerDbContext>>();
            _handler = new GetEquityQueryHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetEquity_WithEquityCode_WhenFound_ReturnsEquityDetails()
        {
            // Arrange
            var query = new GetEquityQuery
            {
                EquityCode = "Test"
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
                },
                 new Equity
                {
                    EquityId = "sys_guid1",
                    EquityCode = "Test1",
                    EquityName = "Test1",
                    Quantity = 10,
                    Price = 10
                }
            };

            this._mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equities.AsQueryable().BuildMock().Object);

            // Act
            var result = await _handler.Handle(query, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Equities[0].EquityCode);
        }

        [Fact]
        public async Task GetAllEquity_WhenFound_ReturnsAllRecords()
        {
            // Arrange
            var query = new GetEquityQuery
            {
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
                },
                 new Equity
                {
                    EquityId = "sys_guid1",
                    EquityCode = "Test1",
                    EquityName = "Test1",
                    Quantity = 10,
                    Price = 10
                }
            };

            this._mockUnitOfWork.Setup(x => x.GetRepository<Equity>().GetAll()).Returns(Equities.AsQueryable().BuildMock().Object);

            // Act
            var result = await _handler.Handle(query, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Equities.Count);
        }

        [Fact]
        public async Task Handle_WhenEquitySearched_WithoutInitializingEquityList_ShouldThrow_Exception()
        {
            // Arrange
            var query = new GetEquityQuery
            {
            };

            CancellationToken cancellationToken = default;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(query, cancellationToken));
        }
    }
}