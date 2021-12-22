using AutoMapper;
using EBroker.Management.Data.Context;
using EBroker.Management.Data.Repository;
using System.Threading.Tasks;
using EBroker.Management.Application.Equities.Models;
using EBroker.Management.Domain.Equity;
using Microsoft.EntityFrameworkCore;

namespace EBroker.Management.Application.Equities
{
    public class EquityService : IEquityService
    {

        private readonly IUnitOfWork<IEBrokerDbContext> _tradingUnitOfWork;
        public EquityService(IUnitOfWork<IEBrokerDbContext> tradingUnitOfWork)
        {
            _tradingUnitOfWork = tradingUnitOfWork;
        }

        public async Task<Equity> GetEquityDetailsByCode(string equityCode)
        {
            var equityDetails = await _tradingUnitOfWork.GetRepository<Equity>().GetAll().FirstOrDefaultAsync(x => x.EquityCode == equityCode);
            return equityDetails;
        }
        public async Task<Equity> GetEquityDetailsById(string equityId)
        {
            var equityDetails = await _tradingUnitOfWork.GetRepository<Equity>().GetAll().FirstOrDefaultAsync(x => x.EquityId == equityId);
            return equityDetails;
        }
    }
}
