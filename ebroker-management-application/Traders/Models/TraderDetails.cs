#nullable disable

using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Traders.Models
{
    [ExcludeFromCodeCoverage]
    public partial class TraderDetails
    {
        public string TraderId { get; set; }
        public string TraderCode { get; set; }
        public string TraderName { get; set; }
        public double Funds { get; set; }

    }
}
