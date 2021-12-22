using System.ComponentModel.DataAnnotations;

namespace EBroker.Management.Api.Models
{
    public class CreateTraderProfileRequest
    {
        [Required]
        public string TraderCode { get; set; }

        [Required]
        public string TraderName { get; set; }

        [Required]
        public double Funds { get; set; }
    }
}
