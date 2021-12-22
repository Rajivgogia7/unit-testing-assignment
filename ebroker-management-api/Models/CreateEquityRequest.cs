using System.ComponentModel.DataAnnotations;

namespace EBroker.Management.Api.Models
{
    public class CreateEquityRequest
    {
        [Required]
        public string EquityCode { get; set; }
                      
        [Required]    
        public string EquityName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
