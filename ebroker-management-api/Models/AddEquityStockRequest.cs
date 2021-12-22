using System.ComponentModel.DataAnnotations;

namespace EBroker.Management.Api.Models
{
    public class AddStockRequest
    {
        [Required]
        public int Quantity { get; set; }
    }
}
