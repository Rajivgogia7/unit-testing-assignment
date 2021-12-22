using EBroker.Management.Application.Shared.Models;

namespace EBroker.Management.Application.Traders.Commands.CreateTraderProfile
{
    public class CreateTraderProfileResponse
    {
        public string TraderId { get; set; }
        public CodeAndDescription<string, string> Status { get; set; }
    }
}
