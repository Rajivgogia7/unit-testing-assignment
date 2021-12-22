using System.ComponentModel.DataAnnotations;

namespace EBroker.Management.Api.Models
{
    public class BuyEquityRequest
    {
        [Required]
        public string EquityCode { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
