using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EBroker.Management.Application.Shared.DataError
{
    [ExcludeFromCodeCoverage]
    public class DataErrorDetail
    {
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("target")]
        public string Target { get; set; }
    }
}
