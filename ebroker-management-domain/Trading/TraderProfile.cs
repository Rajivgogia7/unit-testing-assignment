using System.ComponentModel.DataAnnotations;

#nullable disable

namespace EBroker.Management.Domain.Trading
{
    public partial class TraderProfile
    {
        [Key]
        public string TraderId { get; set; }
        public string TraderCode { get; set; }
        public string TraderName { get; set; }
        public double Funds { get; set; }

    }
}
