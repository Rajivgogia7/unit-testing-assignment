using EBroker.Management.Application.Traders.Models;
using System.Threading.Tasks;

namespace EBroker.Management.Application.Traders
{
    public interface ITraderService
    {
        Task<TraderDetails> GetTraderDetails(string tradercode);
    }
}
