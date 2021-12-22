using System.Diagnostics.CodeAnalysis;

namespace EBroker.Management.Application.Shared.DataError
{
    [ExcludeFromCodeCoverage]
    public static class DataErrorCodes
    {
        public static readonly DataErrorCode BadRequest = new DataErrorCode("BadRequest", "There are one or more errors with the data provided for this request.");
        public static readonly DataErrorCode InternalServer = new DataErrorCode("InternalServer", "Server encountered an issue trying to fulfil this request");
        public static readonly DataErrorCode MissingCorrelationId = new DataErrorCode("MissingCorrelationId", "Missing CorrelationId.");
        public static readonly DataErrorCode MissingApplicationId = new DataErrorCode("MissingApplicationId", "ApplicationId is mandatory.");
        public static readonly DataErrorCode RequiredField = new DataErrorCode("RequiredField", "Field is mandatory.");
        public static readonly DataErrorCode InvalidValue = new DataErrorCode("InvalidValue", "Value is invalid.");
        public static readonly DataErrorCode NotFound = new DataErrorCode("NotFound", "No data found.");
        public static readonly DataErrorCode TraderProfileAlreadyExists = new DataErrorCode("TraderProfileAlreadyExists", "Trader profile already exists for the trader code.");
    }
}
