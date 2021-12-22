using EBroker.Management.Application.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.Management.Application.Traders.Models
{
    public static class TraderAccountCreationStatus
    {
        public static readonly CodeAndDescription<string, string> VALID = new CodeAndDescription<string, string>("Success", "The trader is added to the system successfully.");
        public static readonly CodeAndDescription<string, string> INVALID = new CodeAndDescription<string, string>("Failed", "The trader code is already existing in the system.");
    }
}
