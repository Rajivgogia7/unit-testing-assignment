using EBroker.Management.Application.Shared.RequestParameters;
using MediatR;

namespace EBroker.Management.Application.Traders.Commands.SellEquity
{
    public class SellEquityCommand : DefaultRequestParameters, IRequest<SellEquityResponse>
    {
        public string TraderCode { get; set; }
        public string EquityCode { get; set; }
        public int Quantity { get; set; }
    }
}
