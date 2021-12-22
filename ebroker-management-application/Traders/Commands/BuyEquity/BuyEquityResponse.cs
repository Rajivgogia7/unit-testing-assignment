using EBroker.Management.Application.Shared.Models;

namespace EBroker.Management.Application.Traders.Commands.BuyEquity
{
    public class BuyEquityResponse
    {
        public string EquityCode { get; set; }
        public double Quantity { get; set; }
        public CodeAndDescription<string, string> Status { get; set; }
    }
}
