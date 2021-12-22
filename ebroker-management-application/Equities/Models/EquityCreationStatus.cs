using EBroker.Management.Application.Shared.Models;

namespace EBroker.Management.Application.Equities.Models
{
    public static class EquityCreationStatus
    {
        public static readonly CodeAndDescription<string, string> VALID = new CodeAndDescription<string, string>("Success", "Equity is added to the system successfully.");
        public static readonly CodeAndDescription<string, string> INVALID = new CodeAndDescription<string, string>("Failed", "Equity code already exists in the system.");
    }
}
