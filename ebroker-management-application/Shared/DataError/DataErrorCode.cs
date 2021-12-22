using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace EBroker.Management.Application.Shared.DataError
{
    [ExcludeFromCodeCoverage]
    public class DataErrorCode
    {
        public DataErrorCode(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; }
        public string Description { get; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
