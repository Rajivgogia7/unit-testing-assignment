using EBroker.Management.Application.Shared.Models;

namespace EBroker.Management.Application.Equities.Models
{
    public static class AddStockStatus
    {
        public static readonly CodeAndDescription<string, string> VALID = new CodeAndDescription<string, string>("Success", " Stock is added to the equity successfully.");
        public static readonly CodeAndDescription<string, string> INVALID = new CodeAndDescription<string, string>("Failed", "Equity code is invalid.");
    }
}
