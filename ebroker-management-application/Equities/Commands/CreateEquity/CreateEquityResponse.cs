using EBroker.Management.Application.Shared.Models;

namespace EBroker.Management.Application.Equities.Commands.CreateEquity
{
    public class CreateEquityResponse
    {
        public string EquityId { get; set; }
        public CodeAndDescription<string, string> Status { get; set; }
    }
}
