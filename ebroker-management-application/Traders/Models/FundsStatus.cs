using EBroker.Management.Application.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.Management.Application.Traders.Models
{
    public static class FundsStatus
    {
        public static readonly CodeAndDescription<string, string> VALID = new CodeAndDescription<string, string>("Success", "Funds are added to the account successfully.");
        public static readonly CodeAndDescription<string, string> INVALID = new CodeAndDescription<string, string>("Failed", "The trader code is invalid.");
    }
}
