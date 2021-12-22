using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Api
{
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
        public AppSettings()
        {

        }
        public AppSettings(IOptions<AppSettings> options)
        {
            ApplicationId = options.Value.ApplicationId;
        }
        public string ApplicationId { get; set; }
    }
}
