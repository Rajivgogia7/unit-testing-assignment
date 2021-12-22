using EBroker.Management.Application.Shared.Models;

namespace EBroker.Management.Application.Equities.Commands.AddStock
{
    public class AddStockResponse
    {
        public string EquityCode { get; set; }
        public double Quantity { get; set; }
        public CodeAndDescription<string, string> Status { get; set; }
    }
}
