using System.Collections.Generic;

namespace EBroker.Management.Application.Equities.Queries.GetEquity
{
    public class GetEquityResponse
    {
        public List<EquityModel> Equities { get; set; }
    }
    public class EquityModel
    {
        public string EquityId { get; set; }
        public string EquityCode { get; set; }
        public string EquityName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
