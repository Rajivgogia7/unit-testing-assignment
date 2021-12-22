using EBroker.Management.Application.Shared.DataError;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace EBroker.Management.Api.RequestHeaderValidation
{
    [ExcludeFromCodeCoverage]
    public class HeadersValidationFailureResult : IActionResult
    {
        public HeadersValidationFailureResult(IEnumerable<DataErrorDetail> dataErrorDetails)
        {
            DataErrorDetails = dataErrorDetails;
        }

        public IEnumerable<DataErrorDetail> DataErrorDetails { get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var dataError = new DataError()
            {
                ErrorCode = DataErrorCodes.BadRequest.Code,
                Message = DataErrorCodes.BadRequest.Description,
                Details = DataErrorDetails.ToList(),
                Target = "headers"
            };

            var objectResult = new ObjectResult(dataError)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
