using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Shared.RequestParameters
{
    [ExcludeFromCodeCoverage]
    public class DefaultRequestParameters
    {
        public string CorrelationId { get; set; }
        public string ApplicationId { get; set; }
    }
}
