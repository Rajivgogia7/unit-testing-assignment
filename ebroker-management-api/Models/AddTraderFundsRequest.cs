using System.ComponentModel.DataAnnotations;

namespace EBroker.Management.Api.Models
{
    public class AddTraderFundsRequest
    {
        [Required]
        public double Funds { get; set; }
    }
}
