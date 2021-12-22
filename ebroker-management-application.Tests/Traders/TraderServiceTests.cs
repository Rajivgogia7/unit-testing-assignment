using EBroker.Management.Application.Equities;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using EBroker.Management.Domain.Equity;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EBroker.Management.Application.Tests.EquityServiceTests
{
    public class TraderServiceTests
    {
        private MockRepository _mockRepository;

        private Mock<IUnitOfWork<IEBrokerDbContext>> _mockUnitOfWork;

        public TraderServiceTests()
        {
            this._mockRepository = new MockRepository(MockBehavior.Default);
            this._mockUnitOfWork = this._mockRepository.Create<IUnitOfWork<IEBrokerDbContext>>();
        }

        private EquityService CreateEquityService()
        {
            return new EquityService(
                this._mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetEquityDetailsByCode_WhenFound_ShouldReturnResults()
        {
            // Arrange
            var service = this.CreateEquityService();
           
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
            var result = await service.GetEquityDetailsByCode("Test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.EquityCode);
        }

        [Fact]
        public async Task GetEquityDetailsById_WhenFound_ShouldReturnResults()
        {
            // Arrange
            var service = this.CreateEquityService();

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
            var result = await service.GetEquityDetailsById("sys_guid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.EquityCode);
        }
    }
}
