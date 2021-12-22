using EBroker.Management.Application.Shared.RequestParameters;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Traders.Queries.GetTraders
{
    [ExcludeFromCodeCoverage]
    public class GetTraderProfilesQuery : DefaultRequestParameters, IRequest<GetTraderProfilesResponse>
    {
        public string TraderCode { get; set; }
    }
}
