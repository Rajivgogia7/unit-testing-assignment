using EBroker.Management.Application.Shared.RequestParameters;
using MediatR;

namespace EBroker.Management.Application.Equities.Commands.AddStock
{
    public class AddStockCommand : DefaultRequestParameters, IRequest<AddStockResponse>
    {
        public string EquityCode { get; set; }
        public int Quantity { get; set; }
    }
}
