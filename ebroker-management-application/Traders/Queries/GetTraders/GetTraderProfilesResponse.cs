using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Traders.Queries.GetTraders
{
    [ExcludeFromCodeCoverage]
    public class GetTraderProfilesResponse
    {
        public List<TraderProfileModel> Traders { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class TraderProfileModel
    {
        public string TraderId { get; set; }
        public string TraderCode { get; set; }
        public string TraderName { get; set; }
        public double Funds { get; set; }
        public List<TraderEquityDetailsModel> EquityDetails { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public partial class TraderEquityDetailsModel
    {
        public string EquityCode { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
