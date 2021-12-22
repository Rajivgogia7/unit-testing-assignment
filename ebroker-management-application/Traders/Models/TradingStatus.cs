using EBroker.Management.Application.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBroker.Management.Application.Traders.Models
{
    public static class TradingStatus
    {
        public static readonly CodeAndDescription<string, string> NOT_A_VALID_TIME_OR_DAY = new CodeAndDescription<string, string>("failed", "Equity can be purchased from 9 A.M. to 3.00 P.M from Monday to Friday.");
        public static readonly CodeAndDescription<string, string> EQUITY_NOT_SUFFICIENT = new CodeAndDescription<string, string>("failed", "Equities are not sufficient in the stock.");
        public static readonly CodeAndDescription<string, string> FUNDS_NOT_SUFFICIENT = new CodeAndDescription<string, string>("failed", "Funds are not sufficient to buy the equity.");
        public static readonly CodeAndDescription<string, string> EQUITY_NOT_SUFFICIENT_TRADER = new CodeAndDescription<string, string>("failed", "Equities are not sufficient in traders account.");
        public static readonly CodeAndDescription<string, string> SUCCESS = new CodeAndDescription<string, string>("success", "Transaction completed successfully.");
        public static readonly CodeAndDescription<string, string> INVALID_DETAILS = new CodeAndDescription<string, string>("failed", "Invalid details provided for the transaction.");
        public static readonly CodeAndDescription<string, string> FUNDS_NOT_SUFFICIENT_TRADER_SELL = new CodeAndDescription<string, string>("failed", "Funds are not sufficient to sell the equity as minimum brokerage amount is 20 rupees.");
    }
}
