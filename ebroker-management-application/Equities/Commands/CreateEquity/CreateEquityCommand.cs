using EBroker.Management.Application.Shared.RequestParameters;
using MediatR;

namespace EBroker.Management.Application.Equities.Commands.CreateEquity
{
    public class CreateEquityCommand : DefaultRequestParameters, IRequest<CreateEquityResponse>
    {
        public string EquityCode { get; set; }
        public string EquityName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
