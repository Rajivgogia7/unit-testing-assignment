using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using EBroker.Management.Domain.Equity;
using EBroker.Management.Domain.Trading;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EBroker.Management.Data.Tests
{
    public class RepositoryTests
    {
        public Repository<Equity> _repository;
        public Repository<TraderProfile> _traderProfileRepository;
        public Repository<TraderEquity> _traderEquityRepository;
        public EBrokerDbContext _context;

        #region Equity Repository
        private Repository<Equity> Seed()
        {
            var options = new DbContextOptionsBuilder<EBrokerDbContext>()
                .UseInMemoryDatabase(databaseName: "EBrokerManagementSystemDB")
                .Options;

            //Equities
            var equities = new List<Equity>()
                {
                    new Equity()
                    {
                        EquityId = "sys_guid",
                        EquityCode = "Test",
                        EquityName = "Test",
                        Quantity = 100,
                        Price = 10

                    },
                    new Equity()
                    {
                        EquityId = "sys_guid1",
                        EquityCode = "Test1",
                        EquityName = "Test1",
                        Quantity = 100,
                        Price = 10

                    },
                    new Equity()
                    {
                        EquityId = "sys_guid2",
                        EquityCode = "Test2",
                        EquityName = "Test2",
                        Quantity = 100,
                        Price = 10

                    },
                    new Equity()
                    {
                        EquityId = "sys_guid3",
                        EquityCode = "Test3",
                        EquityName = "Test3",
                        Quantity = 100,
                        Price = 10

                    },
                };

            _context = new EBrokerDbContext(options);
            _context.Equities.RemoveRange(_context.Equities);
            _context.Equities.AddRange(equities);
            _context.SaveChanges();
            _repository = new Repository<Equity>(_context);
            return _repository;
        }

        [Fact]
        public void When_GetAll_Then_Return_All_Equities()
        {
            Seed();
            var result = _repository.GetAll();
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task When_GetAllAsync_Then_Return_All_Equities()
        {
            Seed();
            var result = await _repository.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void When_Add_Then_Return_Added_Equity()
        {
            Seed();
            var result = _repository.Add(new Equity()
            {
                EquityId = "sys_guid4",
                EquityCode = "Test5",
                EquityName = "Test5",
                Quantity = 100,
                Price = 10
            });
            Assert.NotNull(result);
        }

        //Traders 
        #endregion

        #region Traders Repository
        private Repository<TraderProfile> SeedTraders()
        {
            var options = new DbContextOptionsBuilder<EBrokerDbContext>()
                .UseInMemoryDatabase(databaseName: "EBrokerManagementSystemDB")
                .Options;

            //Traders
            var traders = new List<TraderProfile>()
                {
                    new TraderProfile()
                    {
                        TraderId = "sys_guid1",
                        TraderCode = "Test1",
                        TraderName = "Test1",
                        Funds = 100
                    },
                    new TraderProfile()
                    {
                        TraderId = "sys_guid2",
                        TraderCode = "Test2",
                        TraderName = "Test2",
                        Funds = 100
                    },
                    new TraderProfile()
                    {
                        TraderId = "sys_guid3",
                        TraderCode = "Test3",
                        TraderName = "Test3",
                        Funds = 100
                    },
                    new TraderProfile()
                    {
                        TraderId = "sys_guid4",
                        TraderCode = "Test4",
                        TraderName = "Test4",
                        Funds = 100
                    },
                };

            _context = new EBrokerDbContext(options);
            _context.Traders.RemoveRange(_context.Traders);
            _context.Traders.AddRange(traders);
            _context.SaveChanges();
            _traderProfileRepository = new Repository<TraderProfile>(_context);
            return _traderProfileRepository;
        }

        [Fact]
        public void When_GetAll_Then_Return_All_Traders()
        {
            SeedTraders();
            var result = _traderProfileRepository.GetAll();
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task When_GetAllAsync_Then_Return_All_Traders()
        {
            SeedTraders();
            var result = await _traderProfileRepository.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void When_Add_Then_Return_Added_Trader()
        {
            SeedTraders();
            var result = _traderProfileRepository.Add(new TraderProfile()
            {
                TraderId = "sys_guid5",
                TraderCode = "Test5",
                TraderName = "Test5",
                Funds = 100
            });
            Assert.NotNull(result);
        }
        #endregion

        #region TraderEquity Repository
        private Repository<TraderEquity> SeedTraderEquity()
        {
            var options = new DbContextOptionsBuilder<EBrokerDbContext>()
                .UseInMemoryDatabase(databaseName: "EBrokerManagementSystemDB")
                .Options;

            //Trader Equity
            var traderEquity = new List<TraderEquity>()
                {
                    new TraderEquity()
                    {
                        TraderEquityId = "sys_guid1",
                        TraderId = "sys_guid1",
                        EquityId = "sys_guid1",
                        Quantity = 10
                    },
                     new TraderEquity()
                    {
                         TraderEquityId = "sys_guid2",
                        TraderId = "sys_guid2",
                        EquityId = "sys_guid2",
                        Quantity = 10
                    },
                    new TraderEquity()
                    {
                        TraderEquityId = "sys_guid3",
                        TraderId = "sys_guid3",
                        EquityId = "sys_guid3",
                        Quantity = 10
                    },
                    new TraderEquity()
                    {
                        TraderEquityId = "sys_guid4",
                        TraderId = "sys_guid4",
                        EquityId = "sys_guid4",
                        Quantity = 10
                    },
                };

            _context = new EBrokerDbContext(options);
            _context.TraderEquity.RemoveRange(_context.TraderEquity);
            _context.TraderEquity.AddRange(traderEquity);
            _context.SaveChanges();
            _traderEquityRepository = new Repository<TraderEquity>(_context);
            return _traderEquityRepository;
        }

        [Fact]
        public void When_GetAll_Then_Return_All_TraderEquities()
        {
            SeedTraderEquity();
            var result = _traderEquityRepository.GetAll();
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task When_GetAllAsync_Then_Return_All_TraderEquities()
        {
            SeedTraderEquity();
            var result = await _traderEquityRepository.GetAllAsync();
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void When_Add_Then_Return_Added_TraderEquities()
        {
            SeedTraderEquity();
            var result = _traderEquityRepository.Add(new TraderEquity()
            {
                TraderEquityId = "sys_guid5",
                TraderId = "sys_guid5",
                EquityId = "Test5",
                Quantity = 100
            });
            Assert.NotNull(result);
        } 
        #endregion
    }
}
