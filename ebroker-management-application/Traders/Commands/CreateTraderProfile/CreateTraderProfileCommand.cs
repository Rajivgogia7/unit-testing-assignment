using EBroker.Management.Application.Shared.RequestParameters;
using MediatR;

namespace EBroker.Management.Application.Traders.Commands.CreateTraderProfile
{
    public class CreateTraderProfileCommand : DefaultRequestParameters, IRequest<CreateTraderProfileResponse>
    {
        public string TraderCode { get; set; }
        public string TraderName { get; set; }
        public double Funds { get; set; }
    }
}
