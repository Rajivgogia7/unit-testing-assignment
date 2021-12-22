using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EBroker.Management.Domain.Trading
{
    public partial class TraderEquity
    {
        [Key]
        public string TraderEquityId { get; set; }
        public string TraderId { get; set; }
        public string EquityId { get; set; }
        public int Quantity { get; set; }
    }
}
