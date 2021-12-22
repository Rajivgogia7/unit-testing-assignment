using EBroker.Management.Application.Shared.Models;

namespace EBroker.Management.Application.Traders.Commands.AddFunds
{
    public class AddFundsResponse
    {
        public string TraderCode { get; set; }
        public double Funds { get; set; }
        public CodeAndDescription<string, string> Status { get; set; }
    }
}
