using EBroker.Management.Application.Shared.RequestParameters;
using MediatR;

namespace EBroker.Management.Application.Traders.Commands.BuyEquity
{
    public class BuyEquityCommand : DefaultRequestParameters, IRequest<BuyEquityResponse>
    {
        public string TraderCode { get; set; }
        public string EquityCode { get; set; }
        public int Quantity { get; set; }
    }
}
