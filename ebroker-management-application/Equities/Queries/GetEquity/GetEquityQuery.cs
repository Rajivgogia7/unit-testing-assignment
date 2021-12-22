using EBroker.Management.Application.Shared.RequestParameters;
using MediatR;

namespace EBroker.Management.Application.Equities.Queries.GetEquity
{
    public class GetEquityQuery : DefaultRequestParameters, IRequest<GetEquityResponse>
    {
        public string EquityCode { get; set; }
    }
}
