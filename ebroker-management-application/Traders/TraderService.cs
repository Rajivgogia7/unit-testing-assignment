using AutoMapper;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System.Threading.Tasks;
using EBroker.Management.Application.Traders.Models;
using EBroker.Management.Domain.Trading;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Traders
{
    [ExcludeFromCodeCoverage]
    public class TraderService : ITraderService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;
        public TraderService(IMapper mapper, IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork)
        {
            _mapper = mapper;
            _tradingUnitOfWork = tradingUnitOfWork;
        }

        public async Task<TraderDetails> GetTraderDetails(string tradercode)
        {
            var traderProfile = await _tradingUnitOfWork.GetRepository<TraderProfile>().GetAll().FirstOrDefaultAsync(x => x.TraderCode == tradercode);
            return _mapper.Map<TraderDetails>(traderProfile);
        }
    }
}
