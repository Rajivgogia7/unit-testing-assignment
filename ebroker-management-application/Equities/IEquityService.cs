using EBroker.Management.Application.Equities.Models;
using EBroker.Management.Domain.Equity;
using System.Threading.Tasks;

namespace EBroker.Management.Application.Equities
{
    public interface IEquityService
    {
        Task<Equity> GetEquityDetailsByCode(string equitycode);
        Task<Equity> GetEquityDetailsById(string equityId);
    }
}
