using System.ComponentModel.DataAnnotations;

namespace EBroker.Management.Domain.Equity
{
    public class Equity
    {
        [Key]
        public string EquityId { get; set; }
        public string EquityCode { get; set; }
        public string EquityName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
