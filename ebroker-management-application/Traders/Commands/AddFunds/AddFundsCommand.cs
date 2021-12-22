using EBroker.Management.Application.Shared.RequestParameters;
using MediatR;

namespace EBroker.Management.Application.Traders.Commands.AddFunds
{
    public class AddFundsCommand : DefaultRequestParameters, IRequest<AddFundsResponse>
    {
        public string TraderCode { get; set; }
        public double Funds { get; set; }
    }
}
